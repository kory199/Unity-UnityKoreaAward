using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class MailContentController : ControllerBase
{
    private readonly IMailDb _mailDb;
    private readonly IMemoryDb _memoryDb;
    private readonly IUserItemDb _userItemDb;
    private readonly IPayInAppDb _payInAppDb;
    private readonly ILogger<MailContentController> _logger;

    public MailContentController (ILogger<MailContentController> logger, IMailDb mailDb, IMemoryDb memoryDb,
        IUserItemDb playerItemDb, IPayInAppDb payInAppDb)
    {
        _logger = logger;
        _memoryDb = memoryDb;
        _mailDb = mailDb;
        _userItemDb = playerItemDb;
        _payInAppDb = payInAppDb;
    }

    [HttpPost]
    public async Task<PkLoadMailContentResPonse> Post(PkLoadMailContentRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var response = new PkLoadMailContentResPonse();

        int index = request.PageIndex - 1;

        var (openmail, mailContent) = await _mailDb.ReadMailContent(index);
        var mailStateTypeChaged = await _mailDb.ChangedMailTypeAsync(index);

        var itemcode = _mailDb.GetItemCode(index);
        var count = _mailDb.GetItemCount(index);

        var checkItem = _userItemDb.GetItemCodeData(userInfo.AccountId, itemcode);
        var itemUpdate = _userItemDb.UpdateItemAsync(userInfo.AccountId,itemcode, 2);

        if (mailContent == null || openmail == StateType.MailStateType.NotRead || mailStateTypeChaged != ErrorCode.None)
        {
            response.Result = ErrorCode.OpenMailDataFailException;
            return response;
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.LoadMail],
            new { PageIndex = request.PageIndex }, $"Open Mail");

        var mailList = MailDb.mailList;

        ReadMailContent readMailContents = new ReadMailContent
        {
            title = mailList[request.PageIndex].title,
            content = mailContent,

        };
        
        response.MailContentList = readMailContents;

        return response;
    }
}