using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class LoadMailController : ControllerBase
{
    private readonly IMailDb _mailDb;
    private readonly ILogger<LoadMailController> _logger;

    public LoadMailController(ILogger<LoadMailController> logger, IMailDb mailDb)
    {
        _logger = logger;
        _mailDb = mailDb;
    }

    [HttpPost]
    public async Task<PkLoadMailResPonse> Post(PkLoadMailRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new PkLoadMailResPonse();

        var loadMailError = await _mailDb.LoadMailAsync(userInfo.AccountId, request.PageNum);

        if(loadMailError != ErrorCode.None)
        {
            response.Result = loadMailError;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadMail],
            new { ID = request.Id, AuthToken = request.AuthToken }, $"Load Mail");

        var mailList = _mailDb.GetMailList();

        List<LoadMail> loadMail = mailList.Select(m => new LoadMail
        {
            title = m.title,
            exp = m.exp,
        }).ToList();

        response.mailList = loadMail;
        return response;
    }
}