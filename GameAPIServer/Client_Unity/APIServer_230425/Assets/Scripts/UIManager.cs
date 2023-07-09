using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class UIManager : MonoBehaviour
{
    #region
    private static UIManager instance;
    public static UIManager Inst
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if(instance == null)
                {
                    instance = new GameObject(nameof(UIManager), typeof(UIManager)).GetComponent<UIManager>();
                }
            }
            return instance;
        }
    }
    #endregion

    GameObject curPanel = null;

    [Header("StartPanel")]
    [SerializeField] GameObject startPanel = null;
    [SerializeField] Button createAccountBut = null;
    [SerializeField] Button loginBut = null;
    [SerializeField] Button playBut = null;
    [SerializeField] Button mailBut = null;
    [SerializeField] Button itemBut = null;
    [SerializeField] Button attendanceBut = null;

    [Header("[CreateAccount]")]
    [SerializeField] GameObject createAccountPanel = null;
    [SerializeField] public TMP_InputField create_idInput = null;
    [SerializeField] public TMP_InputField create_pwInput = null;
    [SerializeField] public TextMeshProUGUI requestInfo = null;
    [SerializeField] Button createnewaccountBut = null;
    [SerializeField] Button createAccountExitBut = null;
    [SerializeField] Toggle showPW = null;

    [Header("[LoginPanel]")]
    [SerializeField] GameObject loginPanel = null;
    [SerializeField] public TMP_InputField idText = null;
    [SerializeField] public TMP_InputField pwText = null;
    [SerializeField] Button accountSaveBut = null;
    [SerializeField] Button loginExitBut = null;

    [Header("[Notice]")]
    [SerializeField] GameObject noticePanel = null;
    [SerializeField] Button noticExitBut = null;

    [Header("[MailPanel]")]
    [SerializeField] GameObject mailPanel = null;
    [SerializeField] Button mailExitBut = null;

    [Header("[AttendancePanel]")]
    [SerializeField] GameObject attendancePanel = null;
    [SerializeField] Button attendanceExitBut = null;

    [Header("[StagePanel]")]
    [SerializeField] GameObject stagePanel = null;
    [SerializeField] Button stageExitBut = null;

    [Header("[Item]")]
    [SerializeField] GameObject itemPanel = null;
    [SerializeField] Button itemExitBut = null;

    void Awake()
    {
        //FirstSetPanel();

        curPanel = startPanel;
        ShowUI(curPanel);

        // 비밀번호 제약추가
        //create_pwInput.onValidateInput += SetPasswordType;

        // 비밀번호 보기 토굴
        showPW.isOn = false;
        showPW.onValueChanged.AddListener(ShowPW);

        #region Button Event
        createAccountBut.onClick.AddListener(() => ShowUI(createAccountPanel));
        createAccountExitBut.onClick.AddListener(() => ShowUI(startPanel));
        createnewaccountBut.onClick.AddListener(() => APIManager.Inst.CreateAccount());

        playBut.onClick.AddListener(() => ShowUI(stagePanel));
        stageExitBut.onClick.AddListener(() => ShowUI(startPanel));

        loginBut.onClick.AddListener(() => ShowUI(loginPanel));
        accountSaveBut.onClick.AddListener(delegate { ShowUI(noticePanel); APIManager.Inst.StartLoginCheck(); });
        loginExitBut.onClick.AddListener(delegate { ShowUI(startPanel); });

        mailBut.onClick.AddListener(() => ShowUI(mailPanel));
        mailExitBut.onClick.AddListener(delegate { ShowUI(startPanel); });

        attendanceBut.onClick.AddListener(() => ShowUI(attendancePanel));
        attendanceExitBut.onClick.AddListener(delegate { ShowUI(startPanel); });

        itemBut.onClick.AddListener(delegate { ShowUI(itemPanel); });
        itemExitBut.onClick.AddListener(delegate { ShowUI(stagePanel); });
        #endregion
    }

    void ShowUI (GameObject targetUIObj)
    {
        if (curPanel.activeSelf) curPanel.SetActive(false);
        curPanel = targetUIObj;
        curPanel.SetActive(true);
    }

    char SetPasswordType(string text, int charIndex, char addedChar)
    {
        // 영문자와 숫자만 입력하도록 제한
        if(char.IsLetterOrDigit(addedChar))
        {
            addedChar = '\0'; // 입력된 문자를 무시 (빈 문자로 대체)
        }

        return addedChar;
    }

    void ShowPW(bool isOn)
    {
        if(isOn == true)
        {
            create_pwInput.contentType = TMP_InputField.ContentType.Standard;
        }
        else
        {
            create_pwInput.contentType = TMP_InputField.ContentType.Password;

        }
    }

    void FirstSetPanel()
    {
        createAccountPanel.gameObject.SetActive(false);
        noticePanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(false);
        mailPanel.gameObject.SetActive(false);
        attendancePanel.gameObject.SetActive(false);
        stagePanel.gameObject.SetActive(false);
        itemPanel.gameObject.SetActive(false);
    }
}