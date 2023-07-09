using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class CreateGameDataController : ControllerBase
{
    private readonly IGameDb _gameDb;
    private readonly IMemoryDb _memoryDb;
    private readonly IUserItemDb _userItemDb;
    private readonly ILogger<CreateGameDataController> _logger;

    public CreateGameDataController(ILogger<CreateGameDataController> logger, IGameDb gameDb, IMemoryDb memoryDb, IUserItemDb playerItemDb)
    {
        _logger = logger;
        _gameDb = gameDb;
        _memoryDb = memoryDb;
        _userItemDb = playerItemDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(UserInfoRequset request)
    {
       var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
       
       var response = new PkResPonse();
       
       (var errorCode, var uniqueKey) = await CreateDB(userInfo.AccountId);
       
       if(errorCode != ErrorCode.None)
       {
           response.Result = errorCode;
       
           return response;
       }

       _logger.ZLogInformationWithPayload(LogManager.EventIdDic[StateType.EventType.CreateGameData],
          new { ID = request.Id , AccountId = uniqueKey }, $"CeateGameData Success");
       
       return response;
    }

    private async Task<(ErrorCode, Int64)> CreateDB(Int64 accountId)
    {
        try
        {
            var gamedataErrorCode = await _gameDb.CreateDefaultGameData(accountId);
            var itemdataErrorCode = await _userItemDb.CreateDefaultItemData(accountId);

            if(gamedataErrorCode != ErrorCode.None )
            {
                return (ErrorCode.CreateGameDataFailException, accountId);

                _logger.ZLogError($"[CreateGameData] ErrorCode :{ErrorCode.CreateGameDataFailException}, player_id :{accountId}");
            }

            if(itemdataErrorCode != ErrorCode.None)
            {
                return (ErrorCode.CreateItemDataFailException, accountId);

                _logger.ZLogError($"[CreateGameData] ErrorCode :{ErrorCode.CreateItemDataFailException}, player_id :{accountId}");
            }
        }
        catch(Exception e)
        {
            DeleteGameData(accountId);
            
            _logger.ZLogError(e,
                $"[CreateGameData] ErrorCode :{ErrorCode.CreateGameDataFailException}, player_id :{accountId}");
        }

        return (ErrorCode.None, accountId);
    }

    private async void DeleteGameData(Int64 uniqueKey)
    {
        await _gameDb.DeleteGameData(uniqueKey);

        if(uniqueKey != 0)
        {
            await _userItemDb.DeleteItemData(uniqueKey);
        }
    }
}