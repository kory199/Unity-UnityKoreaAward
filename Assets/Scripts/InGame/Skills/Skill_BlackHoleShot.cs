using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_BlackHoleShot : SkillBase
{
    // script
    Bullet_BlackHoleShot bullet_BlackHoleShot;

    // bullet Info
    GameObject bullet;
    Rigidbody2D bulletRb;
    private float healingAmount;


    #region Unity Life Cycle
    private void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet/Bullet_Skill_BlackHoleShot");
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
        GameObject Bullet_BlackHoleShotOject = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        if (Bullet_BlackHoleShotOject != null)
        {
            if (Bullet_BlackHoleShotOject.TryGetComponent<Bullet_BlackHoleShot>(out Bullet_BlackHoleShot bullet_BlackHoleShot))
            {
                this.bullet_BlackHoleShot = bullet_BlackHoleShot;
            }
            else
            {
                this.bullet_BlackHoleShot = Bullet_BlackHoleShotOject.AddComponent<Bullet_BlackHoleShot>();
            }

            if (Bullet_BlackHoleShotOject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                bulletRb = rigidbody2D;
            }
            else
            {
                bulletRb = Bullet_BlackHoleShotOject.AddComponent<Rigidbody2D>();
            }

            bullet_BlackHoleShot.SetBlackHoleScale(_skillLevel);
            bulletRb.velocity = _player.targetDirection * _player.playerProjectileSpeed;
        }
    }
}
