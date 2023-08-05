using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public class VersionReq
{
    [Required] public String Version { get; set; } = "GetVersion";
}

public class MasterDataReq
{
    [Required] public String MasterData { get; set; } = "GetMasterData";
}