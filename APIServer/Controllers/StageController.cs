using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class StageController : BaseApiController
{
    private readonly IStageDb _stageDb;

    public StageController(ILogger<StageController> logger, IStageDb stageDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _stageDb = stageDb;
    }

    [HttpPost]
    public async Task<StageDataRes> Post(StageReq request)
    {
        var uerInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var (resultCode, stageList) = await _stageDb.VerifyStageAsync(uerInfo.AccountId);
        var response = CreateResponse<StageDataRes>(resultCode);

        if (stageList.Count() == 0)
        {
            (resultCode, var defaultStage) = await _stageDb.CreateDefaultStageData(uerInfo.AccountId);
            response.StageData.Add(defaultStage);

            if (defaultStage == null || resultCode != ResultCode.None)
            {
                return CreateResponse<StageDataRes>(resultCode);
            }
        }

        //if(request.StageNum != NextStageDb.StageInfoDic[request.StageNum])
        //{
        //    return CreateResponse<StageDataRes>(ResultCode.StageNumNotMatch);
        //}

        response.StageData = stageList;
        return CreateResponse<StageDataRes>(ResultCode.LoadStageSuccess);
        return response;
    }
}