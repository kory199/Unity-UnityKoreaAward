using System.Collections;
using APIModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountUI : UIBase
{
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] Button createAccountBtn = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] TextMeshProUGUI infoText = null;
    [SerializeField] Button a_backBtn = null;

    private int _currentIndex = 0;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }
    protected override void Awake()
    {
        infoText.gameObject.SetActive(false);
        inputFields[1].contentType = TMP_InputField.ContentType.Password;
    }

    protected override void Start()
    {
        StartCoroutine(SetInitialFocus());
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
        if (TryProcessUserInput(out User user))
        {
            bool result = await APIManager.Instance.CreateAccountAPI(user);

            if(result)
            {
                infoText.text = $"Created New Account Successful ! {user.ID}, {user.Password}";
            }
            else
            {
                infoText.text = $"Username {user.ID} is already in use.";
            }
        }
    }

    public async void OnClickLogin()
    {
        if (TryProcessUserInput(out User user))
        {
            bool result = await APIManager.Instance.LoginAPI(user);

            if(result)
            {
                infoText.text = $"Login Successful {user.ID}, {user.Password}";
                
                //Move Scene
                GameManager.Instance.MoveScene("SceneLobby");
                OnHide();
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
        this.gameObject.SetActive(false);
    }

 
}