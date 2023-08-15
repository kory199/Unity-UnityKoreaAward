using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class GameDataController : BaseApiController
{
    private readonly IGameDb _gameDb;

    public GameDataController(ILogger<GameDataController> logger, IGameDb gameDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _gameDb = gameDb;
    }

    [HttpPost]
    public async Task<GameDataRes> Post(PlayerInfoReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var (resultCode, gameData) = await _gameDb.VerifyGameData(userInfo.AccountId);
    
        if(gameData == null)
        {
            (resultCode, gameData) = await _gameDb.CreateDefaultGameData(userInfo.AccountId, userInfo.Id);

            if(resultCode != ResultCode.None)
            {
                return CreateResponse<GameDataRes>(resultCode);
            }

            if(gameData == null)
            {
                return CreateResponse<GameDataRes>(ResultCode.PlayerGameDataNotFound);
            }
        }

        var response = CreateResponse<GameDataRes>(ResultCode.LoadGameDataSuccess);
        response.PlayerData = gameData;
        return response;
    }
}