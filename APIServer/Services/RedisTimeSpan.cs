using APIServer.DbModel;

namespace APIServer.Services;

public class RedisTimeSpan
{
    public static TimeSpan LoginTimeSpan() => TimeSpan.FromMinutes(RediskeyExpireTime.LoginKeyExpireMin);

    public TimeSpan TicketKeyTimeSpan() => TimeSpan.FromSeconds(RediskeyExpireTime.TicketKeyExpireSecond);

    public static TimeSpan NxKeyTimeSpan() => TimeSpan.FromSeconds(RediskeyExpireTime.NxKeyExpireSecond);

    public static TimeSpan PingKeyTimeSpan() => TimeSpan.FromMinutes(RediskeyExpireTime.PingKeyExpireMin);
}