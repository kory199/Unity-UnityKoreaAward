using APIServer.DbModel;
using System.ComponentModel.DataAnnotations;

namespace APIServer.ReqResModel;

public class GameDataRes
{
    [Required] public ResultCode Result { get; set; } = ResultCode.None;
    [Required] public List<GameData> PlayerData { get; set; } = new List<GameData>();
}