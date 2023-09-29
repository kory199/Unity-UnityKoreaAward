namespace APIServer.Services;

public interface IAttendanceDb
{
    public Task<(ResultCode, List<Int32>?)> VerifyAttendanceAsync(Int64 account_id);

    public Task<ResultCode> AttendanceCheckAsync(Int64 account_id, Int32 day);

    public Task<ResultCode> ResetAttendanceAsync(Int64 account_id);
}