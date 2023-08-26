using System.Collections;
using UnityEngine;
using static EnumTypes;

public class Bullet : MonoBehaviour
{
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletLifeTime;
    [SerializeField] public float rangedBulletDamage;
    [SerializeField] public float meleeBulletDamage;
    [SerializeField] public float playerBulletDamage;
    [SerializeField] public int hitCount;
    public float bulletDamageReduction;
    private bool isMove;
    WaitForSeconds moveAble;

    [SerializeField] GameObject playerObject;
    [SerializeField] Player player;
    [SerializeField] GameObject bossMonster;
    [SerializeField] MeleeMonster meleeMonster;
    [SerializeField] RangedMonster rangedMonster;
    [SerializeField] BossOne _bossOne;
    [SerializeField] string setShooter;

    public Vector2 dirBullet;
    public CircleCollider2D bulletCollider;
    public Rigidbody2D bulletRb;

    private Transform bulletSpawner;
    private Sprite originalSprite;

    #region Unity Life Cycle
    private void Awake()
    {
        BulletInit();
    }

    private void OnEnable()
    {
        SkillVariableReset();
        // 발사 후 5초 뒤 Bullet 비활성화        
        Invoke("ReturnBullet", bulletLifeTime);
    }

    private void Start()
    {

    }

    private void OnDisable()
    {
        hitCount = 0;
        ObjectPooler.ReturnToPool(gameObject);
        CancelInvoke();
    }
    #endregion

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


        playerObject = FindObjectOfType<Player>().gameObject;

        if (playerObject.TryGetComponent<Player>(out Player player))
        {
            this.player = player;
        }
        else
        {
            player = playerObject.AddComponent<Player>();
        }

        bulletSpawner = playerObject.transform;
        bulletRb.gravityScale = 0;
        bulletCollider.isTrigger = true;
        isMove = true;
        moveAble = new WaitForSeconds(0.3f);

        // 레벨업 등에 따라 바뀜 (초기 값으로 추후 스크립터블 오브젝트에서 값을 받아와야됨)
        bulletLifeTime = 10f;
        rangedBulletDamage = 10f;
        meleeBulletDamage = 20f;
        playerBulletDamage = 10f;

        hitCount = 0;
    }

    private void SkillVariableReset()
    {
        bulletDamageReduction = 1f;
    }

    private void ReturnBullet() => gameObject.SetActive(false);

    public void SetShooter(string shooter)
    {
        setShooter = shooter;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Player Hit
        if (other.gameObject.tag == "Player")
        {
            switch (setShooter)
            {
                case "RangedMonster":
                    rangedMonster.PlayerHit();
                    break;
                case "BossOne":
                    // Boss Attack to player
                    break;
                case "Monster":
                    player.PlayerHit(player.playerAttackPower);
                    gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
        // Monster Hit
        else if (other.gameObject.tag == "Monster" && setShooter == "Player")
        {
            switch (other.gameObject.name)
            {
                case "BasicMeleeMonster":
                    meleeMonster = other.gameObject.GetComponent<MeleeMonster>();
                    meleeMonster.Hit();
                    break;
                case "RangedMonster":
                    rangedMonster = other.gameObject.GetComponent<RangedMonster>();
                    rangedMonster.Hit();
                    break;
                case "BossOne":
                    _bossOne = other.gameObject.GetComponent<BossOne>();
                    _bossOne.Hit();
                    break;
                default:
                    break;
            }
            gameObject.SetActive(false);
        }
        else if (hitCount <= 3 && other.gameObject.tag == "Wall" && setShooter == "Player")
        {
            hitCount++;
            gameObject.transform.position *= -1;
            setShooter = "Monster";
        }
        else if (other.gameObject.tag == "Gun")
        {
            gameObject.SetActive(false);
        }
        else
        {
            // gameObject.SetActive(false);
        }
    }
}