using UnityEngine;
using System.Collections;
using static EnumTypes;

public abstract class MonsterBase : MonoBehaviour
{
    protected MonsterData monsterData; //=> 나중에 스크립터블 오브젝트 or 엑셀파일로 정보 받아온 클래스 등등
    protected MonsterStateType state;
    protected Player player;
    protected Vector3 playerTargetDirection;
    [SerializeField] protected MonsterInfo _monsterInfo = null;
    public string MonsterName;

    // 임시 Status
    private int maxHP = 10;
    public int curHP = 10;
    public int exp = 10;
    public int score = 10;


    public bool Death { get { return curHP <= 0; } }

    protected void Start()
    {
        SetMonsterName();
        if ( DataManager.Instacne.MonsterData.TryGetMonsterInfo(MonsterName, out _monsterInfo))
        {
#if UNITY_EDITOR
            Debug.Log("Insert Data");
#endif
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Not Found Data");
#endif
        }
    }

    private void OnEnable()
    {
        state = MonsterStateType.None;
        StartCoroutine("State_" + state);
    }
    protected virtual void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);

        //CancelInvoke(); //invoke 함수를 사용하는 경우적어주세요
    }

    private void OnDestroy()
    {
        if (Death == true)
        {
            // 죽었을 때의 처리
            DeathProcess();
        }
    }
    /// <summary>
    /// Set variable string ' MonsterName ' 
    /// </summary>
    protected abstract void SetMonsterName();
    protected void DeathProcess()
    {
        // 몬스터가 죽었을때 처리해줄 로직 작성
        // ex) 점수를올린다, 경험치를 올린다 등등
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
            // 작동하고있던 코루틴이 있다면 종료한다.
            StopAllCoroutines();
        }
        else
        {
            // 다음 State를 이동으로 바꾼다.
            TransferState(MonsterStateType.Move);
        }

        // 해당 Iterator블럭의 break는 TransferState에서 처리
        yield return null;
    }

    private IEnumerator State_Move()
    {
        while (state == MonsterStateType.Move)
        {
            // 플레이어가 죽으면 실행
            if (player.IsDeath)
            {
                // 몬스터들을 특정 State로 옮겨준다
                TransferState(MonsterStateType.Dance);
                yield break;
            }

            // 플레이어를 향한 방향벡터를 구함.
            Vector3 dirVector = (player.transform.position - gameObject.transform.position).normalized;
            //gameObject.transform.LookAt(player.transform.position);

            // 몬스터를 해당 방향으로 움직임(다른 방식으로 구현해도 ㅇㅋ)
            gameObject.transform.Translate(dirVector *_monsterInfo.MoveSpeed * Time.deltaTime);

            // 플레이어와 자기자신(몬스터)사이의 거리와 본인의 공격 가능범위를 비교하여 수행(다른 방식으로 구현해도 ㅇㅋ)
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= _monsterInfo.Range)
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
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= _monsterInfo.Range)
            {
                //공격
                Attack();
            }
            else
            {
                TransferState(MonsterStateType.Move);
                yield break;
            }

            yield return new WaitForSeconds(_monsterInfo.RateOfFire); // 3f 대신 monsterData.RateOfFire 등등 만들어서 대체
        }
    }

    // 죽었을때의 처리
    private IEnumerator State_Death()
    {
        // 점수+, exp+, 비활성화 되고 풀에 다시 들어가고 등등...


        MonsterDeath();
        yield return null;
    }

    /// <summary>
    /// 공격로직
    /// </summary>
    protected abstract void Attack();


    private void MonsterDeath()
    {
        // 오브젝트 풀에 반환
    }
}
