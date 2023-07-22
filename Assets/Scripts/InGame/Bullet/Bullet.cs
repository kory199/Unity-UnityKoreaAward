using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject player;
    Transform bulletSpawner;
    Rigidbody bulletRb;
    short lifeTime = 5;
    Vector3 dirBullet;

    private void Awake()
    {
    }

    void Start()
    {
    }

    void Update()
    {
        
    }

    private void BulletInit()
    {
        if (gameObject.TryGetComponent<Rigidbody>(out Rigidbody rb))
            bulletRb = rb;
        else
            bulletRb = gameObject.AddComponent<Rigidbody>();

        bulletSpawner = player.transform.GetChild(0);

        bulletSpeed = 500f;

        // 마우스 방향에 따른 방향 벡터 계산
        DirBullet();
    }

    private void DirBullet()
    {
        // 게임 뷰의 마우스 포인트
        Vector3 mousePosition = Input.mousePosition;
        // 마우스 좌표를 월드좌표 기준으로 변환
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            // 방향벡터 구하기
            dirBullet = (hit.point - bulletSpawner.position).normalized;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 몬스터 충돌 체크 & 충돌 시 동작
        if (other.tag == "Monster")
        {
            // 몬스터와 충돌 시 Bullet 반환
            ReturnBullet();
        }
    }

    private void ReturnBullet() => gameObject.SetActive(false);

    private void OnEnable()
    {
        BulletInit();

        bulletRb.velocity = dirBullet * bulletSpeed;

        // 발사 후 5초 뒤 Bullet 비활성화
        Invoke("ReturnBullet", lifeTime);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
