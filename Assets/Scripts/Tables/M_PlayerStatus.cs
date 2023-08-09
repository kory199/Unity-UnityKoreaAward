using System.Collections.Generic;


[System.Serializable]
public class M_PlayerStatus
{
    public int Id;
    public int level;
    public int hp;
    public float movement_speed;
    public int attack_power;
    public float rate_of_fire;
    public int projectile_speed;
    public int xp_requiredfor_levelup;
    public static Dictionary<int, M_PlayerStatus> table = new Dictionary<int, M_PlayerStatus> ();   
}