using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class PkResPonse
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
    public string Message { get; set; } = string.Empty;
}