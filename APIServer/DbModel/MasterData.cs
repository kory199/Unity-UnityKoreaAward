using Microsoft.AspNetCore.Http;

namespace APIServer.DbModel;

public class MasterData
{
    public Int32 game_vresion { get; set; }
}

public class MasterDataDic
{

    public static Dictionary<String, Object> masterDataDic = new Dictionary<String, Object>();
}

public class MasterDataDicKey
{
    public const String MeleeMonstser = "MeleeMonstser";
    public const String RangedMonster = "RangedMonster";
    public const String BOSS = "BOSS";
    public const String PlayerStatus = "PlayerStatus";
    public const String StageSpawnMonster = "StageSpawnMonster";
}