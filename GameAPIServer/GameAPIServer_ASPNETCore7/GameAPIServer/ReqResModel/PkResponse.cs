using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
}