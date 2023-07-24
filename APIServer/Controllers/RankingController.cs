using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class RankingController : BaseApiController
{
    private readonly IGameDb _gameDb;

    public RankingController(ILogger<RankingController> logger, IGameDb gameDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _gameDb = gameDb;
    }

    [HttpPost]
    public async Task<RankingDataRes> Post(PlayerInfoReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var (resultCode, rankingData) = await _gameDb.LoadRankingDataAsync(userInfo.AccountId, userInfo.Id);

        if (resultCode != ResultCode.None)
        {
            return CreateResponse<RankingDataRes>(resultCode);
        }

        var response = CreateResponse<RankingDataRes>(ResultCode.LoadRankingDataSuccess);
        
        if(rankingData != null)
        {
            response.RankingData.AddRange(rankingData);
        }

        return response;
    }
}