using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoginController : ControllerBase
{
    private readonly IAccountDb _accountDb;
    private readonly IMemoryDb _memoryDb;
    private readonly IVersionDb _versionDb;
    private readonly IVerDb _verDb;
    private readonly IGameDb _gameDb;
    private readonly IUserItemDb _userItemDb;
    private readonly ILogger<LoginController> _logger;

    public LoginController(ILogger<LoginController> logger, IAccountDb accountDb, IMemoryDb memoryDb,
        IVersionDb versionDb, IGameDb gameDb, IUserItemDb playerItemDb, IVerDb verDb)
    {
        _logger = logger;
        _accountDb = accountDb;
        _memoryDb = memoryDb;
        _versionDb = versionDb;
        _gameDb = gameDb;
        _userItemDb = playerItemDb;
        _verDb = verDb;
    }

    [HttpPost]
    public async Task<PkLoginResponse> Post(PkLoginRequest request)
    {
        var response = new PkLoginResponse();

        var (errorCode, accountId) = await _accountDb.VerifyAccount(request.Id, request.Password);

        errorCode = await CreateUserGmaeDB(accountId);

        var authToken = Security.CreateAuthToken();
        response.Notice = Notice.LoadNotice();

        //var gameVer = _verDb.GetGameVersion();
        // **** version null 걸러야 한다
        errorCode = await _memoryDb.RegistUserAsync(request.Id, authToken, accountId, _verDb.GetGameVersion());

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[StateType.EventType.Login],
            new { ID = request.Id, AuthToken = authToken, UniqueKey = accountId }, "Login Success");

        if (errorCode != ErrorCode.None)
        {
            response.Result = errorCode;
            return response;
        }

        response.AuthToken = authToken;
        return response;
    }

    private async Task<ErrorCode> CreateUserGmaeDB(Int64 userId)
    {
        var errorCode = ErrorCode.None;

        try
        {
            errorCode = await _gameDb.VerifyGameData(userId);
            var itemerrorCode = await _userItemDb.VerifyItemData(userId);

            if(errorCode != ErrorCode.None)
            {
                errorCode = await _gameDb.CreateDefaultGameData(userId);
            }
            if(itemerrorCode != ErrorCode.None)
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