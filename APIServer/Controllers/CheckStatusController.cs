using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using APIServer.StateType;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CheckStatusController : BaseApiController
{
    private readonly IGameDb _gameDb;

    public CheckStatusController(ILogger<CheckStatusController> logger, IGameDb gameDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _gameDb = gameDb;
    }

    [HttpPost]
    public async Task<CheckStatusRes> Post(CheckStatus request)
    {
        var(resultCode, statusNum) = await _gameDb.CheckStatusAsync(request.AccountId);

        if(resultCode != ResultCode.None || statusNum == (int)UserState.LogOut)
        {
            return CreateResponse<CheckStatusRes>(ResultCode.UserLogOut);
        }

        var response = CreateResponse<CheckStatusRes>(ResultCode.CheckStatusSuccess);
        return response;
    }
}