using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_Flash : SkillBase
{
    [SerializeField] private GameObject _eff = null;
    public override void SkillCoolTime()
    {
        //스킬 쿨타임
        _coolTime = 10f;
    }
    private float _distance = 3;
    public override void SkillLevelUp()
    {
        //스킬 레벨업시 효과
        _skillLevel++;
        _distance += 1f;
    }

    public override void SkillShot()
    {
        //자유 기능 구현
        _eff.SetActive(true);
        _eff.transform.position = gameObject.transform.position;
        Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dir = (newPos - gameObject.transform.position).normalized;
        gameObject.transform.position = new Vector3(gameObject.transform.position.x + dir.x * _distance,
            gameObject.transform.position.y + dir.y * _distance, 0);

        StartCoroutine(Co_EffectOff());
        Debug.Log("점멸");
    }
    WaitForSeconds wait = new WaitForSeconds(1f);
    IEnumerator Co_EffectOff()
    {
        yield return wait;
        _eff.SetActive(false);
    }
    private void Awake()
    {
        Debug.Log("skill flash awake");
        _eff = Instantiate(Resources.Load<GameObject>("Effect/Eff_Player_Flash"));
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        if (_uI_SceneGame == null)
        {
            _uI_SceneGame = FindObjectOfType<UI_SceneGame>();
        }
        Debug.Log("skill flash start");
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}