using GameAPIServer.ReqResModel;

namespace GameAPIServer.Services;

public interface IGameDb
{
    public Task<ErrorCode> CreateDefaultGameData(Int64 uniqueKey);

    public Task<ErrorCode> VerifyGameData(Int64 uniqueKey);

    public Task<ErrorCode> DeleteGameData(Int64 uniqueKey);
}