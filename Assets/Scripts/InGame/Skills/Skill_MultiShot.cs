using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_MultiShot : SkillBase
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
        _damage += 0.3f;
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
        Debug.Log("skill multishot awake");
        _eff = Instantiate(Resources.Load<GameObject>("Effect/"));
    }

    protected override void Start()
    {
        Debug.Log("skill multishot start");
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
