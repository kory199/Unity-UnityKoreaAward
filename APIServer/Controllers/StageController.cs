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
    public async Task<StageDataRes> Post(PlayerInfoReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        ResultCode resultCode;
        Int32? stageNum;

        (resultCode, stageNum) = await _stageDb.VerifyStageAsync(userInfo.AccountId);

        if (!stageNum.HasValue || stageNum == 0)
        {
            (resultCode, var defaultStage) = await _stageDb.CreateDefaultStageData(userInfo.AccountId);

            if (defaultStage == null || resultCode != ResultCode.None)
            {
                return CreateResponse<StageDataRes>(resultCode);
            }

            stageNum = defaultStage.stage_id;
        }

        resultCode = ResultCode.LoadStageSuccess;

        var response = CreateResponse<StageDataRes>(resultCode);
        response.StageNum = stageNum ?? 0;
        return response;
    }
}