using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_RapidShot : SkillBase
{
    private float projectileSpeed;
    private float rateOfFire;
    private float originProjectileSpeed;
    private float originrateOfFire;

    // bullet info
    private GameObject bullet;
    private Rigidbody2D bulletRb;

    [Header("Script")]
    private Bullet_RapidShot bullet_RapidShot;

    #region Unity lifeCycle
    private void Awake()
    {
        bullet = Resources.Load<GameObject>("Bullet/Bullet_Skill_RapidShot");
        InitPlayerComponent();
    }
    protected override void Start()
    {
        base.Start();

        SkillCoolTime();
        projectileSpeed = 1f;
        rateOfFire = 1f;
        originProjectileSpeed = 0;
        originrateOfFire = 0;
    }

    protected override void Update()
    {
        base.Update();
    }
    #endregion

    public override void SkillCoolTime()
    {
        _coolTime = 0.5f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;
        SetSkillElement(_skillLevel);
    }

    public override void SkillShot()
    {
        GameObject Bullet_RapidShotOject = Instantiate(bullet, gameObject.transform.position, Quaternion.identity);

        if (Bullet_RapidShotOject != null)
        {
            if (Bullet_RapidShotOject.TryGetComponent<Bullet_RapidShot>(out Bullet_RapidShot bullet_RapidShot))
            {
                this.bullet_RapidShot = bullet_RapidShot;
            }
            else
            {
                bullet_RapidShot = Bullet_RapidShotOject.AddComponent<Bullet_RapidShot>();
            }

            if (Bullet_RapidShotOject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
            {
                bulletRb = rigidbody2D;
            }
            else
            {
                bulletRb = Bullet_RapidShotOject.AddComponent<Rigidbody2D>();
            }

            SetSkillElement(_skillLevel);
            bulletRb.velocity = _player.targetDirection * _player.playerProjectileSpeed;
            ResetSkillElement();
        }
    }

    private void SetSkillElement(int skillLevel)
    {
        projectileSpeed = 1 + (skillLevel * 0.2f);
        rateOfFire = 1 + (skillLevel * 0.2f);

        originProjectileSpeed = _player.playerProjectileSpeed;
        originrateOfFire = _player.playerRateOfFire;

        _player.playerProjectileSpeed *= projectileSpeed;
        _player.playerRateOfFire *= rateOfFire;
    }

    private void InitPlayerComponent()
    {
        if (gameObject.TryGetComponent<Player>(out Player player))
        {
            _player = player;
        }
        else
        {
            _player = gameObject.AddComponent<Player>();
        }
    }

    private void ResetSkillElement()
    {
        _player.playerProjectileSpeed = originProjectileSpeed;
        _player.playerRateOfFire = originrateOfFire;
    }
}
