using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class VerifyAttendanceController : BaseApiController
{
    private readonly IAttendanceDb _attendanceDb;

    public VerifyAttendanceController(ILogger<VerifyAttendanceController> logger, IAttendanceDb attendanceDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _attendanceDb = attendanceDb;
    }

    [HttpPost]
    public async Task<AttendedDataRes> Post(PlayerInfoReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var (resultCode, attendanceList) = await _attendanceDb.VerifyAttendanceAsync(userInfo.AccountId);

        if(resultCode != ResultCode.None && attendanceList == null)
        {
            return CreateResponse<AttendedDataRes>(ResultCode.AttendanceDataNodeFound);
        }

        var response = CreateResponse<AttendedDataRes>(ResultCode.VerifyAttendanceSuccess);
        response.AttendedData = attendanceList;
        return response;
    }
}