using System.Collections;
using System.Collections.Generic;
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

    public class StageData : GameData
    {
        public int StageNum { get; set; }
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

    public class StageInfo
    {
        public long player_uid { get; set; }
        public int stage_id { get; set; }
        public bool is_achieved { get; set; }
    }
}