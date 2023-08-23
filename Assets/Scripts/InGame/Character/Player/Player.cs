using System.Collections;
using APIModels;
using UnityEngine;

public partial class Player : CharacterBase
{
    [SerializeField] private float _spawnTime = 3f;

    #region unity event func
    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        StartCoroutine(InitializationAfterDelay());
    }

    private IEnumerator InitializationAfterDelay()
    {
        yield return new WaitForSeconds(_spawnTime);

        playerStatus = APIManager.Instance.GetValueByKey<PlayerStatus_res[]>(MasterDataDicKey.PlayerStatus.ToString());

        InitUI_Enhance();
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
        UIControl();

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
