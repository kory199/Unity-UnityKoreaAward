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

    [Space]
    [SerializeField] APIDataSO apidataSO = null;
    [SerializeField] PlayerBaseData playerBaseData = null;

    private int _currentIndex = 0;
    private GameObject curPanel = null;

    private void Awake()
    {
        if (apidataSO == null) apidataSO = Resources.Load<APIDataSO>("APIData");
        if (playerBaseData == null) playerBaseData = Resources.Load<PlayerBaseData>("PlayerData");

        curPanel = panels[0];
        ShowUI(curPanel);
        loginInfotext.gameObject.SetActive(false);

        GetGameVersion();

        // === StartPanel Button Event ===
        accountBut.onClick.AddListener(OnClickeAccount);
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

    private async void OnClickeAccount()
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

    private async UniTask MoveToNextInputField()
    {
        inputFields[_currentIndex].DeactivateInputField();
        _currentIndex = (_currentIndex + 1) % inputFields.Length;
        inputFields[_currentIndex].ActivateInputField();

        await UniTask.WaitUntil(() => inputFields[_currentIndex].isFocused);
    }

    private async void GetGameVersion()
    {
        versionText.text = "Ver :  " +  await APIManager.Instacne.GetGameVersionAPI(); ;
    }

    private async void OnClickCreareAccount()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instacne.CreateAccountAPI(user);
            infotext.text = $"Created New Account Successful ! {user.ID}, {user.Password}";
        }
    }

    private async void OnClickLogin()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instacne.LoginAPI(user);
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

        await APIManager.Instacne.GetRankingAPI();

        GameData userInfo = APIManager.Instacne.GetApiSODicUerData();
        if(userInfo == null)
        {
            r_infoText.gameObject.SetActive(true);
            r_infoText.text = "Please Log in";
        }

        List<RankingData> rankingDataList = apidataSO.GetValueByKey<List<RankingData>>("RankingData");

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

    private void UpdateStartButtonState()
    {
        if (apidataSO.responseDataDic.Count > 0)
        {
            startBut.interactable = true;
            loginInfotext.gameObject.SetActive(false);
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
        apidataSO.OnResponseDataChanged += UpdateStartButtonState;
    }

    private void OnDisable()
    {
        apidataSO.OnResponseDataChanged -= UpdateStartButtonState;
    }

    private async void GoLobbyScene()
    {
        if (startBut.interactable)
        {
            await SceneAndUIManager.Instacne.LoadScene(EnumTypes.ScenesType.SceneLobby);
        }
    }

    private void OnExitBut()
    {
        apidataSO.ClearResponseData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}