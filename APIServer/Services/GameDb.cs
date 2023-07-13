using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Services;

public class GameDb : BaseDb, IGameDb
{
    public GameDb(ILogger<GameDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb)
    {
    }

    public async Task<ResultCode> CreateDefaultGameData(Int64 account_id)
    {
        try
        {
            var gameData = new GameData 
            {
                player_uid = account_id,
            };

            var count = await _queryFactory
                .Query(GameDbTable.player_data)
                .InsertAsync(gameData);

            if(count != 1)
            {
                return ResultCode.CreateDefaultGameDataFailInsert;
            }

            return ResultCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e, 
                $"[GameDb.CreateDefaultGameData] ErrorCode : {ResultCode.CreateDefaultGameDataFailException}");

            return ResultCode.CreateDefaultGameDataFailException;
        }
    }
    
    public async Task<ResultCode> VerifyGameData(Int64 account_id)
    {
        try
        {
            var gameData = await _queryFactory
                .Query(GameDbTable.player_data)
                .Where(GameDbTable.player_uid, account_id)
                .FirstOrDefaultAsync<GameData>();

            if(gameData == null)
            {
                return ResultCode.PlayerGameDataNotFound;
            }

            return ResultCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.VerifyGameData] ErrorCode : {ResultCode.LoadGameDataFailException}");

            return ResultCode.LoadGameDataFailException;
        }
    }
        
    public async Task<ResultCode> DeleteGameData(Int64 account_id)
    {
        try
        {
            await _queryFactory
                .Query(GameDbTable.player_data)
                .Where(GameDbTable.player_uid, account_id)
                .DeleteAsync();

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.DeleteGameData] Error : {ResultCode.DeleteGameDataFailException}");

            return ResultCode.DeleteGameDataFailException;
        }
    }
}