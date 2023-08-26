using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_HealingShot : SkillBase
{
    // script
    Bullet_HealingShot bullet_HealingShot;

    // bullet Info
    GameObject bullet;
    Rigidbody2D bulletRb;

    #region unity life cycle

    private void Awake()
    {
        
    }

    protected override void Start()
    {
        base.Start();

        SkillCoolTime();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    #endregion

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
