using APIServer.ReqResMondel;
using APIServer.Services;
using APIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAccountDb _accountDb;
    private readonly IMemoryDb _memoryDb;
    private ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
    {
        _logger = logger;
        _accountDb = accountDb;
        _memoryDb = memoryDb;
    }

    [HttpPost]
    public async Task<LoginRes> Post(AccountReq request)
    {
        var response = new LoginRes();
        var errorCode = await _accountDb.VerifyAccountAsync(request.ID, request.Password);
        var authToken = Security.CreateAuthToken();

        errorCode = await _memoryDb.RegistUserAsync(request.ID, authToken);

        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.Login],
            new { ID = request.ID, AuthToken = authToken }, "Login Success");

        response.AuthToken = authToken;
        return response;
    }
}