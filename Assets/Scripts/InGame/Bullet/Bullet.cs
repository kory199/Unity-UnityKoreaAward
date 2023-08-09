using UnityEngine;
using static EnumTypes;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletLifeTime;
    [SerializeField] public float rangedBulletDamage;
    [SerializeField] public float meleeBulletDamage;
    [SerializeField] public float playerBulletDamage;

    [SerializeField] GameObject playerObject;
    [SerializeField] Player player;
    [SerializeField] GameObject bossMonster;
    [SerializeField] MeleeMonster meleeMonster;
    [SerializeField] RangedMonster rangedMonster;
    [SerializeField] GameObject setShooter;

    Vector2 dirBullet;

    private Transform bulletSpawner;
    private CircleCollider2D bulletCollider;
    private Rigidbody2D bulletRb;

    private void Start()
    {
        BulletInit();
    }

    private void BulletInit()
    {
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            bulletRb = rb;
        }
        else
        {
            bulletRb = gameObject.AddComponent<Rigidbody2D>();
        }

        if (gameObject.TryGetComponent<CircleCollider2D>(out CircleCollider2D collider2D))
        {
            bulletCollider = collider2D;
        }
        else
        {
            bulletCollider = gameObject.AddComponent<CircleCollider2D>();
        }

        bulletRb.gravityScale = 0;
        bulletCollider.isTrigger = true;

        bulletSpawner = playerObject.transform;

        if (playerObject.TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }
        else
        {
            player = playerObject.AddComponent<Player>();
        }

        meleeMonster = GetComponent<MeleeMonster>();
        rangedMonster = GetComponent<RangedMonster>();

        // 레벨업 등에 따라 바뀜 (초기 값으로 추후 스크립터블 오브젝트에서 값을 받아와야됨)
        bulletSpeed = 10f;
        bulletLifeTime = 5f;
        rangedBulletDamage = 10f;
        meleeBulletDamage = 20f;
        playerBulletDamage = 10f;
    }


    public void SetShooter(GameObject shooter)
    {
        setShooter = shooter;
    }

    private void ReturnBullet() => gameObject.SetActive(false);

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player Hit
        if (other.gameObject.tag == "Player")
        {
            switch (setShooter.name)
            {
                case "BaseMeleeMonster":
                    meleeMonster.PlayerHit();
                    break;
                case "RangedMonster":
                    rangedMonster.PlayerHit();
                    break;
                default:
                    break;
            }
        }
        // Monster Hit
        else if (other.gameObject.tag == "Monster" && setShooter.name == "Player")
        {
            switch (other.gameObject.name)
            {
                case "BasicMeleeMonster":
                    meleeMonster.Hit();
                    break;
                case "RangedMonster":
                    rangedMonster.Hit();
                    break;
                default:
                    break;
            }

            // 몬스터 충돌 처리 (임시)
            other.gameObject.SetActive(false); // monster hit func 구현 후 대체
            gameObject.SetActive(false);
        }
        else
        {
            // 벽 or 다른 collider 충돌처리
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        // 발사 후 5초 뒤 Bullet 비활성화
        Invoke("ReturnBullet", bulletLifeTime);
    }

    private void OnDisable()
    {
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
}
