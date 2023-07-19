namespace APIServer.DbModel;

public class Ranking
{
    public String id { get; set; }
    public Int32 score { get; set; }
    public Int32 ranking { get; set; }
}

public class RankingData
{
    public List<Ranking> rankingList { get; set; } = new();
}