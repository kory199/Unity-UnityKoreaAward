using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("controller")]
public class GameCancelController : ControllerBase
{
    private readonly IDungeonStage _dungeonStage;
    private readonly IMemoryDb _memoryDb;
    private readonly ILogger<GameCancelController> _logger;

    public GameCancelController(ILogger<GameCancelController> logger, IDungeonStage dungeonStage, IMemoryDb memoryDb)
    {
        _logger = logger;
        _dungeonStage = dungeonStage;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(PkRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new PkResPonse();

        var errorCode = await _memoryDb.DelectUserAsync(userInfo.Id);

        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.GameChancel],
             new { ID = request.Id }, $"GameChancel Success");

        return response;
    }
}