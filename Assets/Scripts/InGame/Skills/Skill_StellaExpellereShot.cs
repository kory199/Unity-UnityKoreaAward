using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_StellaExpellereShot : SkillBase
{
    // Script
    Bullet_StellaExpellere bullet_StellaExpellereShot;

    // bullet Info
    GameObject bullet;
    Rigidbody2D bulletRb;

    private void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet/Bullet_Skill_StellaExpellereShot");
    }

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
        GameObject Bullet_StellaExpellereShotOject = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        if (Bullet_StellaExpellereShotOject != null)
        {
            if (Bullet_StellaExpellereShotOject.TryGetComponent<Bullet_StellaExpellere>(out Bullet_StellaExpellere bullet_StellaExpellere))
            {
                this.bullet_StellaExpellereShot = bullet_StellaExpellere;
            }
            else
            {
                this.bullet_StellaExpellereShot = Bullet_StellaExpellereShotOject.AddComponent<Bullet_StellaExpellere>();
            }

            if (Bullet_StellaExpellereShotOject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                bulletRb = rigidbody2D;
            }
            else
            {
                bulletRb = Bullet_StellaExpellereShotOject.AddComponent<Rigidbody2D>();
            }

            bullet_StellaExpellereShot.SetStellaExpellereScale(_skillLevel);
            bulletRb.velocity = _player.targetDirection * _player.playerProjectileSpeed;
        }
    }
}
