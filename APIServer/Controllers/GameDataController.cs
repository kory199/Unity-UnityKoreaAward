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

    public GameDataController(ILogger<BaseApiController> logger, IGameDb gameDb, IMemoryDb memoryDb)
        : base(logger, memoryDb)
    {
        _gameDb = gameDb;
    }

    [HttpPost]
    public async Task<GameDataRes> Post(PlayerInfoReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new GameDataRes();

        var (resultCode, gameData) = await _gameDb.VerifyGameData(userInfo.AccountId);
    
        if(gameData == null)
        {
            gameData = new GameData(userInfo.AccountId);

            (resultCode, gameData) = await _gameDb.CreateDefaultGameData(userInfo.AccountId);

            if(resultCode != ResultCode.None)
            {
                response.Result = resultCode;
                return response;
            }
        }

        response.PlayerData.Add(gameData);
        return response;
    }
}