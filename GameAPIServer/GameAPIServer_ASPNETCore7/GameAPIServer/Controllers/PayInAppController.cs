using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;
namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class PayInAppController : ControllerBase
{
    private readonly IPayInAppDb _payInAppDb;
    private readonly IMailDb _mailDb;
    private readonly ILogger<PayInAppController> _logger;

    public PayInAppController(ILogger<PayInAppController> logger, IPayInAppDb payInAppDb, IMailDb mailDb)
    {
        _logger = logger;
        _payInAppDb = payInAppDb;
        _mailDb = mailDb;
    }

    [HttpPost]
    public async Task<PkResPonse> Post(PkPayInAppRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
    
        var response = new PkResPonse();
        var newReceipet = Receipt.NewReceipt();

        var error = await _payInAppDb.CheckReceiptAsync(userInfo.AccountId, request.ReceipteNo, request.Code);

        if(error != ErrorCode.None)
        {
            response.Result = error;
            return response;
        }

        if(error == ErrorCode.None)
        {
            var (errorcode, newmail) = await _payInAppDb.InsertReceiptDataAsync(userInfo.AccountId, newReceipet.payDate, newReceipet.receiptNo, newReceipet.code);

            for(int i = 0; i < newmail.Count(); i ++)
            {
                var mailerror = await _mailDb.InsertMailAsync(newmail[i], MailType.InApp);
            }
        }

        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.GetInAppPayItem],
         new { ID = request.Id, AuthToken = request.AuthToken }, $"Save InAppPayItem Success");

        return response;
    }
}