using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_NerfShot : MonoBehaviour
{
    Rigidbody2D bulletRb;
    CircleCollider2D bulletCollider2D;
    MeleeMonster meleeMonster;

    private float damageReduction;


    private void Awake()
    {
        NerfShotInit();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void NerfShotInit()
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
    }

    public void SetDamageReduction(float damageReduction)
    {
        this.damageReduction = damageReduction;
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

                meleeMonster.HitNerfShot(damageReduction);
                meleeMonster.Hit();
                Destroy(gameObject);
            }
            else
            {

            }
        }
        else if (collision.gameObject.tag == "Wall")
        {
                Destroy(gameObject);
        }
        else
        {
            
        }
    }
}
