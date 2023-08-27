using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_HealingShot : MonoBehaviour
{
    // bullet info
    private Rigidbody2D bulletRb;
    private CircleCollider2D bulletCollider2D;
    private MeleeMonster meleeMonster;

    // script
    private Player player;

    private float recoveryAmount;


    private void Awake()
    {
        InitHealingShot();
    }

    private void Start()
    {
        recoveryAmount = 10f;
    }

    private void InitHealingShot()
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

        player = FindObjectOfType<Player>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            player.playerCurHp += recoveryAmount;

            if (player.playerCurHp > player.playerMaxHp)
            {
                player.playerCurHp = player.playerMaxHp;
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

    public void SetRecoveryAmount(float recAmount)
    {
        recoveryAmount = recAmount;
    }
}
