using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class MonsterInfo
{
    public string Name;
    public int Exp;
    public int Score;
    public float Hp;
    public float RateOfFire;
    public float Range;
    public float MoveSpeed;
    public MonsterInfo(string name, int exp, float hp, int score, float rateofFire, float range= 0,float moveSpeed= 0.5f)
    {
        this.Name = name;
        this.Exp = exp;
        this.Hp = hp;
        this.Score = score;
        this.RateOfFire = rateofFire;
        this.Range = range;
        this.MoveSpeed = moveSpeed;
    }
}
[CreateAssetMenu(fileName = "MonsterData", menuName = "ScriptableObjects/Monster Data", order = int.MaxValue)]
public class MonsterData : ScriptableObject
{
    public List<MonsterInfo> MonsterDataList = new List<MonsterInfo>();
    public void InsertMonsterInfo(MonsterInfo info)
    {
        MonsterDataList.Add(info);
    }
    public void InsertMonsterInfo()
    {
        MonsterDataList.Add(new MonsterInfo("BossOne", 100, 101, 102, 0.8f));
        MonsterDataList.Add(new MonsterInfo("BasicMeleeMonster", 100, 101, 102, 0.8f));
    }
    public bool TryGetMonsterInfo(string name, out MonsterInfo monsterInfo)
    {
        foreach (var info in MonsterDataList)
        {
            if (info.Name == name)
            {
                monsterInfo = info;
                return true;
            }
        }
        Debug.LogError("not found name");
        monsterInfo = null;
        return false;
    }
}
