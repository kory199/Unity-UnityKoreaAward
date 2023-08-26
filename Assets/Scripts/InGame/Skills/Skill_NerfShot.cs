using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_NerfShot : SkillBase
{
    private float damageReduction;

    // Script
    Player player;
    Bullet_NerfShot bullet_NerfShot;


    // bullet info
    private GameObject bullet;
    private Rigidbody2D bulletRb;

    #region Unity Life Cycle

    private void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet/Bullet_Skill_NerfShot");
    }

    protected override void Start()
    {
        base.Start();
        InitNerfShotSkill();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    #endregion

    public override void SkillCoolTime()
    {
        _coolTime = 5f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;
    }

    public override void SkillShot()
    {
        GameObject Bullet_NerfShotOject = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        if (Bullet_NerfShotOject != null)
        {
            if (Bullet_NerfShotOject.TryGetComponent<Bullet_NerfShot>(out Bullet_NerfShot bullet_NerfShot))
            {
                this.bullet_NerfShot = bullet_NerfShot;
            }
            else
            {
                bullet_NerfShot = Bullet_NerfShotOject.AddComponent<Bullet_NerfShot>();
            }

            if (Bullet_NerfShotOject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                bulletRb = rigidbody2D;
            }
            else
            {
                bulletRb = Bullet_NerfShotOject.AddComponent<Rigidbody2D>();
            }

            bullet_NerfShot.SetDamageReduction(CalculateDamageReduction(_skillLevel));

            bulletRb.velocity = player.targetDirection * player.playerProjectileSpeed;
        }
    }

    private void InitNerfShotSkill()
    {
        if (gameObject.TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }
        else
        {
            player = gameObject.AddComponent<Player>();
        }
    }

    private float CalculateDamageReduction(int skillLevel)
    {
        damageReduction = 1 - (skillLevel * 0.1f);

        return damageReduction;
    }
}
