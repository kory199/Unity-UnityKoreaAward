using UnityEngine;
using System.Collections;
using static EnumTypes;
using APIModels;

public abstract class MonsterBase : MonoBehaviour
{
    protected MonsterData monsterData; //=> ���߿� ��ũ���ͺ� ������Ʈ or �������Ϸ� ���� �޾ƿ� Ŭ���� ���
    [SerializeField] protected MonsterStateType state;
    protected Player player;
    protected Vector3 playerTargetDirection;
    [SerializeField] protected MonsterInfo _monsterInfo = null;
    public string MonsterName;

    int stageNum;

    // ���� �ʿ�
    protected MonsterData_res[] meleeMonsterStatus;
    protected MonsterData_res[] rangedMonsterStatus;
    protected MonsterData_res[] monsterData_Res;

    // �ӽ� Status
    private int maxHP = 10;
    public int curHP = 10;
    public int exp = 10;
    public int score = 10;
    protected bool Death { get { return curHP <= 0; } }


    protected virtual void OnEnable()
    {
        state = MonsterStateType.None;
        StartCoroutine("State_" + state);
    }

    protected virtual void Start()
    {
        // Start Chain
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, GetMeleeMonsterInfo);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, GetRangedMonsterInfo);

        // Next Chain
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);
        stageNum = 1;

        // data manager ���� �� stage ���濡 ���� monster status �������� onEnable�� monster setting ���� �ʿ�
        SetMonsterName();

        initMonsterStatus();

        /*if (DataManager.Instacne.MonsterData.TryGetMonsterInfo(MonsterName, out _monsterInfo))
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
        }*/
    }

    protected virtual void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);

        // CancelInvoke(); //invoke �Լ��� ����ϴ� ��������ּ���
    }

    private void OnDestroy()
    {
        if (Death == true)
        {
            // �׾��� ���� ó��
            DeathProcess();
        }
    }
    /// <summary>
    /// Set variable string ' MonsterName ' 
    /// </summary>
    protected abstract void SetMonsterName();

    protected virtual async void initMonsterStatus()
    {
        bool result = await APIManager.Instance.GetMasterDataAPI();

        if(result)
        {
            meleeMonsterStatus = APIManager.Instance.GetValueByKey<MonsterData_res[]>(MasterDataDicKey.MeleeMonster.ToString());
            rangedMonsterStatus = APIManager.Instance.GetValueByKey<MonsterData_res[]>(MasterDataDicKey.RangedMonster.ToString());
        }

        //meleeMonsterStatus = APIDataSO.Instance.GetValueByKey<MonsterData_res[]>(APIDataDicKey.MeleeMonster);
        //rangedMonsterStatus = APIDataSO.Instance.GetValueByKey<MonsterData_res[]>(APIDataDicKey.RangedMonster);
    }

    public void MonsterHit(int damage)
    {
        curHP -= damage;
        if (curHP <= 0)
        {
            MonsterDeath();
        }
    }
    protected void DeathProcess()
    {
        // ���Ͱ� �׾����� ó������ ���� �ۼ�
        // ex) �������ø���, ����ġ�� �ø��� ���
    }

    // String���·� �ڷ�ƾ �Լ��� ã���Ƿ� ��Ÿ�����ʰ� ����
    // ex) NONE -> X,  None -> O
    protected void TransferState(MonsterStateType Nextstate)
    {
        // ���� State�� �ڷ�ƾ�� ������Ű��
        StopCoroutine("State_" + state);
        // State�� �������ص�
        state = Nextstate;
        // �ش� State�� �ڷ�ƾ�� �����Ų��.
        StartCoroutine("State_" + state);
    }

    // Ȱ��ȭ(ó�� ���� or ������ƮǮ���� �ٽ� ��ȯ�Ǿ� ���ö�)
    private IEnumerator State_None()
    {
        // �÷��̾� ������ ���ٸ�
        if (player == null)
        {
            // �÷��̾ ã�Ƶΰ�
            player = FindObjectOfType<Player>();

            Debug.Log(player == null);
            // �۵��ϰ��ִ� �ڷ�ƾ�� �ִٸ� �����Ѵ�.
            StopAllCoroutines();
            //�̵���Ŵ
        }
        else
        {
            // ���� State�� �̵����� �ٲ۴�.
            TransferState(MonsterStateType.Move);
        }

        // �ش� Iterator������ break�� TransferState���� ó��
        yield return null;
    }

    private IEnumerator State_Move()
    {
        while (state == MonsterStateType.Move)
        {
            // �÷��̾ ������ ����
            if (player.IsDeath)
            {
                // ���͵��� Ư�� State�� �Ű��ش�
                TransferState(MonsterStateType.Dance);
                yield break;
            }

            // �÷��̾ ���� ���⺤�͸� ����.
            Vector3 dirVector = (player.transform.position - gameObject.transform.position).normalized;
            //gameObject.transform.LookAt(player.transform.position);

            // ���͸� �ش� �������� ������(�ٸ� ������� �����ص� ����)
            gameObject.transform.Translate(dirVector * _monsterInfo.MoveSpeed * Time.deltaTime);

            // �÷��̾�� �ڱ��ڽ�(����)������ �Ÿ��� ������ ���� ���ɹ����� ���Ͽ� ����(�ٸ� ������� �����ص� ����)
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
        // ���� ������ �����϶� ���ѹݺ�
        while (state == MonsterStateType.Attack)
        {
            // ���ݰ��ɻ������� üũ
            // �������̸� Move State�� �̵� 
            if (Vector3.Distance(player.transform.position, this.gameObject.transform.position) <= _monsterInfo.Range)
            {
                //����
                Attack();
            }
            else
            {
                TransferState(MonsterStateType.Move);
                yield break;
            }

            yield return new WaitForSeconds(_monsterInfo.RateOfFire); // 3f ��� monsterData.RateOfFire ��� ���� ��ü
        }
    }

    // �׾������� ó��
    private IEnumerator State_Death()
    {
        // ����+, exp+, ��Ȱ��ȭ �ǰ� Ǯ�� �ٽ� ���� ���...


        MonsterDeath();
        yield return null;
    }

    /// <summary>
    /// ���ݷ���
    /// </summary>
    protected abstract void Attack();


    protected virtual void MonsterDeath()
    {
        // ������Ʈ Ǯ�� ��ȯ
        StageManager.Instance.MonsterDeath();
    }

    private void GetMeleeMonsterInfo() => MonsterSetting(stageNum, EnumTypes.MonsterType.MeleeMonster);
    private void GetRangedMonsterInfo() => MonsterSetting(stageNum, EnumTypes.MonsterType.RangedMonster);

    private void MonsterSetting(int stageNum, EnumTypes.MonsterType monsterType)
    {
        monsterData_Res = APIManager.Instance.GetValueByKey<MonsterData_res[]>(monsterType.ToString());

        if (monsterData_Res == null)
        {
            Debug.LogError("No MonsterData_res data");
            return;
        }

        MonsterSetting(monsterType);
    }

    private void MonsterSetting(EnumTypes.MonsterType monsterType)
    {

    }

    private void SetStageNum()
    {
        // ���� Max Stage 5 ����
        if (stageNum > 5)
        {
            InGameManager.Instance.InvokeCallBacks(InGameParamType.Stage, (int)EnumTypes.StageStateType.End);
            return;
        }

        stageNum++;
    }
}
