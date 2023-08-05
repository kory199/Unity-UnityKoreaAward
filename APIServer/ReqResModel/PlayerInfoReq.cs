using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public class PlayerInfoReq
{
    [Required] public String ID { get; set; } = "";
    [Required] public String AuthToken { get; set; } = "";
}

public class ScoreUpdateReq : PlayerInfoReq
{
    [Required] public Int32 Score { get; set; } 
}

public class StageReq : PlayerInfoReq
{
    [Required] public Int32 StageNum { get; set; }
}

public class PingReq : PlayerInfoReq
{
    [Required] public String ClientPing { get; set; } = "";
}