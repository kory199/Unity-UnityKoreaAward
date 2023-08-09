using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonsterBase
{
    // temp monster status
    [SerializeField] private int meleeMonster_Level;
    [SerializeField] private int meleeMonster_exp;
    [SerializeField] private float meleeMonster_Hp;
    [SerializeField] private float meleeMonster_CurHp;
    [SerializeField] private float meleeMonster_Speed;
    [SerializeField] private float meleeMonster_RateOfFire;
    [SerializeField] private float meleeMonster_ProjectileSpeed;
    [SerializeField] private float meleeMonster_CollisionDamage;
    [SerializeField] private int meleeMonster_Score;
    [SerializeField] private float meleeMonster_Range;

    

    #region unity event func
    // stage 변경에 따른 Level별 능력치 부여 => 서버 정보 받아오기
    protected override void OnEnable()
    {
        base.OnEnable();

        // init melee monster variable
    }

    protected override void Start()
    {
        base.Start();
        SetMeleeMonsterStatus();
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        player.Reward(meleeMonster_exp);
    }
    #endregion
    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }

    protected void SetMeleeMonsterStatus()
    {
        meleeMonster_Level = meleeMonsterStatus[0].level;
        meleeMonster_exp = meleeMonsterStatus[0].exp;
        meleeMonster_Hp = meleeMonsterStatus[0].hp;
        meleeMonster_CurHp = meleeMonster_Hp;
        meleeMonster_Speed = meleeMonsterStatus[0].speed;
        meleeMonster_RateOfFire = meleeMonsterStatus[0].rate_of_fire;
        meleeMonster_ProjectileSpeed = meleeMonsterStatus[0].projectile_speed;
        meleeMonster_CollisionDamage = meleeMonsterStatus[0].collision_damage;
        meleeMonster_Score = meleeMonsterStatus[0].score;
        meleeMonster_Range = meleeMonsterStatus[0].ranged;
    }

    public override void Attack()
    {
        transform.Rotate(0, 0, 30);
        // 임시
        PlayerHit();
    }

    public override void Hit()
    {
        meleeMonster_CurHp -= player.playerAttackPower;

        if (meleeMonster_CurHp <= 0)
        {
            MonsterDeath();
        }
    }

    public void PlayerHit()
    {
        player.PlayerHit(meleeMonster_CollisionDamage);
    }

    protected override IEnumerator State_Move()
    {
        // 추후 몬스터 별 이동속도 및 공격 범위 추가
        return base.State_Move();
    }
}