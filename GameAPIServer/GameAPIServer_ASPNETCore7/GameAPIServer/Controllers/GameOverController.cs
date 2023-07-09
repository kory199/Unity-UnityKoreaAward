using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;


namespace GameAPIServer.Controllers;

[ApiController]
[Route("controller")]
public class GameOverController : ControllerBase
{
    private readonly IDungeonStage _dungeonStage;
    private readonly IMemoryDb _memoryDb;
    private readonly IUserItemDb _userItemDb;
    private readonly ILogger<GameOverController> _logger;

    public GameOverController(ILogger<GameOverController> logger, IDungeonStage dungeonStage, IMemoryDb memoryDb, IUserItemDb userItemDb)
    {
        _logger = logger;
        _dungeonStage = dungeonStage;
        _memoryDb = memoryDb;
        _userItemDb = userItemDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(PkRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new PkResPonse();

        var (errorcode, userstate) = await _memoryDb.GetUserStateAsync(userInfo.Id);

        if (userstate != UserState.Playing.ToString())
        {
            response.Result = errorcode;
            return response;
        }

        var itemlist = _dungeonStage.GetItemList();

        for (int i = 0; i < _dungeonStage.GetItemList().Count(); i ++)
        {
            var itemcode = itemlist[i].code;
            var count = 1;

            errorcode = await _userItemDb.UpdateItemAsync(userInfo.AccountId, itemcode, count);
        }

        if (errorcode != ErrorCode.None)
        {
            response.Result = errorcode;
            return response;
        }

        return response;
    }
}