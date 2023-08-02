using UnityEngine;
using System.Collections;
using static EnumTypes;

public abstract class MonsterBase : MonoBehaviour
{
    protected MonsterData monsterData; //=> ���߿� ��ũ���ͺ� ������Ʈ or �������Ϸ� ���� �޾ƿ� Ŭ���� ���
    protected MonsterStateType state;
    protected Player player;
    [SerializeField] protected MonsterInfo _monsterInfo = null;
    public string MonsterName;

    public bool Death { get { return curHP <= 0; } }
    protected float curHP;

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
    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        //CancelInvoke(); //invoke �Լ��� ����ϴ� ��������ּ���
      
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
            // �۵��ϰ��ִ� �ڷ�ƾ�� �ִٸ� �����Ѵ�.
            StopAllCoroutines();
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
            gameObject.transform.Translate(dirVector *_monsterInfo.MoveSpeed * Time.deltaTime);

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


    private void MonsterDeath()
    {
        // ������Ʈ Ǯ�� ��ȯ
    }
}
