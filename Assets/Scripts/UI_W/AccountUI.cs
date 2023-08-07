using APIModels;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AccountUI : MonoBehaviour
{
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] Button createAccountBtn = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] TextMeshProUGUI infoText = null;

    private int _currentIndex = 0;

    private void Awake()
    {
        infoText.gameObject.SetActive(false);
        inputFields[_currentIndex].Select();
        inputFields[1].contentType = TMP_InputField.ContentType.Password;
    }

    private void Start()
    {

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
            infoText.text = "The ID is invalid. It should be between 2 and 12 characters and contain only alphanumeric characters.";
        }
        else
        {
            infoText.text = "";
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
            infoText.text = "The password is invalid. It should be between 2 and 12 characters and include at least one special character.";
        }
        else
        {
            infoText.text = "";
        }
    }

    public void MoveToNextInputFieldOnTab()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MoveToNextInputField();
        }
    }

    private void MoveToNextInputField()
    {
        inputFields[_currentIndex].DeactivateInputField();
        _currentIndex = (_currentIndex + 1) % inputFields.Length;
        inputFields[_currentIndex].ActivateInputField();
    }

    public async void OnClickCreareAccount()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instance.CreateAccountAPI(user);
            infoText.text = $"Created New Account Successful ! {user.ID}, {user.Password}";
        }
    }

    public async void OnClickLogin()
    {
        if (TryProcessUserInput(out User user))
        {
            await APIManager.Instance.LoginAPI(user);
            infoText.text = $"Login Successful {user.ID}, {user.Password}";
        }
    }

    private bool TryProcessUserInput(out User user)
    {
        string id = inputFields[0].text;
        string password = inputFields[1].text;

        infoText.gameObject.SetActive(true);

        if (!IsValidID(id))
        {
            infoText.text = "The ID can only contain alphanumeric characters.";
            user = null;
            return false;
        }
        else
        {
            infoText.text = "The ID is invalid. It should be between 2 and 12 characters.";
        }

        if (!IsValidPassword(password))
        {
            infoText.text = "The password must include at least one special character.";
            user = null;
            return false;
        }
        else
        {
            infoText.text = "The password is invalid. It should be between 2 and 12 characters.";
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
            password.Length < 2 || password.Length > 12)
        {
            return false;
        }
        return true;
    }

    private void OnDisable()
    {
        for (int i = 0; i < inputFields.Length; ++i)
        {
            inputFields[i].text = "";
        }
    }
}