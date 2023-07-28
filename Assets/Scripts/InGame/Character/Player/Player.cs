using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : CharacterBase
{
    protected override void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
    }

    private void Awake()
    {
        InitSetting();
        InitComponent();
        InitPlayer();
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
