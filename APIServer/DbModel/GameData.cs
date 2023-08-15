namespace APIServer.DbModel;

public class GameData
{
    public Int64 player_uid { get; set; }
    public String id { get; set; } = "";
    public Int32 exp { get; set; }
    public Int32 hp { get; set; }
    public Int32 score { get; set; }
    public Int32 status { get; set; }
}