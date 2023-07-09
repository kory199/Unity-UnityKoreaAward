using System.Data;
using System.Reflection.Emit;
using GameAPIServer.DBModel;
using GameAPIServer.StateType;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace GameAPIServer.Services;

public class DungeonStageDb : IDungeonStage
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<DungeonStageDb> _logger;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public static List<DungeonStage> DungeonStagesList;
    public static List<Item> DungeonStageItemList;
    public static List<StageAttackNpc> DungeonStageNPCList;

    public static Int64 finalStage;

    public DungeonStageDb(ILogger<DungeonStageDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();
        DungeonStagesList = new List<DungeonStage>();
        DungeonStageItemList = new List<Item>();
        DungeonStageNPCList = new List<StageAttackNpc>();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> CreateStageData(Int64 accountId, Int64 stageid)
    {
        try
        {
            var count = await _queryFactory.Query(GameDbTable.player_data).InsertAsync(new
            {
                player_id = accountId,
                stage_id = stageid
            });

            if (count != 1)
            {
                return ErrorCode.CreateGameDataFailInsert;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[DungeonStageDb.CreateStageData] ErrorCode :{ErrorCode.CreateGameDataFailException}");

            return ErrorCode.CreateGameDataFailException;
        }
    }

    public async Task<ErrorCode> VerifyStageData(Int64 userId, Int64 stageId)
    {
        try
        {
            var stageInfo = await _queryFactory.Query(GameDbTable.dungeon_stage)
                .Where(DbColumn.player_id, userId)
                .Where("stage_id", stageId)
                .GetAsync<DungeonStage>();

            Console.WriteLine($"stageInfo.Count {stageInfo.Count()}");

            if (stageInfo == null || stageInfo.Count() == 0)
            {
                return ErrorCode.LoadStageFail;
            }

            _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadGameData], $"UserID : {userId}");

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[DungeonStageDb.VerifyStageData] ErrorCode:{ErrorCode.LoadGameDataFailException}, UserID:{userId}");

            return ErrorCode.LoadGameDataFailException;
        }
    }

    public async Task<ErrorCode> CheckStage(Int64 userId, Int64 stageId)
    {
        try
        {
            var stageInfo = GetList();
            var listcheck = stageInfo.Where(stageid => stageid.stage_id == stageId).FirstOrDefault();

            if (listcheck == null)
            {
                return ErrorCode.None;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[DungeonStageDb.CheckStage] ErrorCode:{ErrorCode.StageSelectionFailException}, player_id:{userId}");

            return ErrorCode.StageSelectionFailException;
        }
    }

    public async Task<IEnumerable<DungeonStage>> LoadStageDataAsync(Int64 player_id)
    {
        var query = _queryFactory.Query(GameDbTable.dungeon_stage).Select("player_id", "stage_id").OrderBy("stage_id");
        var stage = await query.GetAsync<DungeonStage>();

        DungeonStagesList = stage.ToList();
        Console.WriteLine($"DungeonStagesList Count : {DungeonStagesList.Count()}");
        return DungeonStagesList;
    }

    public async Task<ErrorCode> GetStageItem (Int64 stage_id)
    {
        try
        {
            // 마지막 클리어 스테이지 번호
            if(stage_id > GetFinalStage())
            {
                return ErrorCode.StageItemStageCodeOverfail;
            }

            Item one;
            Item two;
  
            if(stage_id == 1)
            {
                one = MasterDataDb.itemList[0];
                two = MasterDataDb.itemList[1];

                StageAttackNpc npcone = MasterDataDb.stageAttackNPCList[0];
                StageAttackNpc npctwo = MasterDataDb.stageAttackNPCList[1];
                DungeonStageNPCList.Add(npcone);
                DungeonStageNPCList.Add(npctwo);
            }
            else
            {
                one = MasterDataDb.itemList[2];
                two = MasterDataDb.itemList[2];
                StageAttackNpc npcthree = MasterDataDb.stageAttackNPCList[2];
                StageAttackNpc npcfour = MasterDataDb.stageAttackNPCList[3];
                StageAttackNpc npcfive = MasterDataDb.stageAttackNPCList[4];
                DungeonStageNPCList.Add(npcthree);
                DungeonStageNPCList.Add(npcfour);
                DungeonStageNPCList.Add(npcfive);
            }

            DungeonStageItemList.Add(one);
            DungeonStageItemList.Add(two);
            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[DungeonStageDb.GetStageItem] ErrorCode:{ErrorCode.LoadGameDataFailException}");

            return ErrorCode.LoadStageItemFail;
        }
    }

    public Int64 GetFinalStage()
    {
        var lastStage = DungeonStagesList[DungeonStageItemList.Count - 1];
        Console.WriteLine($" lastStage id :{ lastStage.stage_id}");

        finalStage = lastStage.stage_id;
        return finalStage;
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);
        _dbConn.Open();
    }

    public List<DungeonStage> GetList() => DungeonStagesList;
    public List<Item> GetItemList() => DungeonStageItemList;
    public List<StageAttackNpc> GetNPCList() => DungeonStageNPCList;

    private void Close() => _dbConn.Close();
    public void Dispose() => Close();
}