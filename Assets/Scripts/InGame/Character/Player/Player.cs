using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIModels;

public partial class Player : CharacterBase
{
    private void Awake()
    {
        InitSetting();
        InitComponent();
        InitPlayer();
    }

    protected override void Start()
    {
        // ��Ÿ �ʱ�ȭ �׸� �߰�
        base.Start();
    }

    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - lastAttackTime >= playerRateOfFire)
            {
                Attack();

                lastAttackTime = Time.time;
            }
        }
    }
}
