using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MasterDatasModel
{
    public class MasterDataResponse
    {
        public MasterDataArray masterDataDic { get; set; }
    }

    public class MasterDataArray
    {
        public MonsterData_res[] MeleeMonster { get; set; } = new MonsterData_res[4];
        public MonsterData_res[] RangedMonster { get; set; } = new MonsterData_res[4];
        public MonsterData_res BOSS { get; set; }
        public PlayerStatus_res[] PlayerStatus { get; set; } = new PlayerStatus_res[4];
        public StageSpawnMonsterData_res[] StageSpawnMonster { get; set; } = new StageSpawnMonsterData_res[4];
    }

    public class MonsterData_res
    {
        public string type { get; set; }
        public int level { get; set; }
        public int exp { get; set; }
        public float hp { get; set; }
        public float speed { get; set; }
        public float rate_of_fire { get; set; }
        public float projectile_speed { get; set; }
        public float collision_damage { get; set; }
        public int score { get; set; }
        public float ranged { get; set; }
    }

    public class PlayerStatus_res
    {
        public int level { get; set; }
        public int hp { get; set; }
        public float movement_speed { get; set; }
        public int attack_power { get; set; }
        public float rate_of_fire { get; set; }
        public int projectile_speed { get; set; }
        public int xp_requiredfor_levelup { get; set; }
    }

    public class StageSpawnMonsterData_res
    {
        public int stage { get; set; }
        public int meleemonster_spawn { get; set; }
        public int rangedmonster_spawn { get; set; }
    }
}