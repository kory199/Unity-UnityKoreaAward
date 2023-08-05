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

    [Header("[Account]")]
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] Button createAccountBut = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] TextMeshProUGUI infotext = null;
    [SerializeField] Button a_goBackBut = null;

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

        // === AccountPanel Button Event ===
        a_goBackBut.onClick.AddListener(OnClickAccountBackBut);
        createAccountBut.onClick.AddListener(delegate { OnClickCreareAccount(); });
        loginBut.onClick.AddListener(delegate { OnClickLogin(); });

        // === RankingPanel Button Event ===
        r_goBackBut.onClick.AddListener(OnClickRankBackBut);

        // === OptionPanel Button Event ===
        o_goBackBut.onClick.AddListener(delegate { ShowUI(panels[0]); });
    }

    private async void OnClickAccount()
    {
        ShowUI(panels[1]);
        infotext.gameObject.SetActive(false);
        await CursorMoveCheck();
    }

    private async UniTask CursorMoveCheck()
    {
        inputFields[_currentIndex].Select();
        inputFields[1].contentType = TMP_InputField.ContentType.Password;

        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                await MoveToNextInputField();
            }

            await UniTask.Yield();
        }
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
        Debug.Log($"BOSS) Level: {boss.level}, Exp: { boss.exp}, Hp: { boss.hp}");

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

    private async UniTask MoveToNextInputField()
    {
        inputFields[_currentIndex].DeactivateInputField();
        _currentIndex = (_currentIndex + 1) % inputFields.Length;
        inputFields[_currentIndex].ActivateInputField();

        await UniTask.WaitUntil(() => inputFields[_currentIndex].isFocused);
    }

    private async void GetGameVersion()
    {
        versionText.text = "Ver :  " +  await APIManager.Instance.GetGameVersionAPI(); ;
    }

    private async void GetMasterData()
    {
        await APIManager.Instance.GetMasterDataAPI();
    }

    private async void OnClickCreareAccount()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instance.CreateAccountAPI(user);
            infotext.text = $"Created New Account Successful ! {user.ID}, {user.Password}";
        }
    }

    private async void OnClickLogin()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instance.LoginAPI(user);
            infotext.text = $"Login Successful {user.ID}, {user.Password}";
        }
    }

    private void OnClickAccountBackBut()
    {
        ShowUI(panels[0]);

        for(int i = 0; i < inputFields.Length; ++i)
        {
            inputFields[i].text = "";
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

    private bool TryProcessUserInput(out User user)
    {
        string id = inputFields[0].text;
        string password = inputFields[1].text;

        infotext.gameObject.SetActive(true);

        if (!IsValidID(id))
        {
            infotext.text = "The ID can only contain alphanumeric characters.";
            user = null;
            return false;
        }
        else
        {
            infotext.text = "The ID is invalid. It should be between 2 and 12 characters.";
        }

        if (!IsValidPassword(password))
        {
            infotext.text = "The password must include at least one special character.";
            user = null;
            return false;
        }
        else
        {
            infotext.text = "The password is invalid. It should be between 2 and 12 characters.";
        }

        user = new User
        {
            ID = id,
            Password = password
        };

        return true;
    }

    private bool IsValidID(string id)
    {
        if (string.IsNullOrWhiteSpace(id) ||
            System.Text.RegularExpressions.Regex.IsMatch(id, "[^a-zA-Z0-9]") ||
            id.Length < 2 || id.Length > 12)
        {
            return false;
        }
        return true;
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) ||
            !System.Text.RegularExpressions.Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]") ||
            password.Length < 2 || password.Length > 12)
        {
            return false;
        }
        return true;
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
}