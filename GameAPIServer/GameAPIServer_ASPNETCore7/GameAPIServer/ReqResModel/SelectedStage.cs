using System;
using GameAPIServer.DBModel;
using System.ComponentModel.DataAnnotations;

namespace GameAPIServer.ReqResModel;

public class PkSelectedStagedRequest
{
    [Required] public String Id { get; set; }
    [Required] public String AuthToken { get; set; }
    [Required] public Int64 StageId { get; set; }
}

public class PkSelectedStagedResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public List<StageItemInfo> StageList { get; set; } = new List<StageItemInfo>();
    [Required] public List<StageAttackNpc> StageNPCList { get; set; } = new List<StageAttackNpc>();
}