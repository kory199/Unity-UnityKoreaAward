using APIServer.DbModel;
using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public abstract class BaseResponse
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
    [Required] public string ResultMessage { get; set; } = string.Empty;
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