using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class FarmingController : ControllerBase
{
    private readonly IDungeonStage _dungeonStage;
    private readonly IMemoryDb _memoryDb;
    private readonly ILogger<FarmingController> _logger;

    public FarmingController(ILogger<FarmingController> logger, IDungeonStage dungeonStage, IMemoryDb memoryDb)
    {
        _logger = logger;
        _dungeonStage = dungeonStage;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(PkFarmingItemRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new PkResPonse();

        var (errorcode, userstate) = await _memoryDb.GetUserStateAsync(userInfo.Id);

        if(userstate != UserState.Playing.ToString())
        {
            response.Result = errorcode;
            return response;
        }

        var itemList = _dungeonStage.GetItemList();
        bool itemcode = itemList.Exists(code => code.code == request.ItemCode);

        if (itemcode)
        {
            errorcode = await _memoryDb.RegistUserItemAsync(userInfo.Id);
        }

        if(errorcode != ErrorCode.None)
        {
            response.Result = errorcode;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.FarmingItem],
             new { ID = request.Id }, $"Farming Item Check Success");

        return response;
    }
}