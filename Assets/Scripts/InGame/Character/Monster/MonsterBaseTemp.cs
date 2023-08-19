using UnityEngine;
using System.Collections;
using static EnumTypes;
using APIModels;

public abstract class MonsterBase : MonoBehaviour
{
    [SerializeField] protected MonsterStateType state;
    [SerializeField] protected Player player;
    [SerializeField] protected MonsterInfo _monsterInfo = null;
    protected Vector3 playerTargetDirection;
    protected int stageNum;

    public string MonsterName;

    [SerializeField] protected bool isFirst;

    // 편집 필요
    protected MonsterData_res[] rangedMonsterStatus;
    protected MonsterData_res bossMonsterStatus;
    protected MonsterData_res[] monsterData_Res;

    // protected bool Death { get { return curHP <= 0; } }

    protected virtual void Awake()
    {
        gameObject.tag = "Monster";
        _monsterInfo = new MonsterInfo();
        GetInitMonsterStatus();
        //체인 등록
        InGameManager.Instance.RegisterParams(EnumTypes.InGameParamType.Monster, (int)EnumTypes.StageStateType.Max);

        stageNum = 0;
        isFirst = true;
    }

    protected virtual void OnEnable()
    {
        // Debug.LogError("OnEnable : " + isFirst);
        if (isFirst == false)
        {
            stageNum = StageManager.Instance.GetStageNum();
            //Debug.LogError("stageNum : " + stageNum);
        }

        state = MonsterStateType.None;
        StartCoroutine("State_" + state);
    }

    protected virtual void Start()
    {
        // Next Chain
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Monster, EnumTypes.StageStateType.Next, MonsterStatusUpdate);

        CallMonster(EnumTypes.StageStateType.Awake);

