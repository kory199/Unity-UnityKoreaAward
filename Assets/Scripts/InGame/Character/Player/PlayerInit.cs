using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public partial class Player
{
    PlayerBaseData playerBaseData;

    public int playerHp;
    public int playerLv;
    public int playerMp;
    public int playerExp;
    public bool IsDeath;

    private void InitComponent()
    {
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            playerRb = rigidbody;
        else
            playerRb = gameObject.AddComponent<Rigidbody>();
    }

    private void InitSetting()
    {
        playerSpeed = 100f;

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
