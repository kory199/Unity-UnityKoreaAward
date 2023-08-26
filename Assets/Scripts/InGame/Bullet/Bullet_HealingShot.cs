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
    }
}
