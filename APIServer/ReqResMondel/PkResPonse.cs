using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class PkResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}