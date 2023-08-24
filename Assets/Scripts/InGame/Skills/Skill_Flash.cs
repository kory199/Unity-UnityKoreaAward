using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Flash : SkillBase
{
    public override void SkillCoolTime()
    {
        //스킬 쿨타임
        _coolTime = 10f;
    }

    public override void SkillLevelUp()
    {
        //스킬 레벨업시 효과
        _skillLevel++;
    }

    public override void SkillShot()
    {
        //자유 기능 구현
        Debug.Log("점멸");
    }
    private void Awake()
    {
        Debug.Log("skill flash awake");

    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Debug.Log("skill flash start");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}