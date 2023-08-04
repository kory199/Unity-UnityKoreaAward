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


    private void Awake()
    {
        // InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonsterStatus);
    }

    // stage 변경에 따른 Level별 능력치 부여 => 서버 정보 받아오기
    private void OnEnable()
    {

        // init melee monster variable
        // meleeMonster_Level = 0;
        // meleeMonster_exp = 0;
        // meleeMonster_Hp = 0;
        // meleeMonster_Speed = 0;
        // meleeMonster_RateOfFire = 0;
        // meleeMonster_ProjectileSpeed = 0;
        // meleeMonster_CollisionDamage = 0;
        // meleeMonster_Score = 0;
        // meleeMonster_Range = 0;
    }



    protected override void OnDisable()
    {
        base.OnDisable();
        StageManager.Instance.MonsterDeath();
    }
    protected override void Attack()
    {
        transform.Rotate(0, 0, 30);

        // player.PlayerHit();
    }

    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }

    private void SetMeleeMonsterStatus()
    {
      // meleeMonster_Level = monsterData_Res[0].level;
      // meleeMonster_exp ;
      // meleeMonster_Hp;
      // meleeMonster_Speed;
      // meleeMonster_RateOfFire;
      // meleeMonster_ProjectileSpeed;
      // meleeMonster_CollisionDamage;
      // meleeMonster_Score;
      // meleeMonster_Range;
}
}