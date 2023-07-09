using GameAPIServer.DBModel;
using GameAPIServer.StateType;

namespace GameAPIServer.Services;

public interface IMemoryDb
{
    public void Init(string address); 

    public Task<ErrorCode> RegistUserAsync(string id, string authToken, Int64 accountId, String userVersion);

    public Task<ErrorCode> RegistUserItemAsync(String id);

    public Task<ErrorCode> RegistUerNPCAsync(String id);

    public Task<ErrorCode> DelectUserAsync(String id);

    public Task<ErrorCode> CheckUserAuthAsync(string id, string authToken);

    public Task<ErrorCode> SaveNotice(); 

    public Task<(bool, AuthUser)> GetUserAsync(string id);

    public Task<(bool, String)> GetUserVersion(string id);

    public Task<(ErrorCode, String)> GetUserStateAsync(string id);

    public Task<ErrorCode> ChangegedUserState(String id);

    public Task<bool> SetUserReqLockAsync(string key);
    
    public Task<bool> DelUserReqLockAsync(string key);
}