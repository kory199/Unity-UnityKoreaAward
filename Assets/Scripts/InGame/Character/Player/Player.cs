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

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - lastAttackTime >= rateOfFire)
            {
                Debug.Log("time : " + Time.time);
                Debug.Log("lastAttack Time  : " + lastAttackTime);
                Debug.Log("rateOfFire : " + rateOfFire);
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    protected override void Die()
    {
        // player Die
    }
}
