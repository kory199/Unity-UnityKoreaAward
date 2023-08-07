using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonsterBase
{
    // temp monster status
    private int meleeMonster_Level;
    private int meleeMonster_exp;
    private float meleeMonster_Hp;
    private float meleeMonster_Speed;
    private float meleeMonster_RateOfFire;
    private float meleeMonster_ProjectileSpeed;
    private float meleeMonster_CollisionDamage;
    private int meleeMonster_Score;
    private float meleeMonster_Range;


    protected override void Start()
    {
        base.Start();
        // InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonsterStatus);

        SetMeleeMonsterStatus();
    }

    // stage 변경에 따른 Level별 능력치 부여 => 서버 정보 받아오기
    protected override void OnEnable()
    {
        base.OnEnable();
        // init melee monster variable

    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }
    protected override void Attack()
    {
        transform.Rotate(0, 0, 30);

        // player.PlayerHit(meleeMonster_CollisionDamage);
    }

    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }

    private void SetMeleeMonsterStatus()
    {
        meleeMonster_Level = meleeMonsterStatus[0].level;
        meleeMonster_exp = meleeMonsterStatus[0].exp;
        meleeMonster_Hp = meleeMonsterStatus[0].hp;
        meleeMonster_Speed = meleeMonsterStatus[0].speed;
        meleeMonster_RateOfFire = meleeMonsterStatus[0].rate_of_fire;
        meleeMonster_ProjectileSpeed = meleeMonsterStatus[0].projectile_speed;
        meleeMonster_CollisionDamage = meleeMonsterStatus[0].collision_damage;
        meleeMonster_Score = meleeMonsterStatus[0].score;
        meleeMonster_Range = meleeMonsterStatus[0].ranged;
    }
}