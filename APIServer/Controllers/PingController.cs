using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class PingController : BaseApiController
{
    public PingController(ILogger<PingController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
    : base(logger, memoryDb, accountDb)
    {
    }

    [HttpPost]
    public async Task<PkResponse> Post(PingReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var result = await _memoryDb.PingAsync(userInfo.Id);

        if(result!= ResultCode.None)
        {
            return CreateResponse<PkResponse>(result);
        }

        var response = CreateResponse<PkResponse>(ResultCode.PingSuccess);
        return response;
    }
}