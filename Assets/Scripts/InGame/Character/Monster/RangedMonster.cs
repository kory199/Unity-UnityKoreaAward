using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedMonster : MonsterBase
{
    private Bullet bullet;
    private Rigidbody2D bulletRb;

    // temp monster status
    [SerializeField] protected int rangedMonster_Level;
    [SerializeField] protected int rangedMonster_exp;
    [SerializeField] protected float rangedMonster_Hp;
    [SerializeField] protected float rangedMonster_CurHp;
    [SerializeField] protected float rangedMonster_Speed;
    [SerializeField] protected float rangedMonster_RateOfFire;
    [SerializeField] protected float rangedMonster_ProjectileSpeed;
    [SerializeField] protected float rangedMonster_CollisionDamage;
    [SerializeField] protected int rangedMonster_Score;
    [SerializeField] protected float rangedMonster_Range;
    [SerializeField] private bool isRangedMonsterDead;

    #region unity event func
    protected override void Awake()
    {
        base.Awake();
        isRangedMonsterDead = false;
    }

    protected override void Start()
    {
        base.Start();
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Monster, EnumTypes.StageStateType.Awake, GetRangedMonsterInfo);
    }


    // stage 변경에 따른 Level별 능력치 부여 => 서버 정보 받아오기
    protected override void OnEnable()
    {
        base.OnEnable();
        if (isRangedMonsterDead == true)
        {
            SetRangedMonsterStatus(stageNum);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isRangedMonsterDead = true;
    }

    protected override void SetMonsterName()
    {
        MonsterName = "RangedMonster";
    }

    private void SetRangedMonsterStatus(int inputStageNum)
    {
        rangedMonster_Level = rangedMonsterStatus[inputStageNum].level;
        rangedMonster_exp = rangedMonsterStatus[inputStageNum].exp;
        rangedMonster_Hp = rangedMonsterStatus[inputStageNum].hp;
        rangedMonster_CurHp = rangedMonster_Hp;
        rangedMonster_Speed = rangedMonsterStatus[inputStageNum].speed;
        rangedMonster_RateOfFire = rangedMonsterStatus[inputStageNum].rate_of_fire;
        rangedMonster_ProjectileSpeed = rangedMonsterStatus[inputStageNum].projectile_speed;
        rangedMonster_CollisionDamage = rangedMonsterStatus[inputStageNum].collision_damage;
        rangedMonster_Score = rangedMonsterStatus[inputStageNum].score;
        rangedMonster_Range = rangedMonsterStatus[inputStageNum].ranged;
    }

    protected override void MonsterStatusUpdate()
    {
        SetRangedMonsterStatus(stageNum);
    }

    #endregion
    protected override IEnumerator State_Move()
    {
        // 추후 몬스터 별 이동속도 및 공격 범위 추가
        return base.State_Move();
    }

    public override void Attack()
    {
        Debug.LogError("Ranged Monster Attack : " + rangedMonster_CollisionDamage);

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

    public override void Hit()
    {
        rangedMonster_CurHp -= player.playerAttackPower;

        if (rangedMonster_CurHp <= 0)
        {
            player.Reward(rangedMonster_exp, rangedMonster_Score);
            MonsterDeath();
        }
    }

    public void PlayerHit()
    {
        player.PlayerHit(rangedMonster_CollisionDamage);
    }
}
