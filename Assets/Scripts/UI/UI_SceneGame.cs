using System;
using System.Collections;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_SceneGame : UIBase
{
    [SerializeField] private Slider _hpBar = null;
    [SerializeField] private Slider _expBar = null;
    [SerializeField] private TextMeshProUGUI _stage = null;
    [SerializeField] private TextMeshProUGUI _level = null;
    [SerializeField] private TextMeshProUGUI _attackPower = null;
    [SerializeField] private TextMeshProUGUI _score = null;

    [SerializeField] private RawImage[] _skillImage = null;
    [SerializeField] private RawImage[] _skillCover = null;
    [SerializeField] private Image[] _skillCool = null;

    [SerializeField] Button _pausebtn = null;
    [SerializeField] private Sprite _skillBaseImg =null;

    [SerializeField] private RawImage[] _dirKeys = null;
    [SerializeField] private RawImage[] _attackKeys = null;


    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    #region unity life cycle
    Player _player = null;
    private void OnEnable()
    {
        _player = FindObjectOfType<Player>();
        _player.uI_SceneGame = this;
        StartCoroutine(ActivatePauseButtonAfterDelay());
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            _dirKeys[0].color = Color.blue;
        }
        if(Input.GetKeyDown(KeyCode.A))
        {
            _dirKeys[1].color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            _dirKeys[2].color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            _dirKeys[3].color = Color.blue;
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            _dirKeys[0].color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            _dirKeys[1].color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            _dirKeys[2].color = Color.white;
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            _dirKeys[3].color = Color.white;
        }
        if (Input.GetMouseButtonDown(0))
        {
            _attackKeys[0].color = Color.blue;
        }
        if (Input.GetMouseButtonDown(1))
        {
            _attackKeys[1].color = Color.blue;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _attackKeys[0].color = Color.white;
        }
        if (Input.GetMouseButtonUp(1))
        {
            _attackKeys[1].color = Color.white;
        }
    }


    private IEnumerator ActivatePauseButtonAfterDelay()
    {
        _pausebtn.interactable = false;
        yield return new WaitForSeconds(3f);
        _pausebtn.interactable = true;
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
    public void SetScore(int score)
    {
        _score.text = "Score : " + score.ToString();
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
        }
        _skillCover[num].gameObject.SetActive(false);
        callback();
    }

    [SerializeField] string _basePath = "SkillSprites/";
    int _skillKeyNum = 0;
    /// <summary>
    /// UI_Enhance에서 호출해야함
    /// /// </summary>
    public void AddSkill(string skillName, string imagePath)
    {
        if (_skillKeyNum >= 5) return;

        Assembly assembly = Assembly.GetExecutingAssembly();

        Type type = assembly.GetType(skillName);

        if (type != null)
        {
            Component newSkillType = _player.GetComponent(type);
            if (newSkillType == null)
            {
                newSkillType = _player.gameObject.AddComponent(type);
                (newSkillType as SkillBase).ShotKey = GetSkillKeyCode();
                (newSkillType as SkillBase).UI_SceneGame = this;
                string path = string.Concat(_basePath, imagePath);
                _skillImage[_skillKeyNum].texture = Resources.Load<Sprite>(path).texture;
                _skillKeyNum++;
            }
            else
            {
                (newSkillType as SkillBase).SkillLevelUp();
            }

            /* Component newSkillType = _player.gameObject.AddComponent(type);
             (newSkillType as SkillBase).ShotKey = GetSkillKeyCode();
             (newSkillType as SkillBase).UI_SceneGame = this;
             string path = string.Concat(_basePath, imagePath);
             _skillImage[_skillKeyNum].texture = Resources.Load<Sprite>(path).texture;*/
        }
        else
        {
            Debug.LogError("There is no Skills");
        }
        //_skillKeyNum++;
    }
    private KeyCode GetSkillKeyCode()
    {
        switch (_skillKeyNum)
        {
            case 0:
                return KeyCode.Alpha1;
            case 1:
                return KeyCode.Alpha2;
            case 2:
                return KeyCode.Alpha3;
            case 3:
                return KeyCode.Alpha4;
            case 4:
                return KeyCode.Alpha5;
            default:
                Debug.LogError("It Can not add skill. Because your skills is full.");
                return KeyCode.Alpha0;
        }
    }
    public override void OnHide()
    {
        base.OnHide();
    }
    public override void OnShow()
    {
        base.OnShow();
        foreach (var image in _skillImage)
        {
            image.texture = _skillBaseImg.texture;
        }
        _skillKeyNum = 0;
    }
}