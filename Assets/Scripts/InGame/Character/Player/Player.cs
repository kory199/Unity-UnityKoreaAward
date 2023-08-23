using System.Collections;
using APIModels;
using UnityEngine;

public partial class Player : CharacterBase
{
    [SerializeField] private float _spawnTime = 3f;

    #region unity event func
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        isStart = false;
        StartCoroutine(InitializationAfterDelay());
    }

    protected override void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
    }

    void Update()
    {
        if (isStart)
        {
            Move();
            UIControl();

            if (Input.GetKey(KeyCode.Mouse0))
            {
                if (Time.time - lastAttackTime >= 0.1f)
                {
                    Attack();

                    lastAttackTime = Time.time;
                }
            }
        }
    }

    #endregion
}
