using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonsterBase
{
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