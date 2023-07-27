using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player Data", order = int.MaxValue)]
public class PlayerBaseData : ScriptableObject
{
    List<object> responseDataList = new();

    [SerializeField] public string id;
    [SerializeField] public int exp;
    [SerializeField] public int hp;
    [SerializeField] public int score;
    [SerializeField] public int level;
    [SerializeField] public int status;
}
