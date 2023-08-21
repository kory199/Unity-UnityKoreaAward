using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VersionController : ControllerBase
{
    private readonly IMasterDataDb _masterDataDb;

    public VersionController(ILogger<VersionController> logger, IMasterDataDb masterDataDb)
    {
        _masterDataDb = masterDataDb;
    }

    [HttpPost]
    public async Task<VersionRes> Post(VersionReq request)
    {
        var response = new VersionRes();

        if (request.Version != "GetVersion")
        {
            response.Result = ResultCode.GameVersionResqustStringCheck;
            response.ResultMessage = ResultCode.GameVersionResqustStringCheck.ToString();
            response.GameVer = "";
            return response;
        }
        else
        {
            var (resultcode, verStr) = await _masterDataDb.VerifyGameVersionAsync();

            if (verStr == null)
            {
                response.Result = resultcode;
                response.ResultMessage = resultcode.ToString();
                return response;
            }

            response.Result = ResultCode.LoadGameVersionSuccess;
            response.ResultMessage = ResultCode.LoadGameVersionSuccess.ToString();
            response.GameVer = verStr;
        }

        return response;
    }
}