using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : CharacterBase
{
    protected override void Start()
    {
        // ��Ÿ �ʱ�ȭ �׸� �߰�
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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            Attack();
            if (Time.time - lastAttackTime >= rateOfFire)
            {
                lastAttackTime = Time.time;
            }
        }
    }

    protected override void Die()
    {
        // player Die
    }
}
