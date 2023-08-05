namespace APIServer.Services;

public interface IMasterDataDb
{
    public Task<ResultCode> LoadAllMasterDataAsync();

    public Task<(ResultCode, String?)> VerifyGmaeVersionAsync();

    public Task<ResultCode> VerifyMonsterDataAsync();

    public Task<ResultCode> VerifyPlayerStatusAsync();

    public Task<ResultCode> VerifyStageSpawnAsync();
}