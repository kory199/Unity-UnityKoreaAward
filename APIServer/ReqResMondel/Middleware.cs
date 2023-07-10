using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class MiddlewareResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}