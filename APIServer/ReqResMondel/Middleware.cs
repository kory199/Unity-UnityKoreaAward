using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class MiddlewareResponse
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
}