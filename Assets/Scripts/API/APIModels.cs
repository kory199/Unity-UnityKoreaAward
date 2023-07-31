using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

namespace APIModels
{
    public class GetVersion
    {
        public string Version = "GetVersion";
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

    public class MonsterData
    {
        public string monsterType { get; set; }
        public int level { get; set; }
        public int exp { get; set; }
        public float hp { get; set; }
        public float speed { get; set; }
        public float rateOfFire { get; set; }
        public float collisionDamage { get; set; }
        public int score { get; set; }
        public float ranged { get; set; }
    }
}