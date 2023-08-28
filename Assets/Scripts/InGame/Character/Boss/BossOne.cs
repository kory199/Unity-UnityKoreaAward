using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumTypes;

public class BossOne : BossBase
{
    [SerializeField] private float _delayTime = 2f;
    [SerializeField] private List<float> _rad;
    [SerializeField] private List<float> _initDegree;
    [SerializeField] private float lotationSpeed = 0.01f;
    [SerializeField] private float lotationSpeed2 = 0.1f;
    [Range(0, 100)]
    [SerializeField] private float tempHP = 100;
    [Header("BossProjectile")]
    [SerializeField] private string projectileName;
    protected override void Start()
    {
        //SetRangedMonsterStatus(stageNum);
        SetBossMonsterStatus(1); //보스 테스트
        //회전 초기값
        foreach (var pos in _spawners)
        {
            Vector2 tempVector = _boss.transform.position - pos.position;
            _rad.Add(Vector3.Distance(pos.transform.position, _boss.transform.position));
            _initDegree.Add(Mathf.Atan2(tempVector.x, tempVector.y));
        }
        _monsterInfo.rate_of_fire = 1f;
        _monsterInfo.ranged = 1f;
        TransferState(MonsterStateType.Move);
        TransferState(MonsterStateType.Move);
        // StartCoroutine(Co_BossPattern());
    }

    IEnumerator Co_BossPattern()
    {
        yield return new WaitForSeconds(_delayTime);

        TransferState(MonsterStateType.Phase1);
        yield return new WaitUntil(() => _monsterInfo.curHp < _monsterInfo.hp * 0.6f);

        TransferState(MonsterStateType.Phase2);
        yield return new WaitUntil(() => _monsterInfo.curHp < _monsterInfo.hp * 0.3f);

        TransferState(MonsterStateType.Phase3);
        yield return new WaitUntil(() => _monsterInfo.curHp == 0);

        StartCoroutine(Co_BossDie());
    }

    public void GetDamage(int damage)
    {
        // curHP : 서버에서 받아오깅
        // curHP -= damage;
    }
    public override void Attack()
    {
        ShootProjectile(projectileName);
    }
    private void RotateAttack()
    {
        for (int i = 0; i < _spawners.Count; i++)
        {
            StartCoroutine(Co_halfMoveSpawner(_spawners[i].gameObject, _initDegree[i], _rad[i]));
        }
    }
    IEnumerator Co_halfMoveSpawner(GameObject spawner, float initDegree, float rad)
    {
        float i = 0;
        float transedDegree = initDegree;
        while (true)
        {
            if (i > 360)
            {
                i = 0;
                transedDegree -= 360;
            }
            yield return null;
            Vector3 newPos = Vector3.zero;
            newPos.x = _boss.transform.position.x + rad * Mathf.Cos(transedDegree);
            newPos.y = _boss.transform.position.y + rad * Mathf.Sin(transedDegree);

            spawner.transform.position = newPos;
            transedDegree += lotationSpeed;
            i += lotationSpeed;
        }
    }
    protected override IEnumerator State_Attack()
    {
        // 공격 가능한 상태일때 무한반복
        while (state == MonsterStateType.Attack)
        {
            // 공격가능상태인지 체크
            // 범위밖이면 Move State로 이동 
            Attack();
            yield return new WaitForSeconds(_monsterInfo.rate_of_fire); // 3f ��� monsterData.RateOfFire ��� ���� ��ü
        }
    }
    protected override IEnumerator State_Move()
    {
        // 추후 몬스터 별 이동속도 및 공격 범위 추가
        while (state == MonsterStateType.Move)
        {
            // 추후 몬스터 별 이동속도 및 공격 범위 추가
            if (player.IsDeath)
            {
                // 몬스터들을 특정 State로 옮겨준다
                // TransferState(MonsterStateType.Dance);
                yield break;
            }

            // 플레이어를 향한 방향벡터를 구함.
            //Vector3 dirVector = (player.transform.position - gameObject.transform.position).normalized;
            Vector3 dirVector = (Vector3.zero-gameObject.transform.position ).normalized;
            //gameObject.transform.LookAt(player.transform.position);

            // 임시 이동속도
            gameObject.transform.Translate(dirVector * 2f * Time.deltaTime);

            // 플레이어와 자기자신(몬스터)사이의 거리와 본인의 공격 가능범위를 비교하여 수행(다른 방식으로 구현해도 ㅇㅋ)
            //if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= _monsterInfo.ranged)
            if (Vector3.Distance(Vector3.zero, this.gameObject.transform.position) <= _monsterInfo.ranged)
            {
                StartCoroutine(Co_BossPattern());
                yield break;
            }

            yield return null;
        }
    }
    IEnumerator Co_SpanwerSpin()
    {
        int i = 0;
        while (state == MonsterStateType.Phase1)
        {
            _spawners[i].transform.Rotate(new Vector3(0, 0, 1f));
            i++;
            if (i > 3) i = 0;
            yield return null;
        }
    }
    IEnumerator State_Phase1()
    {
        Debug.Log("페이즈 1");

        RotateAttack();
        // 공격 가능한 상태일때 무한반복
        while (state == MonsterStateType.Phase1)
        {
            Attack();
            yield return new WaitForSeconds(_monsterInfo.rate_of_fire);
        }
    }
    IEnumerator State_Phase2()
    {
        Debug.Log("페이즈 2");
        RotateAttack();
        // 공격 가능한 상태일때 무한반복
        while (state == MonsterStateType.Phase2)
        {
            Attack();
            yield return new WaitForSeconds(_monsterInfo.rate_of_fire - 0.5f);
        }
    }
    IEnumerator State_Phase3()
    {
        Debug.Log("������ 3");
        int i = 10;
        StartCoroutine(Co_SpanwerSpin());
        SpreadOutLine outline= gameObject.AddComponent<SpreadOutLine>();
        outline.MyColor = new Color(0, 255, 255, 1);
        // 공격 가능한 상태일때 무한반복
        while (state == MonsterStateType.Phase3)
        {
            Attack();
            yield return new WaitForSeconds(Mathf.Sin(Mathf.Deg2Rad * i + 0.15f));
            // Debug.Log(Mathf.Sin(Mathf.Deg2Rad * i));
            i--;
            if (i < 0) i = 10;
        }
    }

