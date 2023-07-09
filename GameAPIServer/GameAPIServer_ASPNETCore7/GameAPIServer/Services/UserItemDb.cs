using System.Data;
using System.Reflection.PortableExecutable;
using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.StateType;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata;
using SqlKata.Execution;
using ZLogger;

namespace GameAPIServer.Services;

public class UserItemDb : IUserItemDb
{
    private readonly ILogger<UserItemDb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public static List<GetAttendanceItem> playerMailItemList;
    public static List<GetInAppItem> playerMailInAppItemLsit;
    public static List<ItemDataInfo> playerItemList;

    public UserItemDb(ILogger<UserItemDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        playerMailItemList = new List<GetAttendanceItem>();
        playerMailInAppItemLsit = new List<GetInAppItem>();
        //playerItemList = new List<ItemDataInfo>();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> CreateDefaultItemData(Int64 userId)
    {
        try
        {
            var defaultItem = MasterDataDb.itemList[1]; // 아이템 : 작은 칼 

            //Console.WriteLine($"defaultItem) attack : {defaultItem.attack}, defence : {defaultItem.defence}");

            var count = await _queryFactory.Query(GameDbTable.player_item).InsertAsync(new
            {
                player_id = userId,
                item_code = defaultItem.code,
                count = 1,
                attack = defaultItem.attack,
                defence = defaultItem.defence,
                magic = defaultItem.magic,
                enhance_count = 0,
            });

            if (count != 1)
            {
                return ErrorCode.CreateItemDataFailInsert;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[PlayerItemDb.CreateDefaultItemData] ErrorCode :{ErrorCode.CreateItemDataFailException}");

            return ErrorCode.CreateItemDataFailException;
        }
    }

    public async Task<ErrorCode> VerifyItemData(Int64 userId)
    {
        try
        {
            var itemDataInfo = await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, userId).FirstOrDefaultAsync<ItemDataInfo>();

            if(itemDataInfo == null || itemDataInfo.player_id == 0)
            {
                return ErrorCode.UserItemInfoNotFound;
            }

            _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadGameData],
                $"code : {itemDataInfo.item_code}, attack :{itemDataInfo.attack}, defence :{itemDataInfo.defence}, magic : {itemDataInfo.magic}, enhance_max_count : {itemDataInfo.enhance_count}, [ LoadItemData Ok ]");

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.VerifyItemData] ErrorCode:{ErrorCode.LoadItemDataFailException}, player_id:{userId}");

