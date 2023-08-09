using System.Collections.Generic;


[System.Serializable]
public class M_StageSpawnMonster
{
    public int Id;
    public int stage;
    public int meleemonster_spawn;
    public int rangedmonster_spawn;
    public static Dictionary<int, M_StageSpawnMonster> table = new Dictionary<int, M_StageSpawnMonster> ();   
}