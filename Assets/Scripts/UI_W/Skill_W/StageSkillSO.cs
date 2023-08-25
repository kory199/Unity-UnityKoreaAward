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

// NerfShot : 몬스터의 기본 능력치 (공격력) 을 감소시킴 (보스는 50% 효과만 적용)
// RepidShot : 총알의 이동 속도와 연사 속도 상승
// Flash : 바라보는 방향으로 일정 거리 순간이동


// Depth2
// DoubleShot) 더블 샷: 직선방향으로 총알을 2발 발사
// MultiShot)  멀티 샷: 일정 각도로 총알을 여러발 발사
// SlowingShot) 둔화 샷: 적을 둔화 시키는 총알 발사