        // data manager 삭제 및 stage 변경에 따른 monster status 변경으로 onEnable로 monster setting 변경 필요
        SetMonsterName();
    }

    protected virtual void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        isFirst = false;
        // CancelInvoke(); //invoke 함수를 사용하는 경우적어주세요
    }
    /// <summary>
    /// Set variable string ' MonsterName ' 
    /// </summary>
    protected abstract void SetMonsterName();

    protected virtual void GetInitMonsterStatus()
    {
        rangedMonsterStatus = APIManager.Instance.GetValueByKey<MonsterData_res[]>(MasterDataDicKey.RangedMonster.ToString());
        bossMonsterStatus = APIManager.Instance.GetValueByKey<MonsterData_res>(MasterDataDicKey.BOSS.ToString());

        if (rangedMonsterStatus == null)
        {
            Debug.LogError("rangedMonsterStatus Data is null");
        }

        if (bossMonsterStatus == null)
        {
            Debug.LogError("bossMonsterStatus Data is Null");
        }
    }

    protected void GetMeleeMonsterInfo() => MonsterSetting(stageNum, EnumTypes.MonsterType.MeleeMonster);
    protected void GetRangedMonsterInfo() => MonsterSetting(stageNum, EnumTypes.MonsterType.RangedMonster);

    private void MonsterSetting(int curStageNum, EnumTypes.MonsterType monsterType)
    {
        Debug.LogError("MonsterSetting Call");
        monsterData_Res = APIManager.Instance.GetValueByKey<MonsterData_res[]>(monsterType.ToString());

        if (monsterData_Res == null)
        {
            Debug.LogError("No MonsterData_res data");
            return;
        }

        MonsterStatusSetting(monsterType);
    }

    protected void MonsterStatusSetting(EnumTypes.MonsterType monsterType)
    {
         if (monsterType == EnumTypes.MonsterType.RangedMonster)
        {
            rangedMonsterStatus = APIManager.Instance.GetValueByKey<MonsterData_res[]>(MasterDataDicKey.RangedMonster.ToString());
        }
        else
        {
            Debug.LogError("Confirm Monster Type");
        }
    }

    private void SetStageNum()
    {
        Debug.LogError("SetStageNum : " + stageNum);
        // 현재 Max Stage 5 기준
        if (stageNum > 5)
        {
            return;
        }

        stageNum++;

        CallMonster(EnumTypes.StageStateType.Next);
    }

    protected virtual void MonsterStatusUpdate()
    {

    }

    private void CallMonster(EnumTypes.StageStateType stageType)
    {
        InGameManager.Instance.InvokeCallBacks(EnumTypes.InGameParamType.Monster, (int)stageType);
    }

    // String형태로 코루틴 함수를 찾으므로 오타나지않게 주의
    // ex) NONE -> X,  None -> O
    protected void TransferState(MonsterStateType Nextstate)
    {
        // 현재 State의 코루틴을 중지시키고
        StopCoroutine("State_" + state);
        // State를 변경해준뒤
        state = Nextstate;
        // 해당 State의 코루틴을 실행시킨다.
        StartCoroutine("State_" + state);
    }

    // 활성화(처음 생성 or 오브젝트풀에서 다시 반환되어 나올때)
    private IEnumerator State_None()
    {
        // 플레이어 정보가 없다면
        if (player == null)
        {
            // 플레이어를 찾아두고
            player = FindObjectOfType<Player>();

            Debug.Log(player == null);
            // 작동하고있던 코루틴이 있다면 종료한다.
            StopAllCoroutines();
            //이동시킴
        }
        else
        {
            // 다음 State를 이동으로 바꾼다.
            TransferState(MonsterStateType.Move);
        }

        // 해당 Iterator블럭의 break는 TransferState에서 처리
        yield return null;
    }

    protected virtual IEnumerator State_Move()
    {
        while (state == MonsterStateType.Move)
        {
            // 플레이어가 죽으면 실행
            if (player.IsDeath)
            {
                // 몬스터들을 특정 State로 옮겨준다
                // TransferState(MonsterStateType.Dance);
                yield break;
            }

            // 플레이어를 향한 방향벡터를 구함.
            Vector3 dirVector = (player.transform.position - gameObject.transform.position).normalized;
            //gameObject.transform.LookAt(player.transform.position);

            // 몬스터를 해당 방향으로 움직임(다른 방식으로 구현해도 ㅇㅋ)
            // gameObject.transform.Translate(dirVector * _monsterInfo.MoveSpeed * Time.deltaTime);
            // 임시 이동속도
            gameObject.transform.Translate(dirVector * 2f * Time.deltaTime);

            // 플레이어와 자기자신(몬스터)사이의 거리와 본인의 공격 가능범위를 비교하여 수행(다른 방식으로 구현해도 ㅇㅋ)
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= _monsterInfo.ranged)
            {
                TransferState(MonsterStateType.Attack);
                yield break;
            }

            yield return null;
        }
    }

    protected virtual IEnumerator State_Attack()
    {
        // 공격 가능한 상태일때 무한반복
        while (state == MonsterStateType.Attack)
        {
            // 공격가능상태인지 체크
            // 범위밖이면 Move State로 이동 
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= _monsterInfo.ranged)
            {
                //공격
                Attack();
            }
            else
            {
                TransferState(MonsterStateType.Move);
                yield break;
            }

            yield return new WaitForSeconds(_monsterInfo.rate_of_fire); // 3f 대신 monsterData.RateOfFire 등등 만들어서 대체
        }
    }

    // 죽었을때의 처리
    protected virtual IEnumerator State_Death()
    {
        // 점수+, exp+, 비활성화 되고 풀에 다시 들어가고 등등...

        MonsterDeath();
        yield return null;
    }

    /// <summary>
    /// 공격로직
    /// </summary>
    public abstract void Attack();

    public abstract void Hit();

    protected virtual void MonsterDeath()
    {
        gameObject.SetActive(false);
        StageManager.Instance.MonsterDeath();
    }
}
