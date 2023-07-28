using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float bulletSpeed;
    [SerializeField] GameObject player;
    Transform bulletSpawner;
    Rigidbody2D bulletRb; 
    float lifeTime = 5f;
    Vector2 dirBullet;

    private void BulletInit()
    {
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
            bulletRb = rb;
        else
            bulletRb = gameObject.AddComponent<Rigidbody2D>(); 

        bulletRb.gravityScale = 0;

        bulletSpawner = player.transform;

        bulletSpeed = 10f; 

        // 마우스 방향에 따른 방향 벡터 계산
        DirBullet();
    }

    private void DirBullet()
    {
        // 게임 뷰의 마우스 포인트
        Vector3 mousePosition = Input.mousePosition;
        // 마우스 좌표를 월드좌표 기준으로 변환
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector2 bulletSpawnerPosition = bulletSpawner.position;

        // 방향벡터 구하기 (2D 평면상의 방향을 구하기 위해 Z좌표를 무시)
        dirBullet = ((Vector2)mouseWorldPosition - bulletSpawnerPosition).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        // 몬스터 충돌 체크 & 충돌 시 동작
        if (other.CompareTag("Monster")) 
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
