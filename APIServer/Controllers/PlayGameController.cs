using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.ReqResMondel;
using APIServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlayGameController : BaseApiController
    {
        public PlayGameController(ILogger<PlayGameController> logger, IAccountDb accountDb, IMemoryDb memoryDb)
        : base(logger, memoryDb, accountDb)
        {
        }

        [HttpPost]
        public async Task<PkResponse> Post(PlayerInfoReq request)
        {
            var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
            var resultCode = await _memoryDb.UpdateUserStateAsync(request.ID);

            if (resultCode != ResultCode.None)
            {
                return CreateResponse<PkResponse>(resultCode);
            }

            var response = CreateResponse<PkResponse>(ResultCode.RedisUpdateStatusSuccess);
            return response;
        }
    }
}