using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_BlackHoleChild : MonoBehaviour
{
    CircleCollider2D blackHoleCollider;

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
    }

    public IEnumerator CreateBlackHole(Vector3 originScale, Vector3 maxScale, Vector3 increaseScale)
    {
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
            if (collision.gameObject.name == "BasicMeleeMonster")
            {
                Rigidbody2D monsterTransform = collision.gameObject.GetComponent<Rigidbody2D>();

            }
        }
    }

    // private IEnumerator BlackHoleEntry(Rigidbody2D monsterRb)
    // {
    //     while (true)
    //     {
    // 
    // 
    //         yield return null;
    //     }
    // }
}
