using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_SlowingShot : SkillBase
{
    [SerializeField] private GameObject _eff = null;

    private float _damage;

    public override void SkillCoolTime()
    {
        _coolTime -= 0.5f;
    }

    public override void SkillLevelUp()
    {
        _skillLevel++;
        _damage += 1f;
    }

    public override void SkillShot()
    {
        StartCoroutine(Co_EffectOff());
    }

    IEnumerator Co_EffectOff()
    {
        yield return new WaitForSeconds(1f);
        _eff.SetActive(false);
    }

    private void Awake()
    {
        Debug.Log("skill slowingshot awake");
        _eff = Instantiate(Resources.Load<GameObject>("Effect/"));
    }

    protected override void Start()
    {
        Debug.Log("skill slowingshot start");
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
