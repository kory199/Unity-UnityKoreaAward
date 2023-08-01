using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public partial class Player
{
    PlayerBaseData playerBaseData;

    Rigidbody2D playerRb;

    [Header("User Setting")]
    [SerializeField] float playerSpeed;

    [SerializeField] public int playerMaxHp;
    [SerializeField] public int playerCurHp;
    public float playerMovementSpeed;
    public int playerAttackPower;
    public float rateOfFire;
    public float projectileSpeed;
    public int playerLv;
    public int playerMaxExp;

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

        if (APIManager.Instacne.isLogin)
        {
            playerMaxHp = playerBaseData.hp;
            playerMaxHp = 100;
            playerCurHp = playerMaxHp;
            playerLv = playerBaseData.level;
            playerMaxExp = playerBaseData.exp;
            IsDeath = false;
        }
    }
}
