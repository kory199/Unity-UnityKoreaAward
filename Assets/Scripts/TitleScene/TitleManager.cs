using APIModels;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("PanelObj")]
    [SerializeField] GameObject[] panels = null; // start : 0, account : 1, rank : 2

    [Header("Start")]
    [SerializeField] Button accountBut = null;
    [SerializeField] Button rankBut = null;
    [SerializeField] Button startBut = null;
    [SerializeField] Button exitBut = null;
    [SerializeField] TextMeshProUGUI loginInfotext = null;

    [Header("Account")]
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] Button createAccountBut = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] TextMeshProUGUI infotext = null;
    [SerializeField] Button a_goBackBut = null;

    [Header("Ranking")]
    [SerializeField] Button r_goBackBut = null;

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

        // === StartPanel Button Event ===
        accountBut.onClick.AddListener(OnAccountButtonClicke);
        rankBut.onClick.AddListener(delegate { ShowUI(panels[2]); });
        startBut.onClick.AddListener(GoLobbyScene);
        exitBut.onClick.AddListener(delegate { OnExitBut(); });

        // === AccountPanel Button Event ===
        a_goBackBut.onClick.AddListener(delegate { ShowUI(panels[0]); });
        createAccountBut.onClick.AddListener(delegate { OnClickCreareAccount(); });
        loginBut.onClick.AddListener(delegate { OnClickLogin(); });

        // === RankingPanel Button Event ===
        r_goBackBut.onClick.AddListener(delegate { ShowUI(panels[0]); });
    }

    private async void OnAccountButtonClicke()
    {
        ShowUI(panels[1]);
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
        foreach(GameObject panel in panels)
        {
            panel.SetActive(panel == showUIPanel);
        }
    }

    private async void GoLobbyScene()
    {
        if(APIManager.Instacne.GetApiSODicUerData() == null)
        {
            startBut.interactable = false;
            loginInfotext.gameObject.SetActive(true);
            loginInfotext.text = "Please Sign in";
        }
        else
        {
            startBut.interactable = true;
            await SceneAndUIManager.Instacne.LoadScene(EnumTypes.ScenesType.SceneLobby);
        }
    }

    private void OnExitBut()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}