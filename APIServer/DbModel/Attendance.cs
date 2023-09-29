using System.Diagnostics.Contracts;

namespace APIServer.DbModel;

public class Attendance
{
    public Int64 player_uid { get; set; }
    public DateTime attendance { get; set; }
}