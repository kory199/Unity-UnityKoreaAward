using System;
using System.ComponentModel.DataAnnotations;
using GameAPIServer.DBModel;

namespace GameAPIServer.ReqResModel;

public class PkDungeonStageResPonse
{
    [Required] public ErrorCode Result { get; set; } = ErrorCode.None;
    [Required] public List<DungeonStage> mailList { get; set; } = new List<DungeonStage>();
}