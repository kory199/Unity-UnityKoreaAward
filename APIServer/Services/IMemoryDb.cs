using APIServer.DbModel;

namespace APIServer.Services;

public interface IMemoryDb
{
    public void Init(String address);

    public Task<ResultCode> RegistUserAsync(String id, String authToken, Int64 accountId);

    public Task<ResultCode> CheckUserAuthAsync(String id, String authToken);

    public Task<(bool, AuthUser?)> GetUserAsync(String id);

    public Task<bool> SetUserReqLockAsync(String key);

    public Task<ResultCode> DelectUserAsync(String id);

    public Task<bool> DelUserReqLockAsync(String key);
}