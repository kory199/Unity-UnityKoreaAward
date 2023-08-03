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
    private float RangedMonster_CollisionDamage;
    private int rangedMonster_Score;
    private float rangedMonster_Range;

    // Start is called before the first frame update
    void Start()
    {
        // variable init
        // rangedMonster_Level = 0;
        // rangedMonster_exp = 0;
        // rangedMonster_Hp = 0;
        // rangedMonster_Speed = 0;
        // rangedMonster_RateOfFire = 0;
        // rangedMonster_ProjectileSpeed = 0;
        // RangedMonster_CollisionDamage = 0;
        // rangedMonster_Score = 0;
        // rangedMonster_Range = 0;
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

    protected override void SetMonsterName()
    {
        throw new System.NotImplementedException();
    }
}
