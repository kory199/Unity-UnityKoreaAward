using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Enhance : UIBase
{
    [SerializeField] TextMeshProUGUI enhanceNum = null;
    [SerializeField] TextMeshProUGUI infoText = null;

    [SerializeField] GameObject infotextObj = null;
    [SerializeField] GameObject skillNodePrefab;
    [SerializeField] RectTransform skillTreePos = null;

    [SerializeField] Button[] skillBtn = null;
    [SerializeField] StageSkillSO skillSO = null;

    private UI_SceneGame _ui_SceneGame = null;
    private Player _player = null;

    private List<SkillTreeNode> skillTreeList;
    private SkillInfo[] skillArray;
    private RectTransform infoTextPos;

    private int _stageNum; 
    private Vector3 nextSpawnPos = Vector3.zero;
    private int zigzagDirection = 1;
    private int maxtreeNum = 15;
    private int skillPoint = 0;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    #region unity lifecycle
    protected override void Awake()
    {
        if (skillSO == null)
        {
            skillSO = Resources.Load<StageSkillSO>("SKillSO");
        }

        skillTreeList = new List<SkillTreeNode>();
        skillArray = new SkillInfo[12];
        AddSkillArray();

        _ui_SceneGame = FindObjectOfType<UI_SceneGame>();
        _player = FindObjectOfType<Player>();
        infotextObj.SetActive(false);
        infoTextPos = infotextObj.GetComponent<RectTransform>();
    }

    protected override void Start()
    {
        //Debug.Log($"start _stageNum : {_stageNum}");
        //StageButtonSet();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.EseKey);
            OnClick_ReturnGame();
        }
    }

    private void OnEnable()
    {
        _stageNum = StageManager.Instance.GetStageNum();
        //Debug.Log($"OnEnable _stageNum : {_stageNum}");
        StageButtonSet();
    }
    #endregion

    private void StageButtonSet()
    {
        if (_stageNum == 0)
            return;
        else
        {
            Time.timeScale = 0;
        }

        switch (_stageNum)
        {
            case 1:
                ButtonOnShow(3);
                SetSkillSO(3);
                break;
            case 2:
                ButtonOnShow(6);
                SetSkillSO(6);
                break;
            case 3:
                ButtonOnShow(9);
                SetSkillSO(9);
                break;
            case 4:
                ButtonOnShow(12);
                SetSkillSO(12);
                break;
            case 5:
                ButtonOnShow(12);
                SetSkillSO(12);
                break;
        }
    }

    private void ButtonOnShow(int num)
    {
        for(int i = 0; i < skillBtn.Length; ++i)
        {
            if(i < num)
            {
                skillBtn[i].gameObject.SetActive(true);

            }
            else
            {
                skillBtn[i].gameObject.SetActive(false);
            }
        }
    }

    private void SetSkillSO(int num)
    {
        for (int i = 0; i < num; ++i)
        {
            skillBtn[i].GetComponent<Image>().sprite = skillArray[i].skillImg;

            int index = i;
            int currentBulletNum = skillArray[index].bulletNum;

            MouseReachSkill(skillBtn[i], skillArray[index].infotext, index);

            Button currentBtn = skillBtn[i];
            currentBtn.onClick.AddListener(() => OnClick_Skill(currentBtn, currentBulletNum, skillArray[index]));
        }
    }

    public override void OnShow()
    {
        SkillBtnControl();
        base.OnShow();
    }

    public void MouseReachSkill(Button targetButton, string skillInfoText, int btnIndex)
    {
        EventTrigger trigger = targetButton.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { MouseEvent((PointerEventData)eventData, skillInfoText, btnIndex); });
        trigger.triggers.Add(entry);
    }

    private void MouseEvent(PointerEventData eventData, string skillInfoText, int btnIndex)
    {
        infotextObj.SetActive(true);
        MousePosMove(eventData, btnIndex);
        infoText.text = skillInfoText;
    }

    private void MousePosMove(PointerEventData pointData, int btnIndex)
    {
        Vector2 localPoint;
        Vector3 pos = infotextObj.transform.position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(infoTextPos, pointData.position, pointData.pressEventCamera, out localPoint))
        {
            if(btnIndex >= 0 && btnIndex <= 2)
            {
                infotextObj.transform.position = pos;
            }
            else if (btnIndex >= 3 && btnIndex <= 5)
            {
                infotextObj.transform.position = new Vector3(900f, 200f, 0f);
            }
            else if(btnIndex >= 6 && btnIndex <= 8)
            {
                infotextObj.transform.position = new Vector3(1200f, 200f, 0f);
            }
            else if(btnIndex >= 9 && btnIndex <= 11)
            {
                infotextObj.transform.position = new Vector3(1600f, 200f, 0f);
            }
        }
    }

    // 나중에 필요한 객체로 변경
    public int OnClick_Skill(Button clickedBtn, int bulletNum, SkillInfo skillInfo)
    {
        skillPoint--;
        SkillBtnControl();

        if (skillNodePrefab != null)
        {
            CreateSkillNodePrefab(clickedBtn);
            string imagePath = skillInfo.skillImg.ToString().Split(' ')[0];
            _ui_SceneGame.AddSkill(skillInfo.SkillClassName, imagePath);
        }

        OnHide();
        return bulletNum;
    }

    // TODO : 프리펩 생성 나중에 오브젝트 풀링으로 변경 
    private void CreateSkillNodePrefab(Button clickedBtn)
    {
        if (skillTreeList.Count >= maxtreeNum)
        {
            return;
        }

        GameObject newSkillNode = Instantiate(skillNodePrefab, skillTreePos.position + nextSpawnPos, Quaternion.identity, skillTreePos);
        newSkillNode.transform.eulerAngles = new Vector3(0, 0, -45);

        SkillTreeNode nodeComponent = newSkillNode.GetComponent<SkillTreeNode>();

        if (nodeComponent != null)
        {
            Sprite clickedSprite = clickedBtn.GetComponent<Image>().sprite;
            nodeComponent.SetSprite(clickedSprite);
        }

        skillTreeList.Add(nodeComponent);

        nextSpawnPos.y -= 50;
        nextSpawnPos.x += 50 * zigzagDirection;

        zigzagDirection *= -1;
    }

    public void SkillTreeListClear()
    {
        skillTreeList.Clear();
        // TODO : 생성된 오브젝트 풀링에서 삭제 
    }

    public void OnClick_ReturnGame()
    {
        OnHide();
    }

    public void GetSkillPoint(int playerSkillPoint)
    {
        skillPoint += playerSkillPoint;
    }

    private void SkillBtnControl()
    {
        // 스킬포인트가 없으면 스킬 선택 버튼 비활성화 및 알파값 조절
        for (int i = 0; i < skillBtn.Length; i++)
        {
            Color newColor = skillBtn[i].GetComponent<Image>().color;

            if (skillPoint > 0)
            {
                newColor.a = 1f;
                skillBtn[i].GetComponent<Image>().color = newColor;
                skillBtn[i].interactable = true;
            }
            else
            {
                newColor.a = 0.5f;
                skillBtn[i].GetComponent<Image>().color = newColor;
                skillBtn[i].interactable = false;
            }
        }
        enhanceNum.text = "enhanceNum : " + skillPoint.ToString();
    }

    private void AddSkillArray()
    {
        skillSO.setOne.CopyTo(skillArray, 0);
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}