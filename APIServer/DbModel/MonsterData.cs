namespace APIServer.DbModel;

public class MonsterData
{
    public String type { get; set; } = "";
    public Int32 level { get; set; }
    public Int32 exp { get; set; }
    public float hp { get; set; }
    public float speed { get; set; }
    public float rate_of_fire { get; set; }
    public float projectile_speed { get; set; }
    public float collision_damage { get; set; }
    public Int32 score { get; set; }
    public float ranged { get; set; }
}