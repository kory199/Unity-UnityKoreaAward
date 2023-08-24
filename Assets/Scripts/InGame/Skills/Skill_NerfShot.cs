using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_NerfShot : SkillBase
{
    #region Unity Life Cycle
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    public override void SkillCoolTime()
    {
        _coolTime = 1f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;
    }

    public override void SkillShot()
    {
        // monster damage decrease
        // ∞®º“¿≤ : _skillLevel*0.1f
    }
}
