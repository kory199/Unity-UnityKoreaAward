using System.Data;
using GameAPIServer.DBModel;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;

namespace GameAPIServer.Services;

public class MasterDataDb : IMasterDataDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<MasterDataDb> _logger;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public static List<Item> itemList;
    public static List<ItemAttribute> itemAttributeList;
    public static List<AttendanceReward> attendanceRewardList;
    public static List<InAppProduct> inAppProductList;
    public static List<StageItem> stageItemList;
    public static List<StageAttackNpc> stageAttackNPCList;

    public MasterDataDb (ILogger<MasterDataDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        ListReset();
        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task LoadMasterDataAsync()
    {
        await Task.WhenAll(
            LoadItemDataAsync(),
            LoadItemAttributeDataAsync(),
            LoadAttendanceDataAsync(),
            LoadInAppProductDataAsync(),
            LoadStageItemDataAsync(),
            LoadStageAttackNPCAsync()
        );
    }

    private async Task<IEnumerable<Item>> LoadItemDataAsync()
    {
        var query = _queryFactory.Query(MasterDataTable.item)
            .Select(DbColumn.code, DbColumn.name, DbColumn.attribute, "use_lv", "attack", "defence", "magic", "enhance_max_count" )
            .OrderBy(DbColumn.code);
        var items = await query.GetAsync<Item>();

        itemList = items.ToList();

        foreach( var item in itemList)
        {
            //Console.WriteLine($"ITEM) name :{item.name} attack : {item.attack}, enhance_max_count : {item.enhance_max_count}");
        }

        //Console.WriteLine($"Item List count : {itemList.Count()}");
        return items;
    }

    private async Task<IEnumerable<ItemAttribute>> LoadItemAttributeDataAsync()
    {
        var query = _queryFactory.Query(MasterDataTable.item_attribute).Select(DbColumn.code, DbColumn.name).OrderBy(DbColumn.code);
        var itemAttributes = await query.GetAsync<ItemAttribute>();

        itemAttributeList = itemAttributes.ToList();
        //Console.WriteLine($"itemAttribute List count : {itemAttributeList.Count()}");
        return itemAttributes;
    }

    private async Task<IEnumerable<AttendanceReward>> LoadAttendanceDataAsync()
    {
        var query = _queryFactory.Query(MasterDataTable.attendance_reward).Select(DbColumn.code, DbColumn.item_code, DbColumn.count).OrderBy(DbColumn.code);
        var attendanceRewards = await query.GetAsync<AttendanceReward>();

        attendanceRewardList = attendanceRewards.ToList();
        //Console.WriteLine($"attendanceReward List count : {attendanceRewardList.Count()}");
        return attendanceRewards;
    }

    private async Task<IEnumerable<InAppProduct>> LoadInAppProductDataAsync()
    {
        var query = _queryFactory.Query(MasterDataTable.in_app_product).Select(DbColumn.code, DbColumn.item_code, DbColumn.item_name, DbColumn.item_count).OrderBy(DbColumn.code);
        var inAppProducts = await query.GetAsync<InAppProduct>();

        inAppProductList = inAppProducts.ToList();
        //Console.WriteLine($"inAppProduct List count : {inAppProductList.Count()}");
        return inAppProducts;
    }

    async Task<IEnumerable<StageItem>> LoadStageItemDataAsync()
    {
        var query = _queryFactory.Query(MasterDataTable.stage_item).Select(DbColumn.code, DbColumn.item_code).OrderBy(DbColumn.code);
        var stageItems = await query.GetAsync<StageItem>();

        stageItemList = stageItems.ToList();
        //Console.WriteLine($"stageItem List count : {stageItemList.Count()}");
        return stageItems;
    }

    private async Task<IEnumerable<StageAttackNpc>> LoadStageAttackNPCAsync()
    {
        var query = _queryFactory.Query(MasterDataTable.stage_attack_npc).Select(DbColumn.code, DbColumn.count, DbColumn.exp).OrderBy(DbColumn.code);
        var stageAttackNpcs = await query.GetAsync<StageAttackNpc>();

        stageAttackNPCList = stageAttackNpcs.ToList();
        //Console.WriteLine($"stageAttackNPC List count : {stageAttackNPCList.Count()}");
        return stageAttackNpcs;
    }

    private void ListReset()
    {
        itemList = new List<Item>();
        itemAttributeList = new List<ItemAttribute>();
        attendanceRewardList = new List<AttendanceReward>();
        inAppProductList = new List<InAppProduct>();
        stageItemList = new List<StageItem>();
        stageAttackNPCList = new List<StageAttackNpc>();
    }

    private void Open ()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.MasterDataDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    private void DisPose() => Close();
}