using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace APIModels
{
    // === Request === 
    public class Version_req
    {
        public string Version = "GetVersion";
    }

    public class MasterData_req
    {
        public string MasterData = "GetMasterData";
    }

    public class User
    {
        public string ID;
        public string Password;
    }

    public class GameData
    {
        public string ID { get; set; }
        public string AuthToken { get; set; }
    }

    public class StageData : GameData
    {
        public int StageNum { get; set; }
    }

    // === Response === 
    public class PlayerData
    {
        public string id { get; set; }
        public int exp { get; set; }
        public int hp { get; set; }
        public int score { get; set; }
        public int status { get; set; }
    }

    public class RankingData
    {
        public string id { get; set; }
        public int score { get; set; }
        public int ranking { get; set; }
    }

    public class StageInfo
    {
        public int stage_id { get; set; }
        public bool is_achieved { get; set; }
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