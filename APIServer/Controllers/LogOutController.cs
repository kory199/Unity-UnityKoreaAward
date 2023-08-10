using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LogOutController : BaseApiController
{
    private readonly IGameDb _gameDb;

    public LogOutController(ILogger<LogOutController> logger, IGameDb gameDb, IAccountDb accountDb, IMemoryDb memoryDb)
        : base(logger, memoryDb, accountDb)
    {
        _gameDb = gameDb;
    }

    [HttpPost]
    public async Task<PkResponse> Post(PlayerInfoReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var resultCode = await _memoryDb.DelectUserAsync(request.ID);
        resultCode = await _gameDb.StatusChangedAsync(userInfo.AccountId);

        if (resultCode != ResultCode.None)
        {
            return CreateResponse<PkResponse>(resultCode);
        }

        var response = CreateResponse<PkResponse>(ResultCode.LogOutSuccess);
        return response;
    }
}