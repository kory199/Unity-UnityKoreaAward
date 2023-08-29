using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class Skill_NapalmShotDotDamage : MonoBehaviour
{
    private MeleeMonster meleeMonster;
    private float time;
    private float delayTime;
    private float lifeTime;
    private bool isFirst;

    private void Awake()
    {
        SetMonster();
    }

    void Start()
    {
        time = 0f;
        lifeTime = 4f;
        delayTime = 1f;
        isFirst = true;

        StartCoroutine(SendDotDamageCoroutine());
    }

    private void OnDisable()
    {
        Destroy(gameObject.GetComponent<Skill_NapalmShotDotDamage>());
    }

    private void SetMonster()
    {
        if (gameObject.TryGetComponent<MeleeMonster>(out MeleeMonster meleeMonster))
        {
            this.meleeMonster = meleeMonster;
        }
        else
        {
            this.meleeMonster = gameObject.AddComponent<MeleeMonster>();
        }
    }

    private IEnumerator SendDotDamageCoroutine()
    {
        while (time < lifeTime)
        {
            time += Time.deltaTime;
            delayTime += Time.deltaTime;

            if (meleeMonster.gameObject != null && delayTime > 1f)
            {
                meleeMonster.Hit();
                delayTime = 0;
            }

            yield return null;
        }

        time = 0;
        delayTime = 0;
        isFirst = false;
    }
}
