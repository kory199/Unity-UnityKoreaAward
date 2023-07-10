using APIServer.DbModel;

namespace APIServer.Services;

public interface IMemoryDb
{
    public void Init(String address);

    public Task<ErrorCode> RegistUserAsync(String id, String authToken);

    public Task<ErrorCode> CheckUserAuthAsync(String id, String authToken);

    public Task<(bool, AuthUser?)> GetUserAsync(String id);

    public Task<bool> SetUserReqLockAsync(String key);

    public Task<ErrorCode> DelectUserAsync(String id);

    public Task<bool> DelUserReqLockAsync(String key);
}