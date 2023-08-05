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

    public StageClearController(ILogger<StageClearController> logger, IGameDb gameDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _gameDb = gameDb;
    }

    [HttpPost]
    public async Task<PkResponse> Post(ScoreUpdateReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var (resultCode, gameData) = await _gameDb.UpdataScoreDataAsync(userInfo.AccountId, request.Score);

        if (gameData == null || resultCode != ResultCode.None)
        {
            return CreateResponse<PkResponse>(resultCode);
        }

        var response = CreateResponse<PkResponse>(ResultCode.UpdateScoreSuccess);
        return response;
    }
}