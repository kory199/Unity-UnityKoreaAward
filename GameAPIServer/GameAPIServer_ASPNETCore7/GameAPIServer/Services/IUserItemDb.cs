using GameAPIServer.DBModel;

namespace GameAPIServer.Services;

public interface IUserItemDb
{
    public Task<ErrorCode> CreateDefaultItemData(Int64 userId);

    public Task<ErrorCode> VerifyItemData(Int64 userId);

    public Task<ErrorCode> VerifyUserItem(Int64 userId, Int32 itemCode);

    public Task<IEnumerable<ItemDataInfo>> GetItemCodeData(Int64 userId, Int64 itemcode);

    public Task<ErrorCode> DeleteItemData(Int64 userId);

    public Task<ErrorCode> DeleteItemData(Int64 userId, Int64 uinqueCode);

    public Task<ErrorCode> UpdateItemAsync(Int64 userId, Int32 itemcode, Int64 itemCount);

    public Task<ErrorCode> ItemEnhanceAsync(Int64 userId, Int32 itemcode, Int32 enhancecount);
}