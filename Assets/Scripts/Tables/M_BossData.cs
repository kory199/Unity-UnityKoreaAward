using System.Collections.Generic;


[System.Serializable]
public class M_BossData
{
    public int Id;
    public string type;
    public int level;
    public int exp;
    public float hp;
    public float speed;
    public float rate_of_fire;
    public float projectile_speed;
    public float collision_damage;
    public int score;
    public float ranged;
    public static Dictionary<int, M_BossData> table = new Dictionary<int, M_BossData> ();   
}