using System.Collections;
using UnityEngine;

public partial class Player : CharacterBase
{
    [SerializeField] private float _spawnTime = 3f;
    [SerializeField] private Animator _animator = null;

    #region unity event func
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
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
                    _animator.SetTrigger("Attack");
                    Attack();

                    lastAttackTime = Time.time;
                }
            }
        }
    }
    #endregion
}