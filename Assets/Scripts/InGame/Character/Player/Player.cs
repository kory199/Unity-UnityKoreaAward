using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIModels;
using TMPro;

public partial class Player : CharacterBase
{
    public UI_SceneGame uI_SceneGame;
    private UI_Enhance ui_Enhance;


    #region unity event func
    private void Awake()
    {
        playerStatus = APIManager.Instance.GetValueByKey<PlayerStatus_res[]>(MasterDataDicKey.PlayerStatus.ToString());


        InitSetting();
        InitComponent();
        InitPlayerUI();
        retrunPlayerInfo(0);
        InitPlayer();
    }

    protected override void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
    }

    void Update()
    {
        Move();
        UI_Control();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - lastAttackTime >= 0.1f)
            {
                Attack();

                lastAttackTime = Time.time;
            }
        }
    }
    #endregion
}
