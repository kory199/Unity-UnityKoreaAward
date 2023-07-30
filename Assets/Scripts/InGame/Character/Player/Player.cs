using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : CharacterBase
{
    [Header("User Setting")]
    [SerializeField] float playerSpeed;

    Rigidbody2D playerRb;
    

    protected override void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
    }

    private void Awake()
    {
        InitComponent();
        InitSetting();
        InitPlayer();
        playerRb.gravityScale = 0;
    }

    void Update()
    {
        Move();
        Attack();
    }

    protected override void Die()
    {
        // player Die
    }
}
