using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIModels;
using TMPro;

public partial class Player : CharacterBase
{

    // UI Test Component
    [SerializeField] private TextMeshProUGUI ui_Hp;
    [SerializeField] private TextMeshProUGUI ui_Exp;
    [SerializeField] private TextMeshProUGUI ui_Stage;
    [SerializeField] private TextMeshProUGUI ui_LV;
    [SerializeField] private TextMeshProUGUI ui_Attack;
    public UI_SceneGame uI_SceneGame;


    #region unity event func
    private void Awake()
    {
        InitSetting();
        InitComponent();
        InitPlayer(1);
    }

    protected override void Start()
    {
        // 기타 초기화 항목 추가
        base.Start();
    }

    void Update()
    {
        Move();

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (Time.time - lastAttackTime >= 0.1f)
            {
                Attack();

                lastAttackTime = Time.time;
            }
        }

        // temp ui (추후 각 컴포넌트 별 변경 시 반영)
        ui_Hp.text = playerCurHp.ToString();
        ui_Exp.text = playerCurExp.ToString();
        ui_LV.text = playerLv.ToString();
        ui_Attack.text = playerAttackPower.ToString();

        // uI_SceneGame.Set~~()시리즈로 필요한 부분에 사용해주세용
    }

    #endregion
}
