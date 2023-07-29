namespace APIServer.DbModel;

public class Stage
{
    public Int64 player_uid { get; set; }
    public Int32 stage_id { get; set; }
    public Boolean is_achieved { get; set; }
}

public class StageInfo
{
    public Int32 stage_id { get; set; }
    public Int32 prev_stage_id { get; set; }
}