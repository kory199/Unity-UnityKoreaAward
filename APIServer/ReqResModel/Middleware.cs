using APIServer.ReqResModel;
using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResMondel;

public class MiddlewareResponse
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
    [Required] public string ResultMessage { get; set; } = "";
}