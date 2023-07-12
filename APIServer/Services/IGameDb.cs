namespace APIServer.Services;

public interface IGameDb
{
    public Task<ResultCode> CreateDefaultGameData(Int64 player_id);

    public Task<ResultCode> VerifyGameData(Int64 player_id);

    // TODO : UpdateGameData 

    public Task<ResultCode> DeleteGameData(Int64 player_id);
}