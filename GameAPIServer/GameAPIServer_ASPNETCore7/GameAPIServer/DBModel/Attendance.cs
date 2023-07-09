namespace GameAPIServer.DBModel;

//public class Attendance
//{
//    public Int64 player_id { get; set; }
//    public DateTime attendance_day { get; set; }
//    public Int32 continuous_attendance { get; set; }
//}

public class Attendance
{
    private Int64 player_id;
    private DateTime attendance_day;
    private Int32 continuous_attendance;

    public Int64 GetPlayerId() => player_id;
    public Int64 SetPlayerId(Int64 value) => player_id = value;

    public DateTime GetAttendanceDay() => attendance_day;
    public DateTime SetAttendanceDay(DateTime dateTime) => attendance_day = dateTime;

    public Int32 GetContinuousAttendance() => continuous_attendance;
    public Int32 SetContinuousAttendance(Int32 value) => continuous_attendance = value;
}