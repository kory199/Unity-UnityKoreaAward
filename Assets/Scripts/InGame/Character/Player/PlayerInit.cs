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
    public int playerMaxHp;
    public float playerCurHp = int.MaxValue;
    public int playerAttackPower;
    public int playerLv;
    public int playerMaxExp;
    public int playerCurExp = 0;
    public float playerMovementSpeed;
    public float playerProjectileSpeed;
    public float playerRateOfFire = 0.3f;
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
        playerStatus = APIManager.Instance.GetValueByKey<PlayerStatus_res[]>(MasterDataDicKey.PlayerStatus.ToString());
        // Level 1 기준 초기 셋팅
        // playerMaxHp = playerStatus[0].hp;
        this.playerLv = playerLv;

        if (playerStatus != null)
        {
            playerMaxHp = 1000;
            playerCurHp = playerMaxHp;
            playerAttackPower = playerStatus[playerLv - 1].attack_power;
            playerMaxExp = playerStatus[playerLv - 1].xp_requiredfor_levelup;
            playerMovementSpeed = playerStatus[playerLv - 1].movement_speed;
            playerProjectileSpeed = playerStatus[playerLv - 1].projectile_speed;
            playerRateOfFire = playerStatus[playerLv - 1].rate_of_fire;
        }
               

        // PlayerData player = APIDataSO.Instance.GetValueByKey<PlayerData>(APIDataDicKey.PlayerData);
        // TODO : 로그인 후 해당 유저 데이터 추가예정
        // playerMaxHp = player.hp;
        // playerCurHp = playerMaxHp;
        // playerMaxExp = player.exp;

        IsDeath = false;
        isMoveable = true;
        moveAble = new WaitForSeconds(0.5f);
    }

    // 추후 InitPlayer로 병합 예정 (서버 연결 확인 후 MasterData로부터 받아오기)
    private void retrunPlayerInfo(int inputPlayerLV)
    {
        playerMaxHp = 10000;
        playerCurHp = playerMaxHp;
        playerAttackPower = playerStatus[inputPlayerLV - 1].attack_power;
        playerMaxExp = playerStatus[inputPlayerLV - 1].xp_requiredfor_levelup;
        playerMovementSpeed = playerStatus[inputPlayerLV - 1].movement_speed;
        playerProjectileSpeed = playerStatus[inputPlayerLV - 1].projectile_speed;
        playerRateOfFire = playerStatus[inputPlayerLV - 1].rate_of_fire;
    }
}
