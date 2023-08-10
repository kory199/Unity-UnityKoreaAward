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
        SetMeleeMonsterStatus(1);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    #endregion
    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }

    protected void SetMeleeMonsterStatus(int inputStageNum)
    {
        Debug.LogError("SetMeleeMonsterStatus : " + inputStageNum);

        meleeMonster_Level = meleeMonsterStatus[inputStageNum - 1].level;
        meleeMonster_exp = meleeMonsterStatus[inputStageNum - 1].exp;
        meleeMonster_Hp = meleeMonsterStatus[inputStageNum - 1].hp;
        meleeMonster_CurHp = meleeMonster_Hp;
        meleeMonster_Speed = meleeMonsterStatus[inputStageNum - 1].speed;
        meleeMonster_RateOfFire = meleeMonsterStatus[inputStageNum - 1].rate_of_fire;
        meleeMonster_ProjectileSpeed = meleeMonsterStatus[inputStageNum - 1].projectile_speed;
        meleeMonster_CollisionDamage = meleeMonsterStatus[inputStageNum - 1].collision_damage;
        meleeMonster_Score = meleeMonsterStatus[inputStageNum - 1].score;
        meleeMonster_Range = meleeMonsterStatus[inputStageNum - 1].ranged;
    }

    protected override void MonsterStatusUpdate()
    {
        Debug.LogError("MonsterStatusUpdate : " + stageNum);
        SetMeleeMonsterStatus(stageNum);
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
            player.Reward(meleeMonster_exp);
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