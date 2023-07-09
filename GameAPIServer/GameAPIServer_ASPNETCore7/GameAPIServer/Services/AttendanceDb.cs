using System.Data;
using GameAPIServer.DBModel;
using GameAPIServer.StateType;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace GameAPIServer.Services;

public class AttendanceDb : IAttendanceDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<AttendanceDb> _logger;

    private IDbConnection _dbConn;
    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public AttendanceDb(ILogger<AttendanceDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> SaveAttendanceReward(Int64 userId, Int32 day)
    {
        try
        {
            var query = await _queryFactory.Query(GameDbTable.attendance).InsertAsync(new
            {
                player_id = userId,
                attendance_day = DateTime.Now,
                continuous_attendance = day
            });

            //Mail sandNewMail = await SandMailAsync(userId, itemCode, count);

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[AttendanceDb.SaveAttendanceReward] ErrorCode :{ErrorCode.SaveAttendanceDataFailException}");

            return ErrorCode.SaveAttendanceDataFailException;
        }
    }

    public async Task<ErrorCode> ChecAndCreateAttendanceRecord(Int64 userId, int day)
    {
        try
        {
            var curtime = DateTime.Now;
            var yesterday = DateTime.Today.AddDays(-1).Day;

            var query = await _queryFactory.Query(GameDbTable.attendance).Where(DbColumn.player_id, userId).FirstOrDefaultAsync<Attendance>();
            var errorcode = ErrorCode.None;

            if (query == null || query.GetPlayerId() == 0)
            {
                var newattendance = await SaveAttendanceReward(userId, 1);
            }

            if(query != null || query.GetAttendanceDay().Day == curtime.Day)
            {
                return ErrorCode.DoubleAttendanceException;
            }

            if(query != null || day == query.GetContinuousAttendance() +1 || query.GetAttendanceDay().Day == yesterday)
            {
                var updateAttendanceDay = await ChangedContinuousDay(userId, day);
            }

            if(query != null || day == query.GetContinuousAttendance() + 1 || query.GetAttendanceDay().Day != yesterday)
            {
                Console.WriteLine($"삭제 ~~~~");
            }

            return ErrorCode.None;
        }
        catch
        {
            _logger.ZLogError(
                 $"[AccountDb.ChecAndCreateAttendanceRecord] ErrorCode : {ErrorCode.CheckAttendanceDayException}, Attendanceday : {0}");

            return ErrorCode.SaveAttendanceDataFailException;
        }
    }

    public async Task<ErrorCode> DeleteAttendanceDataAsync(Int64 userId)
    {
        try
        {
            var query = _queryFactory.Query(GameDbTable.attendance).Where(DbColumn.player_id, userId).Delete();

            await _queryFactory.StatementAsync(query.ToString());

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                 $"[Attendance.DeleteAttendanceDataAsync ] ErrorCode : {ErrorCode.DeleteAttendanceDataFailException}");

            return ErrorCode.DeleteAttendanceDataFailException;
        }
    }

    private async Task<ErrorCode> ChangedContinuousDay(Int64 userId, Int32 attendanDay)
    {
        try
        {
            var query = await _queryFactory.Query(GameDbTable.attendance).Where(DbColumn.player_id, userId).UpdateAsync(new { continuous_attendance = attendanDay });

            if (query == 0)
            {
                return ErrorCode.UpdateContinuousAttendanceFail;
            }

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[MailDb.ChangedExp] ErrorCode : {ErrorCode.UpdateContinuousAttendanceFailException}, Continuous_Attendance : {attendanDay}");

            return ErrorCode.UpdateContinuousAttendanceFailException;
        }
    }

    public async Task<Mail> SandMailAsync(Int64 userId, Int32 itemCode, Int64 count)
    {
        try
        {
            Item item = MasterDataDb.itemList.FirstOrDefault(i => i.code == itemCode);
            
            Mail mail = new Mail
            {
                player_id = userId,
                title = "[이벤트] 출석체크 보상 아이템",
                content = $"안녕하세요, 출석체크 보상으로 받은 아이템 : {item.name} 수량 : {count} 개를 지급합니다. 받기 버튼을 클릭하여 수령하세요 !",
                exp = 30,
                type = MailType.Event,
            };

            Console.WriteLine($"Mail exp : {mail.exp}, title :{mail.title}, content : {mail.content}, type :{mail.type}");

            GetAttendanceItem newlist = new GetAttendanceItem
            {
                Code = item.code,
                Name = item.name,
                Count = count,
            };

            UserItemDb.playerMailItemList.Add(newlist);

            return mail;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, "[AttendanceDb.SandMailAsync] Failed to send Mail");

            return null;
        }
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    public void Disponse() => Close();
}