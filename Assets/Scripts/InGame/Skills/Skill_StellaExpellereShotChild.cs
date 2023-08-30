using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Skill_StellaExpellereShotChild : MonoBehaviour
{
    CircleCollider2D stellaExpellereCollider;
    float reverseGravity;
    WaitForSeconds waitForSeconds;

    private void Awake()
    {
        if (gameObject.TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider2D))
        {
            stellaExpellereCollider = circleCollider2D;
        }
        else
        {
            stellaExpellereCollider = gameObject.AddComponent<CircleCollider2D>();
        }
    }
    private void Start()
    {
        reverseGravity = 2f;
        waitForSeconds = new WaitForSeconds(1);
    }

    public void CreatestellaExpellere(Vector3 maxScale)
    {
        gameObject.transform.localScale = maxScale;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            // 몬스터 이동방향
            Vector2 moveDirection = (transform.position - collision.transform.position).normalized;
            Rigidbody2D tempRb;

            if (collision.gameObject.name == "BasicMeleeMonster")
            {
                if (collision.gameObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D monsterRb))
                {
                    monsterRb.gravityScale = 0f;
                    StellaExpellereEntry(monsterRb, moveDirection);
                    tempRb = monsterRb;
                }
                else
                {
                    Rigidbody2D newRb = collision.gameObject.AddComponent<Rigidbody2D>();
                    newRb.gravityScale = 0f;
                    StellaExpellereEntry(newRb, moveDirection);
                    tempRb = monsterRb;
                }

                collision.gameObject.GetComponent<MeleeMonster>().Hit();
                StartCoroutine(ResetSkillEffect(tempRb));
            }
        }
    }

    private void StellaExpellereEntry(Rigidbody2D monsterRb, Vector2 moveDirection)
    {
        Vector2 tempDir = moveDirection;

        tempDir = monsterRb.transform.position - gameObject.transform.position;

        monsterRb.velocity = tempDir * reverseGravity;
    }

    private IEnumerator ResetSkillEffect(Rigidbody2D monsterRb)
    {
        yield return waitForSeconds;
        monsterRb.velocity = Vector3.zero;
    }
}
