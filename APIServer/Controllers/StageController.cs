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
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var resultCode = ResultCode.None;
        var stageList = new List<Stage>();

        if (request.StageNum != 0 && NextStageDb.StageInfoDic.ContainsKey(request.StageNum) == false)
        {
            return CreateResponse<StageDataRes>(ResultCode.StageNumNotMatch);
        }

        if (request.StageNum == 0)
        {
            (resultCode, stageList) = await _stageDb.VerifyStageAsync(userInfo.AccountId);

            if (stageList.Count() == 0)
            {
                (resultCode, var defaultStage) = await _stageDb.CreateDefaultStageData(userInfo.AccountId);
                stageList.Add(defaultStage);

                if (defaultStage == null || resultCode != ResultCode.None)
                {
                    return CreateResponse<StageDataRes>(resultCode);
                }
            }

            resultCode = ResultCode.LoadStageSuccess;
        }
        else 
        {

            if (!stageList.Any(s => s.stage_id == request.StageNum))
            {
                resultCode = await _stageDb.UpdataStageAsync(userInfo.AccountId, request.StageNum);

                if (resultCode != ResultCode.None)
                {
                    return CreateResponse<StageDataRes>(resultCode);
                }
            }

            resultCode = ResultCode.GetNewStageSuccess;
        }

        var response = CreateResponse<StageDataRes>(resultCode);
        response.StageData = stageList;
        if(response.StageData.Count() == 0)
        {
            response.StageData = null;
        }

        return response;
    }
}