using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class MiddlewareResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}