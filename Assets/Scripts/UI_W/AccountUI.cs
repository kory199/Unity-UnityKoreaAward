using System;
using System.Collections;
using APIModels;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountUI : UIBase
{
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] TextMeshProUGUI infoText = null;
    [SerializeField] TextMeshProUGUI versionText = null;

    private int _currentIndex = 0;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }
    protected override void Awake()
    {
        GetGameVersion();
        GetMasterData();

        infoText.gameObject.SetActive(false);
        inputFields[1].contentType = TMP_InputField.ContentType.Password;
    }

    protected override void Start()
    {
        StartCoroutine(SetInitialFocus());
    }

    private async void GetGameVersion()
    {
        versionText.text = "Ver :  " + await APIManager.Instance.GetGameVersionAPI();
    }

    private async void GetMasterData()
    {
        await APIManager.Instance.GetMasterDataAPI();
    }

    private IEnumerator SetInitialFocus()
    {
        yield return new WaitForSeconds(0.1f); 
        inputFields[_currentIndex].Select();
        inputFields[_currentIndex].ActivateInputField();
    }

    public void ValidateID()
    {
        string id = inputFields[0].text;
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Typing);

        if (string.IsNullOrWhiteSpace(id))
        {
            infoText.text = "";
        }
        else if (IsValidID(id) == false)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "Invalid ID (1-12 alphanumeric characters only)";
        }
        else
        {
            infoText.text = "";
        }
    }

    private IEnumerator CheckTabKey()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                MoveToNextInputField();
            }

            yield return null;
        }
    }

    public void ValidatePassword()
    {
        string password = inputFields[1].text;
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Typing);

        if (string.IsNullOrWhiteSpace(password))
        {
            infoText.text = "";
        }
        else if (IsValidPassword(password) == false)
        {
            infoText.gameObject.SetActive(true);
            infoText.text = "Invalid Password (1-12 characters, at least one special)";
        }
        else
        {
            infoText.text = "";
        }
    }

    private void MoveToNextInputField()
    {
        inputFields[_currentIndex].DeactivateInputField();
        _currentIndex = (_currentIndex + 1) % inputFields.Length;
        inputFields[_currentIndex].ActivateInputField();
    }

    public async void OnClickCreateAccount()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);

        if (TryProcessUserInput(out User user))
        {
            bool result = await APIManager.Instance.CreateAccountAPI(user);

            if (result)
            {
                infoText.text = $"Created New Account Successful ! Please Login";
            }
            else
            {
                infoText.text = $"Username {user.ID} is already in use.";
            }
        }
    }

    public async void OnClickLogin()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);

        if (TryProcessUserInput(out User user))
        {
            bool loginResult = await APIManager.Instance.LoginAPI(user);

            if (loginResult)
            {
                bool gameDataResult = await APIManager.Instance.GetGameDataAPI();

                if (gameDataResult)
                {
                    infoText.text = $"Login Successful !";
                    await UniTask.Delay(TimeSpan.FromSeconds(0.5));

                    // Move Scene
                    GameManager.Instance.MoveScene("SceneLobby");
                    OnHide();
                }
                else
                {
                    infoText.text = $"Failed to load game data.";
                }
            }
            else
            {
                infoText.text = $"Incorrect username or password.";
            }
        }
    }

    private bool TryProcessUserInput(out User user)
    {
        string id = inputFields[0].text;
        string password = inputFields[1].text;

        if (IsValidID(id) && IsValidPassword(password))
        {
            user = new User
            {
                ID = id,
                Password = password
            };

            return true;
        }

        infoText.gameObject.SetActive(true);
        infoText.text = "Please correct the highlighted errors.";
        user = null;
        return false;
    }

    private bool IsValidID(string id)
    {
        if (string.IsNullOrWhiteSpace(id) ||
            System.Text.RegularExpressions.Regex.IsMatch(id, "[^a-zA-Z0-9]") ||
            id.Length < 1 || id.Length > 12)
        {
            return false;
        }

        return true;
    }

    private bool IsValidPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password) ||
            !System.Text.RegularExpressions.Regex.IsMatch(password, "[!@#$%^&*(),.?\":{}|<>]") ||
            password.Length < 1 || password.Length > 12)
        {
            return false;
        }
        return true;
    }

    private void OnEnable()
    {
        StartCoroutine(CheckTabKey());
        StartCoroutine(SetInitialFocus());
    }

    private void OnDisable()
    {
        for (int i = 0; i < inputFields.Length; ++i)
        {
            inputFields[i].text = "";
        }

        StopCoroutine(CheckTabKey());
    }

    public void OnClickBackBtn()
    {
        // TODO : 로그인 안한 상태 이면 바로 나가, 로그인 했으면 서버에게 알리기 로직 추가
        StartCoroutine(QuitGameAfterSFX());
    }

    private IEnumerator QuitGameAfterSFX()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        yield return new WaitForSeconds(SoundMgr.Instance.GetSFXLength(EnumTypes.SFXType.Button));

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}