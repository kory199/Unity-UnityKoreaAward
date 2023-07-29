using APIServer.DbModel;
using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public abstract class BaseResponse
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
    [Required] public string ResultMessage { get; set; } = "";
}

public class PkResponse : BaseResponse
{
}


public class LoginRes : BaseResponse
{
    [Required] public string AuthToken { get; set; } = "";
}

public class GameDataRes : BaseResponse
{
    [Required] public List<GameData> PlayerData { get; set; } = new List<GameData>();
}

public class RankingDataRes : BaseResponse
{
    [Required] public List<Ranking> RankingData { get; set; } = new List<Ranking>();
}

public class StageDataRes : BaseResponse
{
    [Required] public List<Stage> StageData { get; set; } = new List<Stage>();
}