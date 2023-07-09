namespace GameAPIServer.DBModel;

public class MasterData_Version
{
    public Int32 master_version { get; set; }
}

public class User_Version
{
    public Int64 player_id { get; set; }
    public Int32 version { get; set; }
}

public class MasterDataVer
{
    private Int32 masterdata_ver;
    private DateTime created_at;

    public Int32 GetMasterdata_ver() => masterdata_ver;
    public DateTime GetCreatedAt() => created_at;
}

public class GameDataVer
{
    private Int32 game_ver;
    private DateTime created_at;

    public Int32 GetGameVer() => game_ver;
    public DateTime GetCreatedAt() => created_at;
}