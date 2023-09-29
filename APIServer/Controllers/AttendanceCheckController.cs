using APIServer.DbModel;
using APIServer.ReqResModel;
using APIServer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AttendanceCheckController : BaseApiController
{
    private readonly IAttendanceDb _attendanceDb;

    public AttendanceCheckController(ILogger<AttendanceCheckController> logger, IAttendanceDb attendanceDb, IMemoryDb memoryDb, IAccountDb accountDb)
        : base(logger, memoryDb, accountDb)
    {
        _attendanceDb = attendanceDb;
    }

    [HttpPost]
    public async Task<PkResponse> Post(AttendanceCheckReq request)
    {
        var userInfo = (AuthUser)HttpContext.Items[nameof(AuthUser)]!;
        var resultCode = await _attendanceDb.AttendanceCheckAsync(userInfo.AccountId, request.Day);

        if(resultCode != ResultCode.None)
        {
            return CreateResponse<PkResponse>(ResultCode.AttendanceDataNodeFound);
        }

        var response = CreateResponse<PkResponse>(ResultCode.AttendanceCheckSuccess);
        return response;
    }
}