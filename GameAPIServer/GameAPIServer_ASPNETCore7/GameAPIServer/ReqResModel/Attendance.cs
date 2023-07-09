using System;
using System.ComponentModel.DataAnnotations;
using GameAPIServer.DBModel;

namespace GameAPIServer.ReqResModel;

public class PkAttendanceRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int32 Day { get; set; }
}

public class PkAttendanceResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public Int32 Code { get; set; } = 0;
    [Required] public Int32 Count { get; set; } = 0;
}