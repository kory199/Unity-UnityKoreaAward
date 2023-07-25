using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[SerializeField]
public class User
{
    public string ID;
    public string Password;
}

public class AccountManager : MonoBehaviour
{
    [Header("[Account]")]
    [SerializeField] TMP_InputField[] inputFields = null;
    [SerializeField] Button createIdBut = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] TextMeshProUGUI infotext = null;

    private int _currentIndex = 0;

    private void Awake()
    {
        infotext.gameObject.SetActive(false);

        // button event 
        createIdBut.onClick.AddListener(delegate { CreareID(); });
        loginBut.onClick.AddListener(delegate { Login(); });

        // Set Password Type
        inputFields[1].contentType = TMP_InputField.ContentType.Password;
    }

    private async UniTaskVoid Start()
    {
        inputFields[_currentIndex].Select();

        while(true)
        {
            if(Input.GetKeyDown(KeyCode.Tab))
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
        if(TryProcessUserInput(out User user))
        {
            await APIManager.Instacne.CreateAccpuntAPI(user);
            infotext.text = $"새 계정 생성 완료 : {user.ID}, {user.Password}";
        }
    }

    private async void Login()
    {
        if(TryProcessUserInput(out User user))
        {
            await APIManager.Instacne.LoginAPI(user);
            infotext.text = $"로그인 완료 : {user.ID}, {user.Password}";
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
}