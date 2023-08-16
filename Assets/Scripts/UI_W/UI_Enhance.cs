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

    private List<SkillTreeNode> skillTreeList;

    private Vector3 nextSpawnPos = Vector3.zero;
    private int zigzagDirection = 1;
    private int maxtreeNum = 15;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    protected override void Awake()
    {
        if (skillSO == null)
        {
            skillSO = Resources.Load<StageSkillSO>("SKillSO");
        }

        Time.timeScale = 0;

        skillTreeList = new List<SkillTreeNode>();
    }

    protected override void Start()
    {
        SetSkillData();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnClick_ReturnGame();
        }
    }

    private void SetSkillData()
    {
        SkillInfo[] skillInfos = null;

        switch(_stageNum)
        {
            case 1:
                skillInfos = skillSO.sateOne;
                break;
        }

        if(skillInfos != null && skillInfos.Length == skillBtn.Length)
        {
            for(int i = 0; i < skillBtn.Length; ++ i)
            {
                skillBtn[i].GetComponent<Image>().sprite = skillInfos[i].skillImg;

                int index = i;
                int currentBulletNum = skillInfos[index].bulletNum;

                MouseReachSkill(skillBtn[i], skillInfos[index].infotext);

                // 로직 확정 후 버튼 컴포넌트에 추가, 코드 삭제
                Button currentBtn = skillBtn[i];
                currentBtn.onClick.AddListener(() => OnClick_Skill(currentBtn, currentBulletNum));
            }
        }
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
    public int OnClick_Skill(Button clickedBtn, int bulletNum)
    {
        Debug.Log($"BulletNum : {bulletNum}");

        if (skillNodePrefab != null)
        {
            CreateSkillNodePrefab(clickedBtn);
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
}