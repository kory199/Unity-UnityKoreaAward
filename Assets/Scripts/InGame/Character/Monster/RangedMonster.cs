using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : MonsterBase
{
    private Bullet bullet;
    private Rigidbody2D bulletRb;

    // temp monster status
    private int rangedMonster_Level;
    private int rangedMonster_exp;
    private float rangedMonster_Hp;
    private float rangedMonster_Speed;
    private float rangedMonster_RateOfFire;
    private float rangedMonster_ProjectileSpeed;
    private float rangedMonster_CollisionDamage;
    private int rangedMonster_Score;
    private float rangedMonster_Range;

    // stage 변경에 따른 Level별 능력치 부여 => 서버 정보 받아오기
    private void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
        SetRangedMonsterStatus();
    }

    protected override void Attack()
    {
        GameObject pullBullet = ObjectPooler.SpawnFromPool("Bullet2D", gameObject.transform.position);
        playerTargetDirection = (player.transform.position - gameObject.transform.position).normalized;

        // Bullet에 발사 정보 전달
        if (pullBullet.TryGetComponent<Bullet>(out Bullet bull))
        {
            bullet = bull;
        }
        else
        {
            bullet = pullBullet.AddComponent<Bullet>();
        }

        bullet.SetShooter(gameObject);

        if (pullBullet.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            bulletRb = rb;
        }
        else
        {
            bulletRb = pullBullet.AddComponent<Rigidbody2D>();
        }

        // 속도 가중치는 서버 데이터 업로드 후 변경
        bulletRb.velocity = playerTargetDirection * rangedMonster_ProjectileSpeed;
    }
    private void SetRangedMonsterStatus()
    {
        rangedMonster_Level = rangedMonsterStatus[0].level;
        rangedMonster_exp = rangedMonsterStatus[0].exp;
        rangedMonster_Hp = rangedMonsterStatus[0].hp;
        rangedMonster_Speed = rangedMonsterStatus[0].speed;
        rangedMonster_RateOfFire = rangedMonsterStatus[0].rate_of_fire;
        rangedMonster_ProjectileSpeed = rangedMonsterStatus[0].projectile_speed;
        rangedMonster_CollisionDamage = rangedMonsterStatus[0].collision_damage;
        rangedMonster_Score = rangedMonsterStatus[0].score;
        rangedMonster_Range = rangedMonsterStatus[0].ranged;
    }

    protected override void SetMonsterName()
    {
        throw new System.NotImplementedException();
    }
}
