using APIServer.ReqResModel;
using APIServer.ReqResMondel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateAccountController : BaseApiController
{
    public CreateAccountController(ILogger<CreateAccountController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
        :base(logger, memoryDb, accountDb)
    {
    }

    [HttpPost]
    public async Task<PkResponse> Post(AccountReq request)
    {
        var resultCode = await _accountDb.CreateAccountAsync(request.ID, request.Password);

        if (resultCode != ResultCode.None)
        {
            return CreateResponse<PkResponse>(resultCode);
        }

        // TODO: 클래스 만들 예정 ZLogInformationWithPayLoad 변경
        _logger.ZLogDebug($"CreateAccount Success");

        var response = CreateResponse<PkResponse>(ResultCode.CreateAccountSuccess);
        return response;
    }
}