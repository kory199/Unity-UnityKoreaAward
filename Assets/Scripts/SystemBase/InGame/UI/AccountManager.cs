using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using APIModels;
using System.Collections.Generic;

public class AccountManager : MonoBehaviour
{
    [Header("[Account]")]
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] Button createIdBut = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] TextMeshProUGUI infotext = null;

    [Header("[Game Data]")]
    [SerializeField] Button gameDataBut = null;
    [SerializeField] Button closeGameDataBut = null;
    [SerializeField] TextMeshProUGUI playerID = null;
    [SerializeField] TextMeshProUGUI playerEXP = null;
    [SerializeField] TextMeshProUGUI playerHP = null;

    [Header("[Ranking]")]
    [SerializeField] Button rankBut = null;
    [SerializeField] Button closeRankBut = null;
    [SerializeField] TextMeshProUGUI[] RankInfo = null;

    [Header("[PanelObj]")]
    [SerializeField] GameObject accountPanel = null;
    [SerializeField] GameObject userDataPanel = null;
    [SerializeField] GameObject rankingPanel = null;

    [Header("Game Start")]
    [SerializeField] Button gameStartButton = null;

    [Header("Scriptable Objects")]
    [SerializeField] APIDataSO apidata = null;
    [SerializeField] PlayerBaseData playerBaseData = null;

    private int _currentIndex = 0;
    private GameObject curPanel = null;

    private void Awake()
    {
        // Laod Scriptabla Objects
        if (apidata == null) apidata = Resources.Load<APIDataSO>("APIData");
        if (playerBaseData == null) playerBaseData = Resources.Load<PlayerBaseData>("PlayerData");

        InitUI();

        curPanel = accountPanel;
        ShowUI(curPanel);

        // button event 
        createIdBut.onClick.AddListener(delegate { CreareID(); });
        loginBut.onClick.AddListener(delegate { Login(); });

        gameDataBut.onClick.AddListener(delegate { GameData(); ShowUI(userDataPanel); });
        rankBut.onClick.AddListener(delegate { Rank(); ShowUI(rankingPanel); });
        closeGameDataBut.onClick.AddListener(delegate { ShowUI(accountPanel); });
        closeRankBut.onClick.AddListener(delegate { ShowUI(accountPanel); });

        // GameStart Test
        gameStartButton.onClick.AddListener(async() => await SceneAndUIManager.Instacne.LoadScene(EnumTypes.ScenesType.SceneTitle));

        // Set Password Type
        inputFields[1].contentType = TMP_InputField.ContentType.Password;
    }

    private async UniTaskVoid Start()
    {
        inputFields[_currentIndex].Select();

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

    private async void CreareID()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instacne.CreateAccountAPI(user);
            infotext.text = $"새 계정 생성 완료 : {user.ID}, {user.Password}";
        }
    }

    private async void Login()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instacne.LoginAPI(user);
            infotext.text = $"로그인 완료 : {user.ID}, {user.Password}";
            gameStartButton.gameObject.SetActive(true);
        }
    }

    private async void GameData()
    {
        await APIManager.Instacne.GetGameDataAPI();

        List<PlayerData> playerDataList = apidata.GetValueByKey<List<PlayerData>>("PlayerData");

        // Separate player data
        if (playerBaseData != null && playerDataList.Count > 0)
        {
            playerBaseData.id = playerDataList[0].id;
            playerBaseData.hp = playerDataList[0].hp;
            playerBaseData.level = playerDataList[0].level;
        }

        if (playerDataList != null && playerDataList.Count > 0)
        {
            PlayerData playerData = playerDataList[0];

            // Temporary data for verification
            playerID.text = $"{playerData.id}";
            playerEXP.text = $"{playerData.exp}";
            playerHP.text = $"{playerData.hp}";
        }
        else
        {
            Debug.LogError("No player data found in APIDataSO.");
        }
    }

    private async void Rank()
    {
        await APIManager.Instacne.GetRanking();

        List<RankingData> rankingDataList = apidata.GetValueByKey<List<RankingData>>("RankingData");

        if (rankingDataList != null && rankingDataList.Count > 0)
        {
            for (int i = 0; i < rankingDataList.Count && i < RankInfo.Length - 1; i++)
            {
                RankingData rankingData = rankingDataList[i];
                RankInfo[i].text = $"ID: {rankingData.id}, Score: {rankingData.score}, Rank: {rankingData.ranking}";
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
            infotext.text = "아이디는 영문자와 숫자만 사용할 수 있습니다.";
            user = null;
            return false;
        }
        else
        {
            infotext.text = "아이디가 유효하지 않습니다. 2~12자 사이로 입력해 주세요.";
        }

        if (!IsValidPassword(password))
        {
            infotext.text = "비밀번호는 적어도 한 개의 특수 문자를 포함해야 합니다.";
            user = null;
            return false;
        }
        else
        {
            infotext.text = "비밀번호가 유효하지 않습니다. 2~12자 사이로 입력해 주세요.";
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

    private void ShowUI(GameObject targetUIObj)
    {
        if (curPanel.activeSelf)
        {
            curPanel.SetActive(false);
        }

        curPanel = targetUIObj;
        curPanel.SetActive(true);
    }

    private void InitUI()
    {
        infotext.gameObject.SetActive(false);
        userDataPanel.gameObject.SetActive(false);
        rankingPanel.gameObject.SetActive(false);
        gameStartButton.gameObject.SetActive(false);
    }
}