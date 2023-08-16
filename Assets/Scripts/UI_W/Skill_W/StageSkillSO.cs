using UnityEngine;
using System;

[Serializable]
public class SkillInfo
{
    public Sprite skillImg;
    public string infotext;
    public int bulletNum;
}

[CreateAssetMenu(fileName = "SKillSO", menuName = "ScriptableObjects/SKillSO", order = int.MaxValue)]
public class StageSkillSO : ScriptableObject
{
    public SkillInfo[] sateOne = new SkillInfo[3];
}