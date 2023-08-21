namespace APIServer.Services;

public interface IMasterDataDb
{
    public Task<ResultCode> LoadAllMasterDataAsync();

    public Task<(ResultCode, String?)> VerifyGameVersionAsync();

    public Task<ResultCode> VerifyMonsterDataAsync();

    public Task<ResultCode> VerifyPlayerStatusAsync();

    public Task<ResultCode> VerifyStageSpawnAsync();
}