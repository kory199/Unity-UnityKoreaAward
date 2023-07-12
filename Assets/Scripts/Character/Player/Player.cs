using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : CharacterBase
{
    float playerSpeed;

    void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
        init();
    }

    void Update()
    {
        Move(playerSpeed);
    }

    public override void Attack()
    {
        // 공격 동작 및 추가 항목
    }

    protected override void Die()
    {
        // player Die
    }

    public override void Move(float moveSpeed)
    {
        base.Move(moveSpeed);
    }

    private void init()
    {
        // 초기화 항목
    }
}