            return ErrorCode.LoadItemDataFailException;
        }
    }

    public async Task<ErrorCode> VerifyUserItem(Int64 userId, Int32 itemCode)
    {
        try
        {
            var itemDataInfo = await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, userId).Where(DbColumn.item_code, itemCode).FirstOrDefaultAsync<ItemDataInfo>();

            if (itemDataInfo == null)
            {
                return ErrorCode.UserItemInfoNotFound;
            }

            _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadGameData],
                $"code : {itemCode}, [ LoadItemData Ok ]");

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.VerifyItemData] ErrorCode:{ErrorCode.LoadItemDataFailException}, player_id:{userId}");

            return ErrorCode.LoadItemDataFailException;
        }

    }

    public async Task<IEnumerable<ItemDataInfo>> GetItemCodeData(Int64 userId, Int64 itemcode)
    {
        try
        {
            var gameDataInfo = _queryFactory.Query(GameDbTable.player_item).Select(DbColumn.player_id, DbColumn.item_code, "count", "attack", "defence", "magic", "enhance_count").OrderBy(DbColumn.item_code);
            var itemdata = await gameDataInfo.GetAsync<ItemDataInfo>();

            playerItemList = itemdata.ToList();

            foreach(var item in playerItemList)
            {
                Console.WriteLine($">>itemcode :  {item.item_code} , itemcount :{item.count}, magic : {item.magic}, enhance_count : {item.enhance_count}");
            }

            Console.WriteLine($"playerItemListCount : {playerItemList.Count()}");
            return itemdata;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.LoadItemData] ErrorCode:{ErrorCode.LoadItemDataFailException}, player_id:{userId}");

            return null;
        }
    }

    public async Task<ErrorCode> InsertItemAsync(Int64 userId, ItemDataInfo newitem, Int32 code)
    {
        try
        {
            var enhanceMaxCount = MasterDataDb.itemList[code].enhance_max_count;
            var enhanceCount = newitem.enhance_count;

            if (enhanceCount > enhanceMaxCount)
            {
                _logger.ZLogError($"[GameDb.InsertItemAsync] ErrorCode : {ErrorCode.EnhanceMaxCountOverException}");
                return ErrorCode.EnhanceMaxCountOverException;
            }

            var query = await _queryFactory.Query("player_item").InsertAsync(new
            {
                player_id = userId,
                item_code = newitem.item_code,
                count = newitem.count,
                attack = newitem.attack,
                defence = newitem.defence,
                magic = newitem.magic,
                enhace_count = enhanceCount
            });

            return ErrorCode.None;

        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[GameDb.InsertItemAsync] ErrorCode : {ErrorCode.CreateItemDataFailException}");

            return ErrorCode.CreateItemDataFailException;
        }
    }

    public async Task<ErrorCode> UpdateItemAsync(Int64 userId, Int32 itemcode, Int64 count)
    {
        try
        {
            var attack = MasterDataDb.itemList[itemcode-1].attack;
            var defence = MasterDataDb.itemList[itemcode - 1].defence;
            var magic = MasterDataDb.itemList[itemcode - 1].magic;
            Console.WriteLine($"attack : {attack}");
            Console.WriteLine($"defence : {defence}");

            //Console.WriteLine($"List Count : {playerItemList.Count()}");
            var itemlist = playerItemList.Where(item => item.item_code == itemcode).FirstOrDefault();
            //Console.WriteLine($"itelist itemcode : ** {itemlist.item_code}, count{itemlist.count}");

            var query = await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, userId)
                .Where("item_code", itemcode).
                UpdateAsync(new
                {
                    count = count + itemlist.count,
                    attack = itemlist.attack + attack,
                    defence = itemlist.defence + defence,
                    magic = itemlist.magic + magic
                });
            

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[GameDb.UpdateItemAsync] ErrorCode : {ErrorCode.UpdateItemDataFailException}");

            return ErrorCode.UpdateItemDataFailException;
        }
    }

    public async Task<ErrorCode> DeleteItemData(Int64 userId)
    {
        try
        {
            await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, userId).DeleteAsync();

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.DeleteItemData] ErrorCode : {ErrorCode.DeleteItmeDataFail}");

            return ErrorCode.DeleteItmeDataFail;
        }
    }


    public async Task<ErrorCode> DeleteItemData(Int64 uniqueKey, Int64 uinqueCode)
    {
        try
        {
            await _queryFactory.Query("player_item").Where("uniquekey", uniqueKey).Where("uniqyeCode", uinqueCode).DeleteAsync();

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[GameDb.DeleteItemData] ErrorCode : {ErrorCode.DeleteItmeDataFail}");

            return ErrorCode.DeleteItmeDataFail;
        }
    }

    public async Task<ErrorCode> ItemEnhanceAsync(Int64 UserId, Int32 itemcode, Int32 enhancecount)
    {
        try
        {
            var items = MasterDataDb.itemList[itemcode - 1];
            Console.WriteLine($"item : ?? {items.code}, {items.enhance_max_count}");

            var itemlist = playerItemList.Where(item => item.item_code == itemcode).FirstOrDefault();
            Console.WriteLine($"itemList enhance count : {itemlist.enhance_count} ,itemCode : {itemlist.item_code}");

            if(playerItemList.Count == 0)
            {
                _logger.ZLogError(  $"[UserItemDb.EnhanceAsync] ErrorCode : {ErrorCode.UserItemInfoNotFound}");
                return ErrorCode.UserItemInfoNotFound;
            }
            
            if(enhancecount > items.enhance_max_count)
            {
                return ErrorCode.EnhanceMaxCountOverException;
            }

            if (items.code == 1 || items.code == 6)
            {
                return ErrorCode.UnenchantableItemException;
            }

            Random random = new Random();
            int randomNum = random.Next(1,101);
            if(randomNum < 30)
            {
                var query = await _queryFactory.Query(GameDbTable.player_item).Where(DbColumn.player_id, UserId).Where("code", itemcode).UpdateAsync(new
                {
                    count = enhancecount + itemlist.count,
                    attack = itemlist.attack + itemlist.attack,
                    defence = itemlist.defence + itemlist.defence,
                    magic = itemlist.magic + itemlist.magic,
                    enhance_count = enhancecount
                });

                return ErrorCode.None;
            }
            else
            {
                //await DeleteItemData(UserId, playerItemList.Count());

                _logger.ZLogError($"[PlayerItemDb.EnhanceFailException] ErrorCode : {ErrorCode.EnhanceFailException}");

                return ErrorCode.EnhanceFailException;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[PlayerItemDb.ItemEnhanceAsync] ErrorCode : {ErrorCode.UpdateItemDataFailException}");

            return ErrorCode.UpdateItemDataFailException;
        }
    }

    private void AddItemList(ItemDataInfo itemData) => playerItemList.Add(itemData);
    private List<ItemDataInfo> GetItemList() => playerItemList;

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);

        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    public void Dispose() => Close();
}