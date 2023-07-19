using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Services;

public class GameDb : BaseDb<GameData>, IGameDb
{
    public GameDb(ILogger<GameDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb, GameDbTable.player_data)
    {
    }

    public async Task<(ResultCode, GameData?)> CreateDefaultGameData(Int64 account_id, String id)
    {
        try
        {
            var gameData = new GameData(account_id, id);
            await ExecuteInsertAsync(gameData);

            return (ResultCode.None, gameData);

        }
        catch(Exception e)
        {
            _logger.ZLogError(e, 
                $"[{GetType().Name}.CreateDefaultGameData] ErrorCode : {ResultCode.CreateDefaultGameDataFailException}");

            return (ResultCode.CreateDefaultGameDataFailException, null);
        }
    }
    
    public async Task<(ResultCode,GameData?)> VerifyGameData(Int64 account_id)
    {
        try
        {
            var gameData = await ExecuteGetByAsync(GameDbTable.player_uid, account_id);

            if (gameData == null)
            {
                return (ResultCode.PlayerGameDataNotFound, null);
            }

            return (ResultCode.None, gameData);
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.VerifyGameData] ErrorCode : {ResultCode.LoadGameDataFailException}");

            return (ResultCode.LoadGameDataFailException, null);
        }
    }

    public async Task<(ResultCode, List<Ranking>?)> LoadRankingDataAsync(Int64 account_id)
    {
        try
        {
            var gameDataList = await _queryFactory.Query(_tableName)
                .Select(GameDbTable.player_uid, GameDbTable.id, GameDbTable.score)
                .OrderByDesc(GameDbTable.score)
                .Limit(10)
                .GetAsync<Ranking>();

            List<Ranking> rankingList = new List<Ranking>();

            if (gameDataList == null)
            {
                return (ResultCode.LoadRankingDataFail, null);
            }

            int rank = 0;

            foreach (var gameData in gameDataList)
            {
                rank++;
                Ranking ranks = new Ranking
                {
                    id = gameData.id,
                    score = gameData.score,
                    ranking = rank
                };


                rankingList.Add(ranks);
            }

            return (ResultCode.None, rankingList);
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
               $"[{GetType().Name}.LoadRankingDataAsync] ErrorCode : {ResultCode.LoadRankingDataFailException}");

            return (ResultCode.LoadRankingDataFailException, null);
        }
    }

    public async Task<ResultCode> DeleteGameData(Int64 account_id)
    {
        try
        {
            await ExecuteDeleteAsync(GameDbTable.player_uid, account_id);

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.DeleteGameData] Error : {ResultCode.DeleteGameDataFailException}");

            return ResultCode.DeleteGameDataFailException;
        }
    }
}