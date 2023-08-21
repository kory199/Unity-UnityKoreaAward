using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class StageClearController : BaseApiController
{
    private readonly IGameDb _gameDb;
    private readonly IStageDb _stageDb;

    public StageClearController(ILogger<StageClearController> logger, IStageDb stageDb, IGameDb gameDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _gameDb = gameDb;
        _stageDb = stageDb;
    }

    [HttpPost]
    public async Task<PkResponse> Post(ScoreUpdateReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var (resultCode, gameData) = await _gameDb.UpdataScoreDataAsync(userInfo.AccountId, request.Score);

        resultCode = await _stageDb.UpdataStageAsync(userInfo.AccountId, request.StageNum);

        if (gameData == null || resultCode != ResultCode.None)
        {
            return CreateResponse<PkResponse>(resultCode);
        }

        var response = CreateResponse<PkResponse>(ResultCode.UpdateScoreSuccess);
        return response;
    }
}