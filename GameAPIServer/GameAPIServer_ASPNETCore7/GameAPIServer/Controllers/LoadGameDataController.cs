using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using GameAPIServer.DBModel;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadGameDataController : ControllerBase
{
    private readonly IGameDb _gameDb;
    private readonly IUserItemDb _userItemDb;
    private readonly ILogger<LoadGameDataController> _logger;

    public LoadGameDataController(ILogger<LoadGameDataController> logger, IGameDb gameDb, IUserItemDb playerItemDb)
    {
        _logger = logger;
        _gameDb = gameDb;
        _userItemDb = playerItemDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(UserInfoRequset request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var response = new PkResPonse();

        var errorCode= await CreateUserGmaeDB(userInfo.AccountId);

        if(errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadGameData],
            new { UniqueKey = userInfo.AccountId },$"LoadGameData Success");

        return response;
    }

    private async Task<ErrorCode> CreateUserGmaeDB(Int64 userId)
    {
        var errorCode = ErrorCode.None;

        try
        {
            errorCode = await _gameDb.VerifyGameData(userId);
            var itemerrorCode = await _userItemDb.VerifyItemData(userId);

            if (errorCode != ErrorCode.None)
            {
                errorCode = await _gameDb.CreateDefaultGameData(userId);
            }
            if (itemerrorCode != ErrorCode.None)
            {
                itemerrorCode = await _userItemDb.CreateDefaultItemData(userId);
            }
        }
        catch (Exception e)
        {
            DeleteGameData(userId);

            _logger.ZLogError(e,
                $"[GameData and ItemData] ErrorCode :{ErrorCode.CreateUserGameDB}, UserID :{userId}");
        }

        return errorCode;
    }

    private async void DeleteGameData(Int64 user_id)
    {
        await _gameDb.DeleteGameData(user_id);

        if (user_id != 0)
        {
            await _userItemDb.DeleteItemData(user_id);
        }
    }
}