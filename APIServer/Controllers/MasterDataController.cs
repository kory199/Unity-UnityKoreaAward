using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MasterDataController : ControllerBase
{
    private readonly IMasterDataDb _masterDataDb;

    public MasterDataController(ILogger<MasterDataController> logger, IMasterDataDb masterDataDb)
    {
        _masterDataDb = masterDataDb;
    }

    [HttpPost]
    public async Task<MasterDataRes> Post(MasterDataReq request)
    {
        MasterDataRes response = new MasterDataRes();

        response.Result = ResultCode.LoadMasterDataSuccess;
        response.ResultMessage = ResultCode.LoadMasterDataSuccess.ToString();
        response.MasterDataDic = MasterDataDic.masterDataDic;
        return response;
    }
}