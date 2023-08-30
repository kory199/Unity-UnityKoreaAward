using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnergeShield : SkillBase
{
    protected override void Start()
    {
        base.Start();
        SkillCoolTime();
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SkillCoolTime()
    {
        _coolTime = 10f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;
    }

    public override void SkillShot()
    {
        throw new System.NotImplementedException();
    }
}
