using UnityEngine;
using System;

[Serializable]
public class SkillInfo
{
    public Sprite skillImg;
    public string infotext;
    public int bulletNum;
    public string SkillClassName;
}

[CreateAssetMenu(fileName = "SKillSO", menuName = "ScriptableObjects/SKillSO", order = int.MaxValue)]
public class StageSkillSO : ScriptableObject
{
    public SkillInfo[] setOne = new SkillInfo[3];
    public SkillInfo[] setTwo = new SkillInfo[3];
    public SkillInfo[] setThree = new SkillInfo[3];
    public SkillInfo[] SetFour = new SkillInfo[3];
    public SkillInfo[] SetFive = new SkillInfo[3];
}