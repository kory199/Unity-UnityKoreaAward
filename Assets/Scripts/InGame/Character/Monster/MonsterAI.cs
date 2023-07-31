using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    public EnumTypes.MonsterType monsterType;
    public EnumTypes.FSMMonsterStateType fsmMonsterStateType;

    [Header("Melee Monster")]
    public float meleeMoveSpeed;
    public float meleeAttackCooldown;
    public float meleeDetectionRange; // 탐지 범위
    public float meleeAttackRange; // 공격 범위
    public int meleeAttackDamage;

    [Header("Ranged Monster")]
    public float rangedMoveSpeed;
    public float rangedAttackCooldown;
    public float rangedDetectionRange; // 탐지 범위
    public float rangedAttackRange; // 공격 범위
    public int rangedAttackDamage;

    [SerializeField] private Transform player;
    [SerializeField] private CircleCollider2D meleeDetectionCollider; // 근거리 몬스터 탐지 범위용
    [SerializeField] private CircleCollider2D rangedDetectionCollider; // 원거리 몬스터 탐지 범위용
    [SerializeField] private CircleCollider2D meleeAttackCollider; // 근거리 몬스터 공격 범위용
    [SerializeField] private CircleCollider2D rangedAttackCollider; // 원거리 몬스터 공격 범위용

    float distanceToPlayer;
    [SerializeField] private bool isAttacking = false;
    private bool isAttackingCooldown = false; // 공격 후 쿨타임 상태를 나타내는 변수
    private float attackCooldownTimer = 0f; // 공격 쿨타임 타이머


    private void Awake()
    {
        // 몬스터 상태
        fsmMonsterStateType = EnumTypes.FSMMonsterStateType.Idle;

        // 원거리
        meleeMoveSpeed = 1f;
        meleeAttackCooldown = 1.5f;
        meleeDetectionRange = 100f;
        meleeAttackRange = 2f;
        meleeAttackDamage = 10;

        // 원거리
        rangedMoveSpeed = 0.5f;
        rangedAttackCooldown = 2f;
        rangedDetectionRange = 100f;
        rangedAttackRange = 15f;
        rangedAttackDamage = 5;

        // Player와의 거리 계산
        distanceToPlayer = Mathf.Infinity;
    }
    private void Start()
    {
        if (monsterType == EnumTypes.MonsterType.MeleeMonster)
        {
            // melee monster 탐지 범위용 SphereCollider
            meleeDetectionCollider = gameObject.AddComponent<CircleCollider2D>();
            meleeDetectionCollider.isTrigger = true;
            meleeDetectionCollider.radius = meleeDetectionRange;

            // melee monster 공격 범위용 SphereCollider
            meleeAttackCollider = gameObject.AddComponent<CircleCollider2D>();
            meleeAttackCollider.isTrigger = true;
            meleeAttackCollider.radius = meleeAttackRange;
        }
        else if (monsterType == EnumTypes.MonsterType.RangedMonster)
        {
            // ranged monster 탐지 범위용 SphereCollider
            rangedDetectionCollider = gameObject.AddComponent<CircleCollider2D>();
            rangedDetectionCollider.isTrigger = true;
            rangedDetectionCollider.radius = rangedDetectionRange;

            // ranged monster 공격 범위용 SphereCollider
            rangedAttackCollider = gameObject.AddComponent<CircleCollider2D>();
            rangedAttackCollider.isTrigger = true;
            rangedAttackCollider.radius = rangedAttackRange;
        }
    }

    private void Update()
    {
        switch (fsmMonsterStateType)
        {
            case EnumTypes.FSMMonsterStateType.Idle:
                UpdateIdleState();
                break;
            case EnumTypes.FSMMonsterStateType.Chasing:
                UpdateChasingState();
                break;
            case EnumTypes.FSMMonsterStateType.Attacking:
                UpdateAttackingState();
                break;
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        transform.Translate(direction * GetMoveSpeed() * Time.deltaTime);
    }

    private void UpdateIdleState()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= GetDetectionRange())
        {
            fsmMonsterStateType = EnumTypes.FSMMonsterStateType.Chasing;
        }
    }

    private void UpdateChasingState()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= GetAttackRange())
        {
            fsmMonsterStateType = EnumTypes.FSMMonsterStateType.Attacking;
        }
        else if (distanceToPlayer > GetDetectionRange())
        {
            fsmMonsterStateType = EnumTypes.FSMMonsterStateType.Idle;
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void UpdateAttackingState()
    {
        distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (isAttackingCooldown)
        {
            // 쿨타임 중이면 공격하지 않고 쿨타임 타이머를 감소시킴
            attackCooldownTimer -= Time.deltaTime;

            if (attackCooldownTimer <= 0f)
            {
                // 쿨타임이 끝나면 다시 공격 가능 상태로 전환
                isAttackingCooldown = false;
            }
        }
        else
        {
            // 쿨타임이 아닌 상태에서만 공격 가능
            if (distanceToPlayer > GetAttackRange())
            {
                fsmMonsterStateType = EnumTypes.FSMMonsterStateType.Chasing;
            }
            else
            {
                Attack();
            }
        }
    }

    private float GetMoveSpeed()
    {
        return monsterType == EnumTypes.MonsterType.MeleeMonster ? meleeMoveSpeed : rangedMoveSpeed;
    }

    private float GetDetectionRange()
    {
        return monsterType == EnumTypes.MonsterType.MeleeMonster ? meleeDetectionRange : rangedDetectionRange;
    }

    private float GetAttackRange()
    {
        return monsterType == EnumTypes.MonsterType.MeleeMonster ? meleeAttackCollider.radius : rangedAttackCollider.radius;
    }

    private float GetAttackCooldown()
    {
        return monsterType == EnumTypes.MonsterType.MeleeMonster ? meleeAttackCooldown : rangedAttackCooldown;
    }

    private void Attack()
    {
        if (!isAttackingCooldown)
        {
            if (monsterType == EnumTypes.MonsterType.MeleeMonster)
            {
                // 근거리 몬스터가 공격
                meleeAttackCollider.enabled = true;
            }
            else if (monsterType == EnumTypes.MonsterType.RangedMonster)
            {
                // 원거리 몬스터가 공격
                rangedAttackCollider.enabled = true;
            }

            isAttackingCooldown = true;
            attackCooldownTimer = GetAttackCooldown(); // 쿨타임 설정
        }
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 공격 범위에 플레이어가 들어오면 공격
        if (!isAttacking && distanceToPlayer < GetAttackRange() && other.CompareTag("Player"))
        {
            if (monsterType == EnumTypes.MonsterType.MeleeMonster)
            {
                Debug.Log("Melee Monster Attack");

                other.GetComponent<Player>().PlayerHit(meleeAttackDamage);
            }
            else if (monsterType == EnumTypes.MonsterType.RangedMonster)
            {
                other.GetComponent<Player>().PlayerHit(rangedAttackDamage);
            }

            isAttacking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 공격 범위에서 플레이어가 나가면 다시 콜라이더 비활성화
        if (isAttacking && other.CompareTag("Player"))
        {
            if (monsterType == EnumTypes.MonsterType.MeleeMonster)
            {
                meleeAttackCollider.enabled = false;
            }
            else
            {
                rangedAttackCollider.enabled = false;
            }

            // 공격 종료 시 
            isAttacking = false;
        }
    }
}
