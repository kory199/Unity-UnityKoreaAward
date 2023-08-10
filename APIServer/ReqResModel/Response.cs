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

public class VersionRes : BaseResponse
{
    [Required] public String GameVer { get; set; } = "";
}

public class LoginRes : BaseResponse
{
    [Required] public Int64 AccountId { get; set; }
    [Required] public String AuthToken { get; set; } = "";
}

public class CheckStatusRes : BaseResponse
{
    [Required] public String ID { get; set; } = "";
    [Required] public String AuthToken { get; set; } = "";
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

public class MasterDataRes : BaseResponse
{
    [Required] public Dictionary<string, object> MasterDataDic { get; set; } = new Dictionary<string, object>();
}