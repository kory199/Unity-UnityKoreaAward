namespace APIServer.DbModel;

public class PlayerStatus
{
    //public Int32 id { get; set; }
    public Int32 level { get; set; }
    public Int32 hp { get; set; }
    public float movement_speed { get; set; }
    public Int32 attack_power { get; set; }
    public float rate_of_fire { get; set; }
    public Int32 projectile_speed { get; set; }
    public Int32 xp_requiredfor_levelup { get; set; }
}