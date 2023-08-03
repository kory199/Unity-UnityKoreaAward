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
<<<<<<< HEAD
                Debug.Log("Time.time - lastAttackTime : " + (Time.time - lastAttackTime));
                Debug.Log("Time.time : " + Time.time);
                Debug.Log("lastAttackTime  1: " + lastAttackTime);
=======
>>>>>>> 7b445977e969774cac433a4390f64c66acca0c77
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
