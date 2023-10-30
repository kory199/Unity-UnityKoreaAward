using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player Data", order = int.MaxValue)]
public class PlayerBaseData : ScriptableObject
{
    public string id;
    public int exp;
    public int hp;
    public int score;
    public int level;
    public int status;
}
