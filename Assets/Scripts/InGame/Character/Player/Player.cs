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
            if (Time.time - lastAttackTime >= rateOfFire)
            {
                Attack();
                Debug.Log("Time.time - lastAttackTime : " + (Time.time - lastAttackTime));
                Debug.Log("Time.time : " + Time.time);
                Debug.Log("lastAttackTime  1: " + lastAttackTime);
                lastAttackTime = Time.time;
                Debug.Log("lastAttackTime  2: " + lastAttackTime);
            }
        }
    }

    protected override void Die()
    {
        // player Die
    }
}
