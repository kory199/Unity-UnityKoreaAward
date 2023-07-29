namespace APIServer.Services;

public interface IMasterDataDb
{
    public Task<(ResultCode, String?)> VerifyGmaeVersionAsync();
}