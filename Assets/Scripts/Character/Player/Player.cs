using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : CharacterBase
{
    [Header("User Setting")]
    [SerializeField] float playerSpeed;

    Rigidbody playerRb;

    protected override void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
    }

    private void Awake()
    {
        InitComponent();
        InitSetting();
    }

    void Update()
    {
        Move();
    }

    public override void Attack()
    {
        // 공격 동작 및 추가 항목
    }

    protected override void Die()
    {
        // player Die
    }
}
