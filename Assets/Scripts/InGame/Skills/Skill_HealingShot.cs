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
    private float healingAmount;

    #region unity life cycle

    private void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet/Bullet_Skill_HealingShot");
    }

    protected override void Start()
    {
        base.Start();
        healingAmount = 10f;

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
        GameObject Bullet_HealingShotOject = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        if (Bullet_HealingShotOject != null)
        {
            if (Bullet_HealingShotOject.TryGetComponent<Bullet_HealingShot>(out Bullet_HealingShot bullet_HealingShot))
            {
                this.bullet_HealingShot = bullet_HealingShot;
            }
            else
            {
                this.bullet_HealingShot = Bullet_HealingShotOject.AddComponent<Bullet_HealingShot>();
            }

            if (Bullet_HealingShotOject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                bulletRb = rigidbody2D;
            }
            else
            {
                bulletRb = Bullet_HealingShotOject.AddComponent<Rigidbody2D>();
            }

            this.bullet_HealingShot.SetRecoveryAmount(CalculateHealAmount(_skillLevel));

            bulletRb.velocity = _player.targetDirection * _player.playerProjectileSpeed;
        }
    }

    private float CalculateHealAmount(int skillLevel)
    {
        float calvalue = skillLevel * healingAmount;

        return calvalue;
    }
}
