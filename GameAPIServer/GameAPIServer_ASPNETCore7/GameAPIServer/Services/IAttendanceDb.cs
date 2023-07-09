using GameAPIServer.DBModel;

namespace GameAPIServer.Services;

public interface IAttendanceDb
{
    public Task<ErrorCode> SaveAttendanceReward(Int64 uniquekey, int day);

    public Task<ErrorCode> ChecAndCreateAttendanceRecord(Int64 uniquekey, int day);

    public Task<Mail> SandMailAsync(Int64 userId, Int32 itemCode, Int64 count);

    public Task<ErrorCode> DeleteAttendanceDataAsync(Int64 userId);
}