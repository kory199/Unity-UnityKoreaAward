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

    // 임시 : 서버로부터 받아야됨
    [Header("User Setting")]
    public float playerSpeed;
    public float playerMaxHp;
    public float playerCurHp = int.MaxValue;
    public int playerAttackPower;
    public int playerLv;
    public int playerMaxExp;
    public int playerCurExp;
    public float playerMovementSpeed;
    public float playerProjectileSpeed;
    public float playerRateOfFire;
    public float lastAttackTime = 0;

    bool isMoveable;
    WaitForSeconds moveAble;

    PlayerStatus_res[] playerStatus;

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

    private void InitPlayer(int playerLv)
    {
        IsDeath = false;
        isMoveable = true;
        moveAble = new WaitForSeconds(0.5f);
    }

    private void retrunPlayerInfo(int inputPlayerLV)
    {
        playerMaxHp = playerStatus[inputPlayerLV - 1].hp;
        playerCurHp = playerMaxHp;
        playerAttackPower = playerStatus[inputPlayerLV - 1].attack_power;
        playerMaxExp = playerStatus[inputPlayerLV - 1].xp_requiredfor_levelup;
        playerMovementSpeed = playerStatus[inputPlayerLV - 1].movement_speed;
        playerProjectileSpeed = playerStatus[inputPlayerLV - 1].projectile_speed;
        playerRateOfFire = playerStatus[inputPlayerLV - 1].rate_of_fire;

        InitPlayerUI();
    }

    private void InitPlayerUI()
    {
        uI_SceneGame.SetMaxHP(playerMaxHp);
        uI_SceneGame.SetNowHP(playerCurHp);
        uI_SceneGame.SetAttackPower(playerAttackPower);
        uI_SceneGame.SetLevel(playerLv + 1);
        uI_SceneGame.SetMaxExp(playerMaxExp);
        uI_SceneGame.SetNowExp(playerCurExp);
    }
}
