namespace GameAPIServer.Services;

public interface IVersionDb
{
    public Task<Tuple<ErrorCode, Int32>> VerifyMasterDataVersion();

    public Task<ErrorCode> InsertUserVersion(Int64 user_id, Int32 user_Version);

    public Task<Tuple<ErrorCode, Int32>> VerifyUserVersion(Int64 user_id);

    public Task<ErrorCode> DeleteUserVersion(Int64 user_id);
}