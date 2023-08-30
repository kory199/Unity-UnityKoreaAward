using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_EnergyShield : SkillBase
{
    // Shield Info
    private GameObject ShieldObj;
    private CircleCollider2D shieldCollider2D;
    private WaitForSeconds delayTime;
    private float lifeTime;
    private Vector3 maxScale;
    private Vector3 increaseScale;
    private GameObject shield;
    private bool isDone;

    private MeleeMonster meleeMonster;


    private void Awake()
    {
        ShieldObj = Resources.Load<GameObject>("Bullet/Bullet_Skill_EnergyShield");
    }

    protected override void Start()
    {
        base.Start();

        shield = Instantiate(ShieldObj, gameObject.transform.position, Quaternion.identity);
        shield.transform.SetParent(gameObject.transform);
        shield.SetActive(false);

        if (shield.TryGetComponent<CircleCollider2D>(out CircleCollider2D circleCollider2D))
        {
            shieldCollider2D = circleCollider2D;
        }
        else
        {
            shieldCollider2D = shield.AddComponent<CircleCollider2D>();
        }

        SkillCoolTime();
        delayTime = new WaitForSeconds(2.5f);
        maxScale = Vector3.one;
        increaseScale = new Vector3(0.01f, 0.01f, 0.01f);
        isDone = false;
        lifeTime = 3f;
        SetMaxScale(1);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void SkillCoolTime()
    {
        _coolTime = 10f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;

        SetMaxScale(_skillLevel);
    }

    public override void SkillShot()
    {
        shield.SetActive(true);
        PlayEnergyShield(maxScale);
    }

    private void SetMaxScale(int skillLevel)
    {
        maxScale +=  Vector3.one * (1 + (skillLevel * 0.05f));
    }

    private void PlayEnergyShield(Vector3 maxScale)
    {
        shield.transform.localScale = maxScale;

        Invoke("ShieldOff", lifeTime);
    }

    private void ShieldOff()
    {
        shield.SetActive(false);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Monster")
        {
            if (collision.gameObject.name == "BasicMeleeMonster")
            {
                if (collision.gameObject.TryGetComponent<MeleeMonster>(out MeleeMonster monster))
                {
                    meleeMonster = monster;
                }
                else
                {
                    meleeMonster = collision.gameObject.AddComponent<MeleeMonster>();
                }

                meleeMonster.HitShield();
            }
        }
    }
}
