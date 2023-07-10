using APIServer.DbModel;
using APIServer.StateType;
using CloudStructures;
using CloudStructures.Structures;
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

    public async Task<ErrorCode> RegistUserAsync(String id, String authToken)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ErrorCode.None;

        var user = new AuthUser
        {
            Id = id,
            AuthToken = authToken,
            //AccountId = accountId,
            State = UserState.Default.ToString(),
        };

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, LoginTimeSpan());
        
            if(await redis.SetAsync(user, LoginTimeSpan()) == false)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.LoginAddRedis],
                    $"ID : {id}, AuthToken : {authToken}, ErrorMessage : UserBasicAuth, RedisString Set Error");

                result = ErrorCode.LoginFailAddRedis;
            }
        }
        catch (Exception e) 
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.LoginAddRedis], e,
                $"ID : {id}, AuthToken : {authToken}, ErrorMessage : Redis Connection Error");

            result = ErrorCode.LoginFailAddRedisException;
        }

        return result;
    }

    public async Task<ErrorCode> CheckUserAuthAsync(string id, string authToken)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ErrorCode.None;

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, null);
            var user = await redis.GetAsync();

            if(user.HasValue == false)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                    $"[RedisDb.CheckUserAuthAsync] ID : {id}, AuthToken : {authToken}, " +
                    "ErrorMessage : ID Dose Not Exist");

                result = ErrorCode.CheckAuthFailNotExist;

                return result;
            }

            if(user.Value.Id != id || user.Value.AuthToken != authToken)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                    $"[RedisDb.CheckUserAuthAsync], ID ; {id}, AuthToken : {authToken}, " +
                    "ErrorMessage : Wrong ID Or Auth Token");

                result = ErrorCode.CheckAuthFailNotExist;
                return result;
            }

            return result;
        }
        catch (Exception e) 
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.Login], e,
                $"[RedisDb.CheckUserAuthAsync], ID ; {id}, AuthToken : {authToken}, ErrorMessage : Redis Connection Error");

            result = ErrorCode.CheckAuthFailNotExist;
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

            if(user.HasValue == false)
            {
                s_logger.ZLogError($"RedisDb.GetUserAsync, UID : {uid}," +
                    "ErrorMessage = Not Assigned User, RedisStaring GET Error");

                return (false, null);
            }

            return (true, user.Value);
        }
        catch(Exception e) 
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

            if(await redis.SetAsync(new AuthUser { }, NxKeyTimeSpan(), StackExchange.Redis.When.NotExists) == false)
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

    public async Task<ErrorCode> DelectUserAsync(String id)
    {
        try
        {
            var key = MemoryDbKeyMaker.MakeUIDKey(id);
            var redis = new RedisString<AuthUser>(redisConn, key, LoginTimeSpan());
            var result = await redis.DeleteAsync();

            return ErrorCode.None;
        }
        catch (Exception e) 
        {
            s_logger.ZLogError(e, $"ErrorMessage : Redis Delect User Data !");

            return ErrorCode.RedisDelectFailException;
        }
    }

    public async Task<bool> DelUserReqLockAsync(String key)
    {
        if(string.IsNullOrWhiteSpace(key))
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