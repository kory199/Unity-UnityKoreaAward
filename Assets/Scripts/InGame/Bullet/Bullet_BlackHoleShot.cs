using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bullet_BlackHoleShot : MonoBehaviour
{
    // Bullet info
    private Rigidbody2D bulletRb;
    private CircleCollider2D bulletCollider2D;
    private MeleeMonster meleeMonster;
    private float lifeTime;

    // child info
    private GameObject blackHole;
    Skill_BlackHoleChild skill_BlackHoleChild;
    Vector3 maxScale;
    Vector3 originScale;
    Vector3 increaseScale;

    #region Unity Life Cycle
    private void Awake()
    {
        BlackHoleShotInit();
    }
    // Start is called before the first frame update
    private void Start()
    {
        lifeTime = 3f;
    }
    #endregion

    private void BlackHoleShotInit()
    {
        if (gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody2D))
        {
            bulletRb = rigidbody2D;
        }
        else
        {
            bulletRb = gameObject.AddComponent<Rigidbody2D>();
        }

        if (gameObject.TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider2D))
        {
            bulletCollider2D = circleCollider2D;
        }
        else
        {
            gameObject.AddComponent<CircleCollider2D>();
        }

        blackHole = gameObject.transform.GetChild(0).gameObject;

        if (blackHole.TryGetComponent<Skill_BlackHoleChild>(out Skill_BlackHoleChild blackholeChild))
        {
            skill_BlackHoleChild = blackholeChild;
        }
        else
        {
            skill_BlackHoleChild = blackHole.AddComponent<Skill_BlackHoleChild>();
        }

        originScale = new Vector3(0.1f, 0.1f, 0.1f);
        maxScale = Vector3.one;
        increaseScale = new Vector3(0.0002f, 0.0002f, 0.0002f);

        blackHole.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (collision.gameObject.name == "BossOne")
            {
                // 보스 로직 추가 필요
            }
            else if (collision.gameObject.name == "BasicMeleeMonster")
            {
                if (collision.gameObject.TryGetComponent<MeleeMonster>(out MeleeMonster meleeMonster))
                {
                    this.meleeMonster = meleeMonster;
                }
                else
                {
                    meleeMonster = collision.gameObject.AddComponent<MeleeMonster>();
                }

                // meleeMonster.Hit();
                bulletRb.velocity = Vector3.zero;
                blackHole.SetActive(true);
                StartCoroutine(skill_BlackHoleChild.CreateBlackHole(originScale, maxScale, increaseScale));
                Destroy(gameObject, lifeTime);
            }
            else
            {
                // 다른 몬스터 종류 추가 시
            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
        else
        {
            // 다른 오브젝트 추가 시
        }
    }

    public void SetBlackHoleScale(int skillLevel)
    {
        maxScale *= 1 + (skillLevel * 0.05f);
    }
}
