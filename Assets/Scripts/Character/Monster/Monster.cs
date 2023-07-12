using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : CharacterBase
{
    float monsterSpeed;
    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        Move(monsterSpeed);
    }

    public override void Move(float moveSpeed)
    {
        base.Move(moveSpeed);
    }

    public override void Attack()
    {
        // 몬스터 공격 구현
    }

    protected override void Die()
    {
        // 몬스터 죽음
    }

    private void init()
    {

    }
}
