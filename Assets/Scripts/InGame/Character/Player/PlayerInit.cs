using System.Collections;
using System.Collections.Generic;
using APIModels;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public partial class Player
{
    public Rigidbody2D bulletRb;
    public Rigidbody2D playerRb;
    public Bullet bullet;

    // �ӽ� : �����κ��� �޾ƾߵ�
    [Header("User Setting")]
    [SerializeField] float playerSpeed;
    [SerializeField] public int playerMaxHp;
    [SerializeField] public int playerCurHp;
    public int playerAttackPower;
    public int playerLv;
    public int playerMaxExp;
    public int playerCurExp = 0;
    public float playerMovementSpeed;
    public float projectileSpeed = 5f;
    public float rateOfFire = 0.3f;
    public float lastAttackTime = 0;

    public bool IsDeath;

    private void InitComponent()
    {
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            playerRb = rigidbody;
        else
            playerRb = gameObject.AddComponent<Rigidbody2D>();
        playerRb.gravityScale = 0;
    }

    private void InitSetting()
    {
        playerSpeed = 3f;

        // Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void InitPlayer()
    {
        // Test
        playerMaxHp = 100;
        playerCurHp = playerMaxHp;

        //PlayerData player = APIDataSO.Instance.GetValueByKey<PlayerData>(APIDataDicKey.PlayerData);
        // TODO : 로그인 후 해당 유저 데이터 추가예정
        playerMaxHp = hp;
        playerMaxHp = 100;
        playerCurHp = playerMaxHp;
        playerMaxExp = playerMaxHp;
        IsDeath = false;
    }
}
