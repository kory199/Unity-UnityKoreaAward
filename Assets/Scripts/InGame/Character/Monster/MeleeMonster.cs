using System.Collections;
using System.Collections.Generic;
using APIModels;
using UnityEngine;
using static EnumTypes;

public class MeleeMonster : MonsterBase
{
    private MonsterData_res[] meleeMonsterStatus;

    [SerializeField] private bool isMeleeMonsterDead;

    WaitForSeconds waitForAttackSFX;
    WaitForSeconds waitForHitSFX;

    private List<Sprite> monsterSprite = new();
    private SpriteRenderer meleeMonsterSpriteRenderer;
    private float rotationSpeed;

    #region unity event func
    protected override void Awake()
    {
        base.Awake();
        GetInitMonsterStatus();
        InitMeleeMonsterInfo();

        meleeMonsterSpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        isMeleeMonsterDead = false;
        rotationSpeed = 25f;
    }

    // stage 변경에 따른 Level별 능력치 부여 => 서버 정보 받아오기
    protected override void OnEnable()
    {
        meleeMonsterSpriteRenderer.sprite = SetMonsterSprite();

        base.OnEnable();
        isSelfDestruct = false;

        if (isMeleeMonsterDead == true)
        {
            SetMeleeMonsterStatus(stageNum);
        }
    }

    protected override void Start()
    {
        waitForAttackSFX = new WaitForSeconds(2f);
        waitForHitSFX = new WaitForSeconds(0.5f);

        base.Start();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isMeleeMonsterDead = true;
    }
    #endregion

    protected override void GetInitMonsterStatus()
    {
        meleeMonsterStatus = APIManager.Instance.GetValueByKey<MonsterData_res[]>(MasterDataDicKey.MeleeMonster.ToString());

        if (meleeMonsterStatus == null)
        {
            Debug.LogError("meleeMonsterStatus Data is Null");
        }
    }

    protected override void SetMonsterName()
    {
        MonsterName = "BasicMeleeMonster";
    }

    protected void SetMeleeMonsterStatus(int inputStageNum)
    {
        _monsterInfo.level = meleeMonsterStatus[inputStageNum].level;
        _monsterInfo.exp = meleeMonsterStatus[inputStageNum].exp;
        _monsterInfo.hp = meleeMonsterStatus[inputStageNum].hp;
        _monsterInfo.curHp = _monsterInfo.hp;
        _monsterInfo.speed = meleeMonsterStatus[inputStageNum].speed;
        _monsterInfo.rate_of_fire = meleeMonsterStatus[inputStageNum].rate_of_fire;
        _monsterInfo.projectile_speed = meleeMonsterStatus[inputStageNum].projectile_speed;
        _monsterInfo.collision_damage = meleeMonsterStatus[inputStageNum].collision_damage;
        _monsterInfo.score = meleeMonsterStatus[inputStageNum].score;
        _monsterInfo.ranged = meleeMonsterStatus[inputStageNum].ranged;
    }

    protected override void MonsterStatusUpdate()
    {
        //  Debug.LogError("MonsterStatusUpdate : " + stageNum);
        SetMeleeMonsterStatus(stageNum);
    }

    public override void Attack()
    {
        isSelfDestruct = true;

        monsterSFX.MonsterStateSFX(gameObject.transform, EnumTypes.MonsterStateType.Attack);

        PlayerHit();
        MonsterDeath(); // 자폭에 의한 공격은 보상 X
    }

    public override void Hit()
    {
        _monsterInfo.curHp -= player.playerAttackPower;
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.MonsterHit);
        monsterSFX.MonsterStateSFX(gameObject.transform, EnumTypes.MonsterStateType.Hit);

        if (_monsterInfo.curHp <= 0)
        {
            monsterSFX.MonsterStateSFX(gameObject.transform, EnumTypes.MonsterStateType.Death);
            player.Reward(_monsterInfo.exp, _monsterInfo.score);
            MonsterDeath();
        }
    }

    public void PlayerHit()
    {
        player.PlayerHit(_monsterInfo.collision_damage);
    }

    protected override IEnumerator State_Move()
    {
        StartCoroutine(MonsterRotate());
        // 추후 몬스터 별 이동속도 및 공격 범위 추가
        return base.State_Move();
    }

    public void HitNerfShot(float damageReduction)
    {
        Debug.LogError("1. collision_damage : " + _monsterInfo.collision_damage);
        _monsterInfo.collision_damage = _monsterInfo.collision_damage * damageReduction;
        Debug.LogError("2. collision_damage : " + _monsterInfo.collision_damage);
    }

    private void InitMeleeMonsterInfo()
    {
        Sprite[] sprites = Resources.LoadAll<Sprite>("Monsters");

        foreach (Sprite sprite in sprites)
        {
            monsterSprite.Add(sprite);
        }
    }

    private Sprite SetMonsterSprite()
    {
        int randomNum = Random.Range(0, monsterSprite.Count);

        return monsterSprite[randomNum];
    }

    private IEnumerator MonsterRotate()
    {
        while (true)
        {
            // 회전 각도 계산 (프레임 속도에 비례하여 회전)
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // 오브젝트를 회전시킴
            meleeMonsterSpriteRenderer.gameObject.transform.Rotate(Vector3.back, rotationAmount);

            yield return null;
        }
    }

    public void HitShield()
    {
        monsterSFX.MonsterStateSFX(gameObject.transform, EnumTypes.MonsterStateType.Attack);
        MonsterDeath();
    }
}