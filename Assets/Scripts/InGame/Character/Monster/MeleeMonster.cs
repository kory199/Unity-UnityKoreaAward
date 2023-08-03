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

    private void Start()
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
        StageManager.Instacne.MonsterDeath();
    }
    protected override void Attack()
    {
        transform.Rotate(0, 0, 30);

    }

    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }
}