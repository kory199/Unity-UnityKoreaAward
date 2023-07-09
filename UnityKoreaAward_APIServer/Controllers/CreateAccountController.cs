using Microsoft.AspNetCore.Mvc;
using UnityKoreaAward_APIServer.ReqResMondel;
using UnityKoreaAward_APIServer.Services;
using ZLogger;

namespace UnityKoreaAward_APIServer.Controllers;

[ApiController]
[Route("api/[controller]")]
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
    public async Task<PkResPonse>Post(PkAccountRequest request)
    {
        var response = new PkResPonse();
        var errorCode = await _accountDb.CreateAccountAsync(request.ID, request.Password);

        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        // TODO: 클래스 만들 예정 ZLogInformationWithPayLoad 변경
        _logger.ZLogDebug($"CreateAccount Success");

        return response;
    }
}