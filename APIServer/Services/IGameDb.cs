using APIServer.DbModel;

namespace APIServer.Services;

public interface IGameDb
{
    public Task<(ResultCode, GameData?)> CreateDefaultGameData(Int64 account_id, String id);

    public Task<(ResultCode, GameData?)> VerifyGameData(Int64 account_id);

    public Task<(ResultCode, GameData?)> UpdataScoreDataAsync(Int64 account_id, int newscore);

    public Task<(ResultCode, List<Ranking>?)> LoadRankingDataAsync(Int64 account_id, String id);

    public Task<(ResultCode, Int32 statusnum)> CheckStatusAsync(Int64 account_id);

    public Task<ResultCode> StatusChangedAsync(Int64 account_id);

    public Task<ResultCode> DeleteGameData(Int64 account_id);
}