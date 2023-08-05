using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Services;

public class GameDb : BaseDb<GameData>, IGameDb
{
    public GameDb(ILogger<GameDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb, GameDbTable.player_data)
    {
    }

    public async Task<(ResultCode, GameData?)> CreateDefaultGameData(Int64 account_id, String _id)
    {
        try
        {
            var gameData = new GameData
            {
                player_uid = account_id,
                id = _id,
                exp = 1,
                hp = 10,
                score = 0,
                level = 0,
                status = 0
            };

            await ExecuteInsertAsync(gameData);
            return (ResultCode.None, gameData);

        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.CreateDefaultGameData] ErrorCode : {ResultCode.CreateDefaultGameDataFailException}");

            return (ResultCode.CreateDefaultGameDataFailException, null);
        }
    }

    public async Task<(ResultCode, GameData?)> VerifyGameData(Int64 account_id)
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
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.VerifyGameData] ErrorCode : {ResultCode.LoadGameDataFailException}");

            return (ResultCode.LoadGameDataFailException, null);
        }
    }

    public async Task<(ResultCode, GameData?)> UpdataScoreDataAsync(Int64 account_id, int newscore)
    {
        try
        {
            var scoreUpdata = await _queryFactory.Query(_tableName)
                .Where(GameDbTable.player_uid, account_id)
                .UpdateAsync(new
                {
                    score = newscore,
                    created_at = DateTime.Now
                });

            if (scoreUpdata == 0)
            {
                _logger.ZLogError($"[{GetType().Name}.UpdataScoreDataAsync] ErrorCode : {ResultCode.UpdateScoreDataFail}");
                return (ResultCode.UpdateScoreDataFail, null);
            }

            var updatedData = await _queryFactory.Query(_tableName)
            .Where(GameDbTable.player_uid, account_id)
            .FirstOrDefaultAsync<GameData>();

            if (updatedData == null)
            {
                _logger.ZLogError($"[{GetType().Name}.UpdataScoreDataAsync] ErrorCode : {ResultCode.UpdateScoreDataFailException}");
                return (ResultCode.UpdateScoreDataNullException, null);
            }

            return (ResultCode.None, updatedData);
        }
        catch(Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.UpdataScoreDataAsync] ErrorCode : {ResultCode.UpdateScoreDataFailException}");

            return (ResultCode.UpdateScoreDataFailException, null);
        }
    }

    public async Task<(ResultCode, List<Ranking>?)> LoadRankingDataAsync(Int64 account_id, String id)
    {
        try
        {
            var gameDataList = (await _queryFactory.Query(_tableName)
                        .Select(GameDbTable.player_uid, GameDbTable.id, GameDbTable.score, GameDbTable.created_at)
                        .OrderByDesc(GameDbTable.score)
                        .OrderBy(GameDbTable.created_at)
                        .Limit(10)
                        .GetAsync<Ranking>()).ToList();

            List<Ranking> rankingList = new List<Ranking>();

            if (gameDataList == null)
            {
                return (ResultCode.LoadRankingDataFail, null);
            }

            bool isUserInList = false;
            for (int rank = 0; rank < gameDataList.Count; rank++)
            {
                Ranking ranks = new Ranking
                {
                    id = gameDataList[rank].id,
                    score = gameDataList[rank].score,
                    ranking = rank + 1
                };

                if (gameDataList[rank].id == id)
                {
                    isUserInList = true;
                }

                rankingList.Add(ranks);
            }

            if (isUserInList == false)
            {
                var userData = await ExecuteGetByAsync(GameDbTable.player_uid, account_id);

                if (userData != null)
                {
                    Ranking ranks = new Ranking
                    {
                        id = userData.id,
                        score = userData.score,
                        ranking = 0
                    };

                    rankingList.Add(ranks);
                }
                else
                {
                    return (ResultCode.LoadRankingDataforUserFail, null);
                }
            }

            return (ResultCode.None, rankingList);
        }
        catch (Exception e)
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