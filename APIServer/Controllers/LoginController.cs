using APIServer.ReqResModel;
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
    public LoginController(ILogger<BaseApiController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
        : base(logger, memoryDb, accountDb)
    {
    }

    [HttpPost]
    public async Task<LoginRes> Post(AccountReq request)
    {
        var (resultCode, account_id) = await _accountDb.VerifyAccountAsync(request.ID, request.Password);

        if (resultCode != ResultCode.None || account_id == 0)
        {
            return CreateResponse<LoginRes>(resultCode);
        }
        else
        {
            var authToken = Security.CreateAuthToken();

            resultCode = await _memoryDb.RegistUserAsync(request.ID, authToken, account_id);

            //_logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.Login],
                    //new { ID = request.ID, AuthToken = authToken }, "Login Success");

            var response = CreateResponse<LoginRes>(ResultCode.LoginSuccess);
            response.AuthToken = authToken;
            response.AccountId = account_id;

            return response;
        }
    }
}