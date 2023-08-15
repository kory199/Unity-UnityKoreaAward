namespace APIServer.Services;

public interface IAccountDb
{
    public Task<ResultCode> CreateAccountAsync(String id, String pw);

    public Task<(ResultCode, Int64)> VerifyAccountAsync(String id, String pw);
}