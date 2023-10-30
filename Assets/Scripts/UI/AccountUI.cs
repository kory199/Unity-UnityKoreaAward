using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

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

    #region Unity Lifecycle
    protected override void Awake()
    {
        GetGameVersion();
        GetMonsterData();

        SoundMgr.Instance.BGMPlay(EnumTypes.StageBGMType.Title);
    }

    protected override void Start()
    {
        StartInputFocus();
    }

    private void StartInputFocus()
    {
        infoText.gameObject.SetActive(false);
        inputFields[1].contentType = TMP_InputField.ContentType.Password;

        StartCoroutine(SetInitialFocus());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickLogin();
        }
    }
    #endregion

    private void GetGameVersion()
    {
        versionText.text = "Ver : 1.1.1 ";

    }

    private void GetMonsterData()
    {

    }

    private IEnumerator SetInitialFocus()
    {
        _currentIndex = 0;
        inputFields[_currentIndex].Select();
        inputFields[_currentIndex].ActivateInputField();
        yield return null;
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

        if (TryProcessUserInput(out UserData user))
        {
            if(user.ID == null && user.Password == null)
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

        if (TryProcessUserInput(out UserData user))
        {
            if(user.ID != null && user.Password != null)
            {
                infoText.text = $"Login Successful !";
                await UniTask.Delay(TimeSpan.FromSeconds(0.5));

                OnHide();
                CreateLodingBar();
            }
            else
            {
                infoText.text = $"Incorrect username or password.";
            }
        }
    }

    private void CreateLodingBar()
    {
        UI_LodingBar uI_lodingBar = null;
        if (uI_lodingBar == null)
        {
            uI_lodingBar = UIManager.Instance.CreateObject<UI_LodingBar>("UI_LodingBar", EnumTypes.LayoutType.Middle);
        }

        uI_lodingBar.OnShow();
    }

    private bool TryProcessUserInput(out UserData user)
    {
        string id = inputFields[0].text;
        string password = inputFields[1].text;

        if (IsValidID(id) && IsValidPassword(password))
        {
            user = new UserData
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