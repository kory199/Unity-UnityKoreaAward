using APIServer.DbModel;
using APIServer.StateType;
using CloudStructures;
using CloudStructures.Structures;
using StackExchange.Redis;
using ZLogger;
using static APIServer.Services.RedisTimeSpan;

namespace APIServer.Services;

public class RedisDb : IMemoryDb
{
    public RedisConnection redisConn;

    private static readonly ILogger<RedisDb> s_logger = LogManager.GetLogger<RedisDb>();

    public void Init(string address)
    {
        var config = new RedisConfig("default", address);
        redisConn = new RedisConnection(config);

        s_logger.ZLogDebug($"UserDbAddress : {address}");
    }

    public async Task<ResultCode> RegistUserAsync(String id, String authToken, Int64 accountId)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ResultCode.None;

        var user = new AuthUser
        {
            Id = id,
            AuthToken = authToken,
            AccountId = accountId,
            State = UserState.Login.ToString(),
        };

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, LoginTimeSpan());

            if (await redis.SetAsync(user, LoginTimeSpan()) == false)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.LoginAddRedis],
                    $"ID : {id}, AuthToken : {authToken}, ErrorMessage : UserBasicAuth, RedisString Set Error");

                result = ResultCode.LoginFailAddRedis;
            }
        }
        catch (Exception e)
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.LoginAddRedis], e.Message,
                $"ID : {id}, AuthToken : {authToken}, ErrorMessage : Redis Connection Error");

            result = ResultCode.LoginFailAddRedisException;
        }

        return result;
    }

    public async Task<ResultCode> UpdateUserStateAsync(String id)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ResultCode.None;

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, LoginTimeSpan());
            var redisResult = await redis.GetAsync();

            if (redisResult.HasValue && redisResult.Value != null)
            {
                var user = redisResult.Value;
                user.State = UserState.GamePlay.ToString();

                if (await redis.SetAsync(user, LoginTimeSpan()) == false)
                {
                    s_logger.ZLogError(LogManager.EventIdDic[EventType.UpdateStatus],
                        $"ID : {id}, ErrorMessage : Redis UpdateStatus Error");

                    result = ResultCode.RedisUpdateStatusFail;
                }
            }
            else
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.UpdateStatus],
                        $"ID : {id}, ErrorMessage : User not found");

                result = ResultCode.RedisUserNotFound;
            }
        }   
        catch (Exception e)
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.UpdateStatus], e.Message,
                    $"ID : {id}, ErrorMessage : Redis UpdateStatus Error");

            return ResultCode.RedisUpdateStatusFailException;
        }

        return result;
    }

    public async Task<ResultCode> CheckUserAuthAsync(String id, String authToken)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ResultCode.None;

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, null);
            var user = await redis.GetAsync();

            if (user.HasValue == false)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                    $"[RedisDb.CheckUserAuthAsync] ID : {id}, AuthToken : {authToken}, " +
                    "ErrorMessage : ID Dose Not Exist");

                result = ResultCode.CheckAuthFailNotExist;

                return result;
            }

            if (user.Value.Id != id || user.Value.AuthToken != authToken)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                    $"[RedisDb.CheckUserAuthAsync], ID ; {id}, AuthToken : {authToken}, " +
                    "ErrorMessage : Wrong ID Or Auth Token");

                result = ResultCode.CheckAuthFailNotExist;
                return result;
            }

            return result;
        }
        catch (Exception e)
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.Login], e,
                $"[RedisDb.CheckUserAuthAsync], ID ; {id}, AuthToken : {authToken}, ErrorMessage : Redis Connection Error");

            result = ResultCode.CheckAuthFailNotExist;
            return result;
        }
    }

    public async Task<(bool, AuthUser?)> GetUserAsync(String id)
    {
        var uid = MemoryDbKeyMaker.MakeUIDKey(id);

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, uid, null);
            var user = await redis.GetAsync();

            if (user.HasValue == false)
            {
                s_logger.ZLogError($"RedisDb.GetUserAsync, UID : {uid}," +
                    "ErrorMessage = Not Assigned User, RedisStaring GET Error");

                return (false, null);
            }

            return (true, user.Value);
        }
        catch (Exception e)
        {
            s_logger.ZLogError(e, $"UID:{uid}, ErrorMessage:ID does Not Exist");

            return (false, null);
        }
    }

    public async Task<bool> SetUserReqLockAsync(String key)
    {
        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, NxKeyTimeSpan());

            if (await redis.SetAsync(new AuthUser { }, NxKeyTimeSpan(), StackExchange.Redis.When.NotExists) == false)
            {
                return false;
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ResultCode> DelectUserAsync(String id)
    {
        try
        {
            var key = MemoryDbKeyMaker.MakeUIDKey(id);
            var redis = new RedisString<AuthUser>(redisConn, key, LoginTimeSpan());
            var result = await redis.DeleteAsync();

            return ResultCode.None;
        }
        catch (Exception e)
        {
            s_logger.ZLogError(e, $"ErrorMessage : Redis Delect User Data !");

            return ResultCode.RedisDelectFailException;
        }
    }

    public async Task<ResultCode> PingAsync(String id)
    {
        var key = MemoryDbKeyMaker.MakePingkey(id);
        var redis = new RedisString<DateTime>(redisConn, key, null);
        
        try
        {
            await redis.SetAsync(DateTime.UtcNow, PingKeyTimeSpan());
            return ResultCode.None;
        }
        catch (RedisConnectionException e)
        {
            s_logger.ZLogError(e.Message, $"Redis connection exception for client: {id}");
            return ResultCode.RedisConnectionException;
        }
        catch (Exception e)
        {
            s_logger.ZLogError(e.Message, $"Ping failed for client: {id}");
            return ResultCode.RedisPingException;
        }
    }

    public async Task<bool> DelUserReqLockAsync(String key)
    {
        if (string.IsNullOrWhiteSpace(key))
        {
            return false;
        }

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, null);
            var redisResult = await redis.DeleteAsync();

            return redisResult;
        }
        catch
        {
            return false;
        }
    }
}