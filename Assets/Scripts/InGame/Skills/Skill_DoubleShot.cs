using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Skill_DoubleShot : SkillBase
{
    [SerializeField] private GameObject _eff = null;

    private Sprite Sprite_NerfShot;
    private float _damage;
    float damageReduction;

    public override void SkillCoolTime()
    {
        _coolTime -= 0.5f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;
        _damage += 0.5f;
    }

    public override void SkillShot()
    {
        GameObject dpubleShot_bullet = ObjectPooler.SpawnFromPool("Bullet2D", _player.transform.position);

        if (dpubleShot_bullet != null)
        {
            Bullet bullet = dpubleShot_bullet.GetComponent<Bullet>();

            damageReduction = 1 - (_skillLevel * 0.1f);

            bullet.GetComponent<SpriteRenderer>().sprite = Sprite_NerfShot; 

            bullet.bulletDamageReduction = damageReduction;
        }

        StartCoroutine(Co_EffectOff());
    }

    IEnumerator Co_EffectOff()
    {
        yield return new WaitForSeconds(1f);
        _eff.SetActive(false);
    }

    private void Awake()
    {
        Debug.Log("skill doubleshot awake");
        _eff = Instantiate(Resources.Load<GameObject>("Effect/"));
    }

    protected override void Start()
    {
        Debug.Log("skill doubleshot start");
        base.Start();
        if (_uI_SceneGame == null)
        {
            _uI_SceneGame = FindObjectOfType<UI_SceneGame>();
        }
    }

    protected override void Update()
    {
        base.Update();   
    }
}
