using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public class VersionReq
{
    [Required] public String Version { get; set; } = "GetVersion";
}