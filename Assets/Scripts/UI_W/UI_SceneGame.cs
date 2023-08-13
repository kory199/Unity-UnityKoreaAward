using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UI_SceneGame : UIBase
{
    [SerializeField] private Slider _hpBar = null;
    [SerializeField] private Slider _expBar = null;
    [SerializeField] private TextMeshProUGUI _stage = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _attackPower = null;

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
    public void SetNowExp (int exp)
    {
        _expBar.value = exp;
    }
    public void SetMaxExp(int maxExp)
    {
        _expBar.maxValue = maxExp;
    }
    public void SetStageNum(int num)
    {
        _stage.text = "Stage\n" +num.ToString();
    }
    public void SetLevel(int level)
    {
        _level.text = level.ToString();
    }   
    public void SetAttackPower(int power)
    {
        _attackPower.text = power.ToString();
    }
}
