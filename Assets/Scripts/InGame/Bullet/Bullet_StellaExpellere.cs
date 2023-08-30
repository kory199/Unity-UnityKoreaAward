using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_StellaExpellere : MonoBehaviour
{
    // Bullet info
    private Rigidbody2D bulletRb;
    private CircleCollider2D bulletCollider2D;
    private MeleeMonster meleeMonster;
    private float lifeTime;

    // child info
    private GameObject stellaExpellere;
    Skill_StellaExpellereShotChild skill_StellaExpellereShotChild;
    Vector3 maxScale;
    Vector3 originScale;

    private void Awake()
    {
        StellaExpellereInit();
    }

    private void Start()
    {
        lifeTime = 4f;
    }

    private void StellaExpellereInit()
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

        stellaExpellere = gameObject.transform.GetChild(0).gameObject;

        if (stellaExpellere.TryGetComponent<Skill_StellaExpellereShotChild>(out Skill_StellaExpellereShotChild stellaExpellereChild))
        {
            skill_StellaExpellereShotChild = stellaExpellereChild;
        }
        else
        {
            skill_StellaExpellereShotChild = stellaExpellere.AddComponent<Skill_StellaExpellereShotChild>();
        }

        originScale = new Vector3(0.1f, 0.1f, 0.1f);
        maxScale = Vector3.one;

        stellaExpellere.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (collision.gameObject.name == "BasicMeleeMonster")
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
                stellaExpellere.SetActive(true);
                skill_StellaExpellereShotChild.CreatestellaExpellere(maxScale);
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

    public void SetStellaExpellereScale(int skillLevel)
    {
        maxScale *= 1 + (skillLevel * 0.05f);
    }
}
