using CloudStructures;
using CloudStructures.Structures;
using GameAPIServer.DBModel;
using GameAPIServer.StateType;
using ZLogger;

namespace GameAPIServer.Services;

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

    public async Task<ErrorCode> RegistGestAsync()
    {
 
        return ErrorCode.None;
    }

    public async Task<ErrorCode> RegistUserAsync(String id, String authToken, Int64 accountId, String userVersion)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ErrorCode.None;

        var user = new AuthUser
        {
            Id = id,
            AuthToken = authToken,
            AccountId = accountId,
            UserVersion = userVersion,
            State = UserState.Default.ToString()
        };

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, LoginTimeSpan());

            if (await redis.SetAsync(user, LoginTimeSpan()) == false)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.LoginAddRedis],
                    $"ID:{id}, AuthToken:{authToken}, ErrorMessage:UserBasicAuth, RedisString set Error");

                result = ErrorCode.LoginFailAddRedis;
            }
        }
        catch
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.LoginAddRedis],
                $"ID:{id}, AuthToken:{authToken}, ErrorMessage :Redis Connection Error");

            result = ErrorCode.LoginFailAddRedisException;
        }

        return result;
    }

    public async Task<ErrorCode> RegistUserItemAsync(String id)
    {
        var key = MemoryDbKeyMaker.MakeItemKey(id);
        var result = ErrorCode.None;

        var userItemList = DungeonStageDb.DungeonStageItemList;

        try
        {
            var redis = new RedisList<Item>(redisConn, key, LoginTimeSpan());
            await redis.RightPushAsync(userItemList);
        }
        catch
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.GamePlay],$"ID:{id}, ErrorMessage :Redis ItemList Save Error");

            result = ErrorCode.LoginFailAddRedisException;
        }

        return result;
    }

    public async Task<ErrorCode> RegistUerNPCAsync(String id)
    {
        var key = MemoryDbKeyMaker.MakeItemKey(id);
        var result = ErrorCode.None;

        var userNpcList = DungeonStageDb.DungeonStageNPCList;

        try
        {
            var redis = new RedisList<StageAttackNpc>(redisConn, key, LoginTimeSpan());
            await redis.RightPushAsync(userNpcList);
        }
        catch
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.GamePlay], $"ID:{id}, ErrorMessage :Redis ItemList Save Error");

            result = ErrorCode.LoginFailAddRedisException;
        }

        return result;

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
        catch(Exception e)
        {
            s_logger.ZLogError("ErrorMessage : Redis Delect User Play Data");

            return ErrorCode.RedisFailException;

        }
    }

    public async Task<ErrorCode> SaveNotice()
    {
        try
        {
            var key = "Notice";

            string notice = Notice.LoadNotice();
            var redis = new RedisString<String>(redisConn, key, LoginTimeSpan());

            if (await redis.SetAsync(notice, LoginTimeSpan()) == false)
            {
                s_logger.ZLogError($"Not Save Notice");
                return ErrorCode.RedisFailException;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                $"RedisDb.SaveNotice, ErrorMessage: Redis SaveNotice Error");

            return ErrorCode.RedisFailException;
        }
    }

    public async Task<ErrorCode> CheckUserAuthAsync(string id, string authToken)
    {
        var key = MemoryDbKeyMaker.MakeUIDKey(id);

        var result = ErrorCode.None;

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, null);
            var user = await redis.GetAsync();

            if (user.HasValue == false)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                    $"RedisDb.CheckUserAuthAsync, ID:{id}, AuthToken:{authToken}, ErrorMessage: ID does Not Exist");

                result = ErrorCode.CheckAuthFailNotExist;

                return result;
            }

            if (user.Value.Id != id || user.Value.AuthToken != authToken)
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                    $"RedisDb.CheckUserAuthAsync, ID:{id}, AuthToken:{authToken}, ErrorMessage: Wrong ID or Auth Token");

                result = ErrorCode.CheckAuthFailNotMatch;
                return result;
            }
        }
        catch
        {
            s_logger.ZLogError(LogManager.EventIdDic[EventType.Login],
                $"RedisDb.CheckUSerAuthAsync, ID:{id}, AuthToken:{authToken}, ErrorMessage: Redis Connection Error");

            result = ErrorCode.CheckAuthFailException;
            return result;
        }
        return result;
    }

    public async Task<(bool, AuthUser)> GetUserAsync(string id)
    {
        var uid = MemoryDbKeyMaker.MakeUIDKey(id);

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, uid, null);

            var user = await redis.GetAsync();

            if (user.HasValue == false)
            {
                s_logger.ZLogError($"RedisDb.GetUserAsync, UID:{uid}, ErrorMessage = Not Assigned User, RedisStaring get Error");

                return (false, null);
            }

            return (true, user.Value);
        }
        catch
        {
            s_logger.ZLogError($"UID:{uid}, ErrorMessage:ID does Not Exist");

            return (false, null);
        }
    }

    public async Task<(ErrorCode, String)> GetUserStateAsync(string id)
    {
        var uid = MemoryDbKeyMaker.MakeUIDKey(id);

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, uid, null);

            var user = await redis.GetAsync();

            if (user.HasValue == false)
            {
                s_logger.ZLogError($"RedisDb.GetUserAsync, UID:{uid},Redis Error");

                return (ErrorCode.RedisFailException, "");
            }

            return (ErrorCode.None, user.Value.State);
        }
        catch
        {
            s_logger.ZLogError($"UID:{uid}, ErrorMessage:ID does Not Exist");

            return (ErrorCode.RedisFailException, "");
        }
    }

    public async Task<(bool, String)> GetUserVersion(String id)
    {
        var uid = MemoryDbKeyMaker.MakeUIDKey(id);

        try
        {
            var redis = new RedisString<AuthUser>(redisConn, uid, null);
            var user = await redis.GetAsync();

            if (user.HasValue == false)
            {
                s_logger.ZLogError($"RedisDb.GetUserVersion, UID:{uid}, ErrorMessage = Not Assigned User, RedisStaring get Error");

                return (false, null);
            }

            return (true, user.Value.UserVersion);
        }
        catch
        {
            s_logger.ZLogError($"UID : {uid}, ErrorMessage : User Version does Not Exist ");

            return (false, null);
        }
    }

    public async Task<ErrorCode> ChangegedUserState(String id)
    {
        var uid = MemoryDbKeyMaker.MakeUIDKey(id);
        var result = ErrorCode.None;

        var redis = new RedisString<AuthUser>(redisConn, uid, null);
        var userinfo = await redis.GetAsync();

        if (redis.GetAsync() != null)
        {
            userinfo.Value.State = UserState.Playing.ToString();

            try
            {
                await redis.SetAsync(userinfo.Value);
            }
            catch
            {
                s_logger.ZLogError(LogManager.EventIdDic[EventType.GamePlay],
                    $"ID:{id}, ErrorMessage: User State Update set Error");

                result =  ErrorCode.UserStateUpdataFail;
            }
        }

        return result;
    }

    public async Task<bool> SetUserReqLockAsync(string key)
    {
        try
        {
            var redis = new RedisString<AuthUser>(redisConn, key, NxKeyTimeSpan());

            if (await redis.SetAsync(new AuthUser { }, NxKeyTimeSpan(), StackExchange.Redis.When.NotExists) == false)
            {
                return false;
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    public async Task<bool> DelUserReqLockAsync(string key)
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

    public TimeSpan LoginTimeSpan() => TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);

    public TimeSpan TicketKeyTimeSpan() => TimeSpan.FromSeconds(RediskeyExpireTime.TicketKeyExpireSecond);

    public TimeSpan NxKeyTimeSpan() => TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);
}