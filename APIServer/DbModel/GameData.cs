namespace APIServer.DbModel;

public class GameData
{
    public Int64 player_uid { get; set; }
    public Int32 exp { get; set; }
    public Int32 hp { get; set; }
    public Int32 score { get; set; }
    public Int32 level { get; set; }
    public Int32 status { get; set; }

    public GameData()
    {
        exp = 1;
        hp = 10;
        score = 0;
        level = 0;
        status = 0;
    }
}