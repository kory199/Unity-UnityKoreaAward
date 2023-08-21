using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UI_SceneGame : UIBase
{
    [SerializeField] private Slider _hpBar = null;
    [SerializeField] private Slider _expBar = null;
    [SerializeField] private TextMeshProUGUI _stage = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _attackPower = null;

    [SerializeField] private RawImage[] _skillImage = null;
    [SerializeField] private RawImage[] _skillCover = null;
    [SerializeField] private Image[] _skillCool = null;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }


    #region unity life cycle
    Player player = null;
    private void OnEnable()
    {
        player = FindObjectOfType<Player>();
        player.uI_SceneGame = this;
    }
    #endregion
    Popup_GamePause _popup_GamePause = null;
    public void OnClick_GamePause()
    {
        Time.timeScale = 0;
        if (_popup_GamePause == null)
            _popup_GamePause = UIManager.Instance.CreateObject<Popup_GamePause>("Popup_GamePause", EnumTypes.LayoutType.Middle);
        _popup_GamePause.OnShow();
    }
    public void SetNowHP(float hp)
    {
        _hpBar.value = hp;
    }
    public void SetMaxHP(float maxHp)
    {
        _hpBar.maxValue = maxHp;
    }
    public void SetNowExp(int exp)
    {
        _expBar.value = exp;
    }
    public void SetMaxExp(int maxExp)
    {
        _expBar.maxValue = maxExp;
    }
    public void SetStageNum(int num)
    {
        _stage.text = "Stage\n" + num.ToString();
    }
    public void SetLevel(int level)
    {
        _level.text = level.ToString();
    }
    public void SetAttackPower(int power)
    {
        _attackPower.text = power.ToString();
    }
    public void SetCoolTime(KeyCode keyCode, float coolTime, Action callback)
    {
        switch (keyCode)
        {
            case KeyCode.Q:
                SetCoolTime(0, coolTime, callback);
                break;
            case KeyCode.W:
                SetCoolTime(1, coolTime, callback);
                break;
            case KeyCode.E:
                SetCoolTime(2, coolTime, callback);
                break;
            case KeyCode.R:
                SetCoolTime(3, coolTime, callback);
                break;
            case KeyCode.T:
                SetCoolTime(4, coolTime, callback);
                break;
            ////qwert vs 12345
            case KeyCode.Alpha1:
                SetCoolTime(0, coolTime, callback);
                break;
            case KeyCode.Alpha2:
                SetCoolTime(1, coolTime, callback);
                break;
            case KeyCode.Alpha3:
                SetCoolTime(2, coolTime, callback);
                break;
            case KeyCode.Alpha4:
                SetCoolTime(3, coolTime, callback);
                break;
            case KeyCode.Alpha5:
                SetCoolTime(4, coolTime, callback);
                break;

        }
    }
    private void SetCoolTime(int num, float coolTime, Action callback)
    {
        _skillCover[num].gameObject.SetActive(true);
        _skillCool[num].fillAmount = 0f;
        StartCoroutine(Co_CoolTime(num, coolTime, callback));
    }
    IEnumerator Co_CoolTime(int num, float coolTime, Action callback)
    {
        float nowTime = 0;
        while (nowTime < coolTime)
        {
            yield return null;
            nowTime += Time.deltaTime;
            _skillCool[num].fillAmount = nowTime / coolTime;
#if UNITY_EDITOR
            Debug.Log(nowTime / coolTime);
#endif
        }
        _skillCover[num].gameObject.SetActive(false);
        callback();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AddSkillTest("Skill_Flash", "Skill_Flash");
        }
    }

    /// <summary>
    /// UI_Enhance에서 호출해야함 
    /// </summary>
    public void AddSkillTest(string skillName , string imagePath)
    {
        Debug.Log("스킬 추가");
        switch(skillName)
        {
            case "Skill_Flash":
                Skill_Flash newskill = player.gameObject.AddComponent<Skill_Flash>();

                //스킬 번호 지정 로직 수정필요
                newskill.ShotKey = KeyCode.Q;
                _skillImage[0].texture = Resources.Load<Sprite>(imagePath).texture;
                break;
        }
    
    }
}