using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccountController : ControllerBase
{
    private readonly IAccountDb _accountDb;
    private readonly ILogger<CreateAccountController> _logger;

    public CreateAccountController(ILogger<CreateAccountController> logger, IAccountDb accountDb)
    {
        _logger = logger;
        _accountDb = accountDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(PkCreateAccountRequest request)
    {
        var response = new PkResPonse();

        var errorCode = await _accountDb.CreateAccountAsync(request.Id, request.Password);

        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.CreateAccount], new { ID = request.Id }, $"CreateAccount Success");

        return response;
    }
}