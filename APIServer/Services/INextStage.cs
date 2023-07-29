namespace APIServer.Services;

public interface INextStage
{
    public Task<ResultCode> LoadStageDataAsync();
}