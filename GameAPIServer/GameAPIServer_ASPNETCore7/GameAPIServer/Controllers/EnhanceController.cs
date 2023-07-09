using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class EnhanceController : ControllerBase
{
    private readonly IUserItemDb _userItemDb;
    private readonly ILogger<EnhanceController> _logger;

    public EnhanceController (ILogger<EnhanceController> logger, IUserItemDb playerItemDb)
    {
        _logger = logger;
        _userItemDb = playerItemDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post (PkEnhanceRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var response = new PkResPonse();

        var checkItem = _userItemDb.GetItemCodeData(userInfo.AccountId, request.ItemCode);
        var error = await _userItemDb.ItemEnhanceAsync(userInfo.AccountId, request.ItemCode, request.EnhanceCount);

        if(error != ErrorCode.None)
        {
            response.Result = error;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.Enhance],
            new { ID = request.Id, ItemCode = request.ItemCode }, $"Hardening Success");

        return response;
    }
}