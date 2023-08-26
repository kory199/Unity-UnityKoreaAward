using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Enhance : UIBase
{
    [SerializeField] int _stageNum; // 추후 받아온 데이터로 셋
    [SerializeField] TextMeshProUGUI enhanceNum = null;
    [SerializeField] TextMeshProUGUI rerollNum = null;
    [SerializeField] TextMeshProUGUI infoText = null;

    [SerializeField] GameObject skillNodePrefab;
    [SerializeField] RectTransform skillTreePos = null;

    [SerializeField] Button[] skillBtn = null;
    [SerializeField] StageSkillSO skillSO = null;

    private UI_SceneGame _ui_SceneGame = null;
    private Player _player = null;

    private List<SkillTreeNode> skillTreeList;

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

        Time.timeScale = 0;

        skillTreeList = new List<SkillTreeNode>();

        _ui_SceneGame = FindObjectOfType<UI_SceneGame>();
        _player = FindObjectOfType<Player>();
    }

    protected override void Start()
    {
        SetSkillData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.EseKey);
            OnClick_ReturnGame();
        }
    }
    #endregion

    private void SetSkillData()
    {
        SkillInfo[] skillInfos = null;

        switch (_stageNum)
        {
            case 1:
                skillInfos = skillSO.setOne;
                break;

            case 2:
                skillInfos = skillSO.setOne;
                break;

                //스테이지마다 다른스킬
        }

        if (skillInfos != null && skillInfos.Length == skillBtn.Length)
        {
            for (int i = 0; i < skillBtn.Length; ++i)
            {
                skillBtn[i].GetComponent<Image>().sprite = skillInfos[i].skillImg;

                int index = i;
                int currentBulletNum = skillInfos[index].bulletNum;

                MouseReachSkill(skillBtn[i], skillInfos[index].infotext);

                // 로직 확정 후 버튼 컴포넌트에 추가, 코드 삭제
                Button currentBtn = skillBtn[i];
                currentBtn.onClick.AddListener(() => OnClick_Skill(currentBtn, currentBulletNum, skillInfos[index]));
            }
        }
    }

    public override void OnShow()
    {
        SkillBtnControl();
        base.OnShow();
    }

    public void MouseReachSkill(Button targetButton, string skillInfoText)
    {
        EventTrigger trigger = targetButton.gameObject.AddComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerEnter;
        entry.callback.AddListener((eventData) => { infoText.text = skillInfoText; });
        trigger.triggers.Add(entry);
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
        Time.timeScale = 1;
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
}