    protected override void SetMonsterName()
    {
        MonsterName = "BossOne";
    }
    IEnumerator Co_BossDie()
    {
        yield return null;
        //죽을 경우 나타나는 이펙트 알림 모두 처리
        Debug.Log("boss die");
        //스테이지 매니저에 보스 죽음알림
        FindObjectOfType<StageManager>().BossDeath();

    }
    bool _stageOver = false;
    public override void Hit()
    {
        BossHit();
    }
    public  void BossHit(float multiple = 1)
    {
        _monsterInfo.curHp -= player.playerAttackPower * multiple;
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.MonsterHit);
        monsterSFX.MonsterStateSFX(gameObject.transform, EnumTypes.MonsterStateType.Hit);
        if (_monsterInfo.curHp <= 0)
        {
            if (_stageOver == true) return;
            _stageOver = true;
            gameObject.SetActive(false);
            monsterSFX.MonsterStateSFX(gameObject.transform, EnumTypes.MonsterStateType.Death);
            StageManager.Instance.StageClear();
            //player.Reward(_monsterInfo.exp);
        }
    }

    private void SetBossMonsterStatus(int inputStageNum)
    {
        _monsterInfo.level = bossMonsterStatus.level;
        _monsterInfo.exp = bossMonsterStatus.exp;
        _monsterInfo.hp = bossMonsterStatus.hp;
        _monsterInfo.curHp = bossMonsterStatus.hp;
        _monsterInfo.speed = bossMonsterStatus.speed;
        _monsterInfo.rate_of_fire = bossMonsterStatus.rate_of_fire;
        _monsterInfo.projectile_speed = bossMonsterStatus.projectile_speed;
        _monsterInfo.collision_damage = bossMonsterStatus.collision_damage;
        _monsterInfo.score = bossMonsterStatus.score;
        _monsterInfo.ranged = bossMonsterStatus.ranged;
        _monsterInfo.collision_damage = bossMonsterStatus.collision_damage;
    }

}
