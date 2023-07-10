namespace APIServer.DbModel;

public class AuthUser
{
    public String Id { get; set; } = "";
    public String AuthToken { get; set; } = "";
    public Int64 AccountId { get; set; } = 0;
    public String State { get; set; } = "";
}

public class RediskeyExpireTime
{
    public const ushort NxKeyExpireSecond = 3;
    public const ushort RedisKeyExpireSecong = 6000;
    public const ushort LoginKeyExpireMin = 60;
    public const ushort TicketKeyExpireSecond = 6000;
}