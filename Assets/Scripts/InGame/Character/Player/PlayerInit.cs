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
    public float playerSpeed;
    public int playerMaxHp;
    public float playerCurHp;
    public int playerAttackPower;
    public int playerLv;
    public int playerMaxExp;
    public int playerCurExp = 0;
    public float playerMovementSpeed;
    public float playerProjectileSpeed;
    public float playerRateOfFire = 0.3f;
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

    private async void InitPlayer()
    {
        await APIManager.Instance.GetMasterDataAPI();

        PlayerStatus_res[] playerStatus = APIDataSO.Instance.GetValueByKey<PlayerStatus_res[]>(APIDataDicKey.PlayerStatus);

        // Level 1 기준 초기 셋팅
        playerMaxHp = playerStatus[0].hp;
        playerCurHp = playerMaxHp;
        playerAttackPower = playerStatus[0].attack_power;
        playerMaxExp = playerStatus[0].xp_requiredfor_levelup;
        playerMovementSpeed = playerStatus[0].movement_speed;
        playerProjectileSpeed = playerStatus[0].projectile_speed;
        playerRateOfFire = playerStatus[0].rate_of_fire;

        // PlayerData player = APIDataSO.Instance.GetValueByKey<PlayerData>(APIDataDicKey.PlayerData);
        // TODO : 로그인 후 해당 유저 데이터 추가예정
        // playerMaxHp = player.hp;
        // playerCurHp = playerMaxHp;
        // playerMaxExp = player.exp;


        IsDeath = false;
    }
}
