using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_BlackHoleChild : MonoBehaviour
{
    CircleCollider2D blackHoleCollider;
    float gravity;

    private void Awake()
    {
        if (gameObject.TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider2D))
        {
            blackHoleCollider = circleCollider2D;
        }
        else
        {
            blackHoleCollider = gameObject.AddComponent<CircleCollider2D>();
        }
        gravity = 2f;
    }

    public IEnumerator CreateBlackHole(Vector3 originScale, Vector3 maxScale, Vector3 increaseScale)
    {
        gameObject.transform.localScale = originScale;

        while (originScale.x < maxScale.x)
        {
            gameObject.transform.localScale = originScale;

            originScale += increaseScale;

            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            // 몬스터 이동방향
            Vector2 moveDirection = (transform.position - collision.transform.position).normalized;
            
            if (collision.gameObject.name == "BasicMeleeMonster")
            {
                if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D monsterRb))
                {
                    monsterRb.gravityScale = 0f;
                    StartCoroutine(BlackHoleEntry(monsterRb, moveDirection));
                }
                else
                {
                    Rigidbody2D newRb = collision.gameObject.AddComponent<Rigidbody2D>();
                    newRb.gravityScale = 0f;
                    StartCoroutine(BlackHoleEntry(newRb, moveDirection));
                }
            }
        }
    }

    private IEnumerator BlackHoleEntry(Rigidbody2D monsterRb, Vector2 moveDirection)
    {
        Vector2 tempDir = moveDirection;

        while (true)
        {
            tempDir = gameObject.transform.position - monsterRb.transform.position;

            monsterRb.velocity = tempDir * gravity;
            yield return null;
        }
    }
}
