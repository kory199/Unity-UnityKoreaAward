using APIServer.ReqResMondel;
using APIServer.Services;
using APIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : BaseApiController
{
    private readonly IAccountDb _accountDb;

    public LoginController(ILogger<BaseApiController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
        : base (logger, memoryDb)
    {
        _accountDb = accountDb;
    }

    [HttpPost]
    public async Task<LoginRes> Post(AccountReq request)
    {
        var response = new LoginRes();
        var (errorCode, account_id) = await _accountDb.VerifyAccountAsync(request.ID, request.Password);
        var authToken = Security.CreateAuthToken();

        errorCode = await _memoryDb.RegistUserAsync(request.ID, authToken, account_id);

        if (errorCode != ResultCode.None)
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