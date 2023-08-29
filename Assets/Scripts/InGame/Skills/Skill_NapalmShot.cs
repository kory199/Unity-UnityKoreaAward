using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_NapalmShot : SkillBase
{
    // script
    Bullet_NapalmShot bullet_NapalmShot;

    // bullet Info
    GameObject bullet;
    Rigidbody2D bulletRb;

    #region Unity Life Cycle
    private void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet/Bullet_Skill_NapalmShot");
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
        GameObject Bullet_NapalmShotOject = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        if (Bullet_NapalmShotOject != null)
        {
            if (Bullet_NapalmShotOject.TryGetComponent<Bullet_NapalmShot>(out Bullet_NapalmShot bullet_NapalmShot))
            {
                this.bullet_NapalmShot = bullet_NapalmShot;
            }
            else
            {
                this.bullet_NapalmShot = Bullet_NapalmShotOject.AddComponent<Bullet_NapalmShot>();
            }

            if (Bullet_NapalmShotOject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                bulletRb = rigidbody2D;
            }
            else
            {
                bulletRb = Bullet_NapalmShotOject.AddComponent<Rigidbody2D>();
            }

            bullet_NapalmShot.SetNapalmShotScale(_skillLevel);
            bulletRb.velocity = _player.targetDirection * _player.playerProjectileSpeed;
        }
    }
}
