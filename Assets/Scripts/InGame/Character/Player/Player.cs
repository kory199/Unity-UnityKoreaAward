using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using APIModels;
using TMPro;

public partial class Player : CharacterBase
{
    
    // UI Test Component
    [SerializeField] TextMeshProUGUI ui_Hp;
    [SerializeField] TextMeshProUGUI ui_Exp;
    [SerializeField] TextMeshProUGUI ui_Stage;
    [SerializeField] TextMeshProUGUI ui_LV;


    #region unity event func
    private void Awake()
    {
        InitSetting();
        InitComponent();
        InitPlayer();
    }

    protected override void Start()
    {
        // ��Ÿ �ʱ�ȭ �׸� �߰�
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

        // temp ui
        ui_Hp.text = playerCurHp.ToString();
        ui_Exp.text = playerCurExp.ToString();
        ui_Stage.text = StageManager.Instance.GetStageNum().ToString();
        ui_LV.text = playerLv.ToString();
    }

    #endregion
}
