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

    public int playerHp;
    public int playerLv;
    public int playerMp;
    public int playerExp;
    public bool IsDeath;

    private void InitComponent()
    {
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            playerRb = rigidbody;
        else
            playerRb = gameObject.AddComponent<Rigidbody2D>();
    }

    private void InitSetting()
    {
        playerSpeed = 3f;

        // Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void InitPlayer()
    {
        if (APIManager.Instacne.isLogin)
        {
            playerHp = playerBaseData.hp;
            playerLv = playerBaseData.level;
            playerExp = playerBaseData.exp;
            IsDeath = false;
        }
    }
}
