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
    private bool isMoveable;
    private bool isStart;

    private WaitForSeconds moveAble;
    public UI_SceneGame uI_SceneGame;
    private UI_Enhance uI_Enhance;
    private PlayerStatus_res[] playerStatus;

    public Vector3 targetDirection;
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
        isStart = true;
        IsDeath = false;
        isMoveable = true;
        moveAble = new WaitForSeconds(0.5f);
    }

    private void retrunPlayerInfo(int inputPlayerLV)
    {
        playerMaxHp = playerStatus[inputPlayerLV].hp;
        playerCurHp = playerMaxHp;
        playerAttackPower = playerStatus[inputPlayerLV].attack_power;
        playerMaxExp = playerStatus[inputPlayerLV].xp_requiredfor_levelup;
        playerMovementSpeed = playerStatus[inputPlayerLV].movement_speed;
        playerProjectileSpeed = playerStatus[inputPlayerLV].projectile_speed;
        playerRateOfFire = playerStatus[inputPlayerLV].rate_of_fire;

        InitPlayerUI();
    }

    public void InitPlayerUI()
    {
        uI_SceneGame.SetMaxHP(playerMaxHp);
        uI_SceneGame.SetNowHP(playerCurHp);
        uI_SceneGame.SetAttackPower(playerAttackPower);
        uI_SceneGame.SetLevel(playerLv + 1);
        uI_SceneGame.SetMaxExp(playerMaxExp);
        uI_SceneGame.SetNowExp(playerCurExp);
    }

    private void InitUI_Enhance()
    {
        uI_Enhance = UIManager.Instance.CreateObject<UI_Enhance>("UI_Enhance", EnumTypes.LayoutType.First);
        uI_Enhance.OnHide();
        Time.timeScale = 1;
    }

    private IEnumerator InitializationAfterDelay()
    {
        playerStatus = APIManager.Instance.GetValueByKey<PlayerStatus_res[]>(MasterDataDicKey.PlayerStatus.ToString());
        
        yield return new WaitForSeconds(_spawnTime);

        InitUI_Enhance();
        InitSetting();
        InitComponent();
        InitPlayerUI();
        retrunPlayerInfo(0);
        InitPlayer();
    }
}
