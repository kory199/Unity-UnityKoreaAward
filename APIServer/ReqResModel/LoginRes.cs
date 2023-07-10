using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class LoginRes
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
    [Required] public string AuthToken { get; set; } = "";
}