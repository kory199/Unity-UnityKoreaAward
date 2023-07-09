using GameAPIServer.DBModel;
using GameAPIServer.ReqResModel;
using GameAPIServer.Services;
using GameAPIServer.StateType;
using Microsoft.AspNetCore.Mvc;
using ZLogger;
using static Humanizer.On;

namespace GameAPIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceDb _attendanceDb;
    private readonly IMailDb _mailDb;
    private readonly ILogger<AttendanceController> _logger;

    public AttendanceController (ILogger<AttendanceController> logger, IAttendanceDb attendanceDb, IMailDb mailDb)
    {
        _logger = logger;
        _attendanceDb = attendanceDb;
        _mailDb = mailDb;
    }

    [HttpPost]
    public async Task<PkAttendanceResPonse> Post (PkAttendanceRequest request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;

        var response = new PkAttendanceResPonse();

        var saveAttendanceError = await _attendanceDb.ChecAndCreateAttendanceRecord(userInfo.AccountId, request.Day);


        var index = request.Day - 1;
        var itemCode = MasterDataDb.attendanceRewardList[index].item_code;
        var count = MasterDataDb.attendanceRewardList[index].count;

        if (saveAttendanceError == ErrorCode.None)
        {
            Mail newMail = await _attendanceDb.SandMailAsync(userInfo.AccountId, itemCode, count);
            await _mailDb.InsertMailAsync(newMail, MailType.Event);
        }

        if (saveAttendanceError != ErrorCode.None)
        {
            response.Result = saveAttendanceError;
            return response;
        }
        
        _logger.ZLogInformationWithPayload(LogManager.EventIdDic[EventType.GetAttendance],
                 new { ID = request.Id, AuthToken = request.AuthToken }, $"Save Attendance Reward Success");

        return response;
    }
}