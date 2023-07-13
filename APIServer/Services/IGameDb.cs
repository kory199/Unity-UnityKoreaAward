namespace APIServer.Services;

public interface IGameDb
{
    public Task<ResultCode> CreateDefaultGameData(Int64 account_id);

    public Task<ResultCode> VerifyGameData(Int64 account_id);

    // TODO : UpdateGameData 

    public Task<ResultCode> DeleteGameData(Int64 account_id);
}