using GameAPIServer.DBModel;
using GameAPIServer.StateType;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using System.Reflection.PortableExecutable;
using ZLogger;

namespace GameAPIServer.Services;

public class GameDb : IGameDb
{
    private readonly ILogger<GameDb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public GameDb(ILogger<GameDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> CreateDefaultGameData(Int64 accountId)
    {
        try
        {
            var count = await _queryFactory.Query(GameDbTable.player_data).InsertAsync(new
            {
                player_id = accountId,
                exp = 100,
                level = 1,
                hp = 50,
                mp = 50,
                gold = 10,
            });

            if(count != 1)
            {
                return ErrorCode.CreateGameDataFailInsert;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.CreateDefaultGameData] ErrorCode :{ErrorCode.CreateGameDataFailException}");

            return ErrorCode.CreateGameDataFailException;
        }
    }

    public async Task<ErrorCode> VerifyGameData(Int64 userId)
    {
        try
        {
            var gameDataInfo = await _queryFactory.Query(GameDbTable.player_data).Where(DbColumn.player_id, userId).FirstOrDefaultAsync<LoadGameDataInfo>();

            if (gameDataInfo == null || gameDataInfo.player_id == 0)
            {
                //return new Tuple<ErrorCode, Int32>(ErrorCode.UserGameInfoNotFound, 0);
                return ErrorCode.UserGameInfoNotFound;
            }

            _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadGameData], $"exp : {gameDataInfo.exp}, Level : {gameDataInfo.Level}, Hp : {gameDataInfo.Hp}, Mp : {gameDataInfo.Mp}, Gold : {gameDataInfo.Gold}, [ LoadGameData Ok ]");

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.VerifyGameData] ErrorCode:{ErrorCode.LoadGameDataFailException}, player_id:{userId}");

            return ErrorCode.LoadGameDataFailException;
        }
    }

    // 해당 유저 쿼리를 찾아와서 리스트에 넣기

    // 유저 스테이지 레벨 변, 스테이지 테이블에 과 같이 변경 진행 

    public async Task<ErrorCode> UpdateGold(Int64 userId, Int64 gold)
    {
        try
        {
            var query = await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, userId)
                .UpdateAsync(new{gold = gold});

            if(query == 0)
            {
                return ErrorCode.UserGoldUpdateFail;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.UpdateGold] ErrorCode : {ErrorCode.UserGoldUpdateFailException}");

            return ErrorCode.UserGoldUpdateFailException;
        }
    }

    public async Task<ErrorCode> UpdateUserGameData(Int64 userId, string colunm, Int64 newdata)
    {
        try
        {
            var query = await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, userId)
                .UpdateAsync(new { colunm = newdata });

            if (query == 0)
            {
                return ErrorCode.UpdatUserGameDataFail;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.UpdateGold] ErrorCode : {ErrorCode.UpdatUserGameDataFailException}");

            return ErrorCode.UpdatUserGameDataFailException;
        }
    }

    public async Task<ErrorCode> DeleteGameData(Int64 userId)
    {
        try
        {
            await _queryFactory.Query(GameDbTable.player_data).Where(DbColumn.player_id, userId).DeleteAsync();

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.DeleteGameData] ErrorCode : {ErrorCode.DeleteGameDataFail}");

            return ErrorCode.DeleteGameDataFail;
        }
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    public void Dispose() => Close();
}