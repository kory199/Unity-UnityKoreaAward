using Microsoft.AspNetCore.Mvc;
using UnityKoreaAward_APIServer.ReqResMondel;
using UnityKoreaAward_APIServer.Services;

namespace UnityKoreaAward_APIServer.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAccountDb _accountDb;
    private ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger, IAccountDb accountDb)
    {
        _logger = logger;
        _accountDb = accountDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(PkAccountRequest request)
    {
        var response = new PkResPonse();

        var errorCode = await _accountDb.VerifyAccountAsync(request.ID, request.Password);

        if(errorCode != ErrorCode.None) 
        {
            response.Result = errorCode;
            return response;
        }

        return response;
    }
}