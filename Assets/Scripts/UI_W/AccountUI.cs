using System.Collections;
using APIModels;
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

    private void OnEnable()
    {
        StartCoroutine(CheckTabKey());
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
            infoText.text = "The password is invalid. It should be between 2 and 12 characters and include at least one special character.";
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

            if(result == false)
            {
                infoText.text = $"Username {user.ID} is already in use.";
            }
            else
            {
                infoText.text = $"Created New Account Successful ! {user.ID}, {user.Password}";
            }
        }
    }

    public async void OnClickLogin()
    {
        if (TryProcessUserInput(out User user))
        {
            bool result = await APIManager.Instance.LoginAPI(user);

            if(result == false)
            {
                infoText.text = $"Incorrect username or password.";
            }
            else
            {
                infoText.text = $"Login Successful {user.ID}, {user.Password}";
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

    private void OnDisable()
    {
        for (int i = 0; i < inputFields.Length; ++i)
        {
            inputFields[i].text = "";
        }

        StopCoroutine(CheckTabKey());
    }
}