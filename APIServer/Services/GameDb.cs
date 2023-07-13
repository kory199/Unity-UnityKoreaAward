using APIServer.DbModel;
using Microsoft.Extensions.Options;
using ZLogger;

namespace APIServer.Services;

public class GameDb : BaseDb<GameData>, IGameDb
{
    public GameDb(ILogger<GameDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb, GameDbTable.player_data)
    {
    }

    public async Task<(ResultCode, GameData)> CreateDefaultGameData(Int64 account_id)
    {
        try
        {
            var gameData = new GameData(account_id);
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
    
    public async Task<(ResultCode,GameData)> VerifyGameData(Int64 account_id)
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