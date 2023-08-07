using System.Collections.Generic;
using APIModels;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("[PanelObj]")]
    [SerializeField] GameObject[] panels = null;
    // start : 0, account : 1, rank : 2, option : 3

    [SerializeField] Button test = null;

    [Header("[Start]")]
    [SerializeField] Button accountBut = null;
    [SerializeField] Button rankBut = null;
    [SerializeField] Button startBut = null;
    [SerializeField] Button optionBut = null;
    [SerializeField] Button exitBut = null;
    [SerializeField] TextMeshProUGUI loginInfotext = null;
    [SerializeField] TextMeshProUGUI versionText = null;

    [Header("[Ranking]")]
    [SerializeField] Button r_goBackBut = null;
    [SerializeField] TextMeshProUGUI[] rankTopThreeName = null;
    [SerializeField] TextMeshProUGUI[] rankTopTen = null;
    [SerializeField] TextMeshProUGUI userRank = null;
    [SerializeField] TextMeshProUGUI r_infoText = null;

    [Header("Option")]
    [SerializeField] Button o_goBackBut = null;

    private int _currentIndex = 0;
    private GameObject curPanel = null;

    private void Awake()
    {
        curPanel = panels[0];
        ShowUI(curPanel);
        loginInfotext.gameObject.SetActive(false);

        GetGameVersion();
        GetMasterData();
        test.onClick.AddListener(() => GetMasterData_t());

        // === StartPanel Button Event ===
        accountBut.onClick.AddListener(OnClickAccount);
        rankBut.onClick.AddListener(OnClickRank);
        startBut.onClick.AddListener(GoLobbyScene);
        optionBut.onClick.AddListener(delegate { ShowUI(panels[3]);});
        exitBut.onClick.AddListener(delegate { OnExitBut(); });

        // === RankingPanel Button Event ===
        r_goBackBut.onClick.AddListener(OnClickRankBackBut);

        // === OptionPanel Button Event ===
        o_goBackBut.onClick.AddListener(delegate { ShowUI(panels[0]); });
    }

    private void OnClickAccount()
    {
        ShowUI(panels[1]);
    }

    private async void GetGameVersion()
    {
        versionText.text = "Ver :  " +  await APIManager.Instance.GetGameVersionAPI(); ;
    }

    private async void GetMasterData()
    {
        await APIManager.Instance.GetMasterDataAPI();
    }

    private void ShowUI(GameObject showUIPanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == showUIPanel);
        }
    }

    private async void UpdateStartButtonState()
    {
        if (APIDataSO.Instance.GetValueByKey<GameData>(APIDataDicKey.GameData) != null)
        {
            startBut.interactable = true;
            loginInfotext.gameObject.SetActive(false);
            await APIManager.Instance.GetGameDataAPI();
        }
        else
        {
            startBut.interactable = false;
            loginInfotext.gameObject.SetActive(true);
            loginInfotext.text = "Please Log in";
        }
    }

    private void OnEnable()
    {
        UpdateStartButtonState();
        APIDataSO.Instance.OnResponseDataChanged += UpdateStartButtonState;
    }

    private void OnDisable()
    {
        APIDataSO.Instance.OnResponseDataChanged -= UpdateStartButtonState;
    }

    private async void GoLobbyScene()
    {
        if (startBut.interactable)
        {
            await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
        }
    }

    private void OnExitBut()
    {
        APIDataSO.Instance.ClearResponseData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void GetMasterData_t()
    {
        MonsterData_res[] monsterData = APIDataSO.Instance.GetValueByKey<MonsterData_res[]>(APIDataDicKey.MeleeMonster);

        if (monsterData == null)
        {
            Debug.LogError("MonsterData list is null!");
            return;
        }

        foreach (MonsterData_res a in monsterData)
        {
            Debug.Log("MeleeMonstser) Level: " + a.level + "Exp: " + a.exp + "Hp: " + a.hp);
        }

        MonsterData_res boss = APIDataSO.Instance.GetValueByKey<MonsterData_res>(APIDataDicKey.BOSS);
        Debug.Log($"BOSS) Level: {boss.level}, Exp: {boss.exp}, Hp: {boss.hp}");

        MonsterData_res[] rangedMonsterData = APIDataSO.Instance.GetValueByKey<MonsterData_res[]>(APIDataDicKey.RangedMonster);

        if (rangedMonsterData == null)
        {
            Debug.LogError("MonsterData list is null!");
            return;
        }

        foreach (MonsterData_res a in rangedMonsterData)
        {
            Debug.Log("RangedMonster) Level: " + a.level + "Exp: " + a.exp + "Hp: " + a.hp);
        }
    }

    private void OnClickRankBackBut()
    {
        ShowUI(panels[0]);

        for(int i = 0; i < rankTopThreeName.Length; ++i)
        {
            rankTopThreeName[i].text = "";
            rankTopThreeName[i].text = "";
            userRank.text = "";
        }
    }

    private async void OnClickRank()
    {
        ShowUI(panels[2]);
        r_infoText.gameObject.SetActive(false);

        await APIManager.Instance.GetRankingAPI();

        GameData userInfo = APIManager.Instance.GetApiSODicUerData();
        if(userInfo == null)
        {
            r_infoText.gameObject.SetActive(true);
            r_infoText.text = "Please Log in";
        }

        List<RankingData> rankingDataList = APIDataSO.Instance.GetValueByKey<List<RankingData>>("RankingData");

        if (rankingDataList != null && rankingDataList.Count > 0)
        {
            for (int i = 0; i < rankingDataList.Count; i++)
            {
                RankingData rankingData = rankingDataList[i];

                if (i < rankTopThreeName.Length)
                {
                    rankTopThreeName[i].text = rankingData.id;
                }

                if (i < rankTopTen.Length)
                {
                    rankTopTen[i].text = $"ID: {rankingData.id}, Score: {rankingData.score}, Rank: {rankingData.ranking}";
                }

                if(rankingDataList.Count == 11)
                {
                    userRank.text = $"ID: {rankingData.id}, Score: {rankingData.score}, Rank: {rankingData.ranking}";
                }
                else if (rankingDataList.Count == 10)
                {
                    RankingData userRankingData = rankingDataList.Find(r => r.id == userInfo.ID);
                    if (userRankingData != null)
                    {
                        userRank.text = $"ID: {userRankingData.id}, Score: {userRankingData.score}, Rank: {userRankingData.ranking}";
                    }
                    else
                    {
                        Debug.LogError("User's ranking data not found.");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("No ranking data found in APIDataSO.");
        }

    }
}