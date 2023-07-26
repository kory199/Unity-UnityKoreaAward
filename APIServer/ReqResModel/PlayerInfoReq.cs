using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public class PlayerInfoReq
{
    [Required] public String ID { get; set; } = "";
    [Required] public String AuthToken { get; set; } = "";
}

public class ScoreUpdateReq : PlayerInfoReq
{
    [Required] public int Score { get; set; } 
}