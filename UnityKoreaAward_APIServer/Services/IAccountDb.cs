namespace UnityKoreaAward_APIServer.Services;

public interface IAccountDb
{
    public Task<ErrorCode> CreateAccountAsync(String id, String pw);
    public Task<ErrorCode> VerifyAccountAsync(String id, String pw);
}
