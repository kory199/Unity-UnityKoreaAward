using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using ZLogger;

namespace APIServer.Services;

public class AttendanceDb : BaseDb<AttendanceDb>, IAttendanceDb
{
    public AttendanceDb(ILogger<AttendanceDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.GameDb, AttendanceTable.player_attendance)
    {
    }

    public async Task<(ResultCode, List<Int32>?)> VerifyAttendanceAsync(Int64 account_id)
    {
        try
        {
            var attendanceInfo = await _queryFactory.Query(_tableName)
              .Where(GameDbTable.player_uid, account_id)
              .GetAsync<Attendance>();

            List<Int32> attendanceList = new List<Int32>();

            foreach (var attendance in attendanceInfo)
            {
                attendanceList.Add(attendance.attendance.Day);
            }

            return (ResultCode.None, attendanceList);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.VerifyAttendanceAsync] " +
               $"ResultCode : {ResultCode.VerifyAttendanceFailException}");

            return (ResultCode.VerifyAttendanceFailException, new List<int>());
        }
    }

    public async Task<ResultCode> AttendanceCheckAsync(Int64 account_id, Int32 day)
    {
        try
        {
            DateTime today = DateTime.Today;
            DateTime firstDayOfMonth = new DateTime(today.Year, today.Month, 1);
            DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            //if(day == firstDayOfMonth.Day)
            //{
            //    await ResetAttendanceAsync(account_id);
            //}

            if (day == DateTime.Now.Day)
            {
                var attendanceCheck = new Attendance
                {
                    player_uid = account_id,
                    attendance = DateTime.Now,
                };

                var count = await _queryFactory.Query(_tableName)
                        .InsertAsync(attendanceCheck);

                return ResultCode.None;
            }
            else
            {
                return ResultCode.AttendanceCheckFail;
            }
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.AttendanceCheckAsync] " +
                $"ResultCode : {ResultCode.AttendanceCheckFailException}");

            return ResultCode.AttendanceCheckFailException;
        }
    }

    public async Task<ResultCode> ResetAttendanceAsync(Int64 account_id)
    {
        try
        {
            await ExecuteDeleteAsync(GameDbTable.player_uid, account_id);
            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.ResetAttendanceAsync] " +
               $"ResultCode : {ResultCode.AttendanceResetFailException}");

            return ResultCode.AttendanceResetFailException;
        }
    }
}