using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class LoginRes
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public string AuthToken { get; set; } = "";
}