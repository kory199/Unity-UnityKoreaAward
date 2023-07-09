using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkVersionRequset
{
    [Required] public String Version { get; set; } = "Get Version";
}

public class PkVersionResponse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public String MasterDataVer { get; set; } = "";
    [Required] public String GameVer { get; set; } = "";
}