using System;
namespace GameAPIServer.DBModel;

public class DungeonStage
{
    public Int64 player_id { get; set; }
    public Int64 stage_id { get; set; }
}

public class StageItemInfo
{
    public Int32 item_code { get; set; }
    public Int32 count { get; set; }
}