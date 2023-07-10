using APIServer.ReqResMondel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace APIServer.Controllers;

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
    public async Task<PkResPonse> Post(AccountReq request)
    {
        var response = new PkResPonse();
        var errorCode = await _accountDb.CreateAccountAsync(request.ID, request.Password);

        if (errorCode != ResultCode.None)
        {
            response.Result = errorCode;
            response.Message = errorCode.ToString();
            return response;
        }

        // TODO: 클래스 만들 예정 ZLogInformationWithPayLoad 변경
        _logger.ZLogDebug($"CreateAccount Success");
        response.Message = ResultCode.CreateAccountSuccess.ToString();

        return response;
    }
}