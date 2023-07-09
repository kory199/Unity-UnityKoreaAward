using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;



public class Loading_UIMgr : MonoBehaviour
{
    #region Singleton
    private static Loading_UIMgr instance;
    public static Loading_UIMgr Inst
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<Loading_UIMgr>();

                if(instance == null)
                {
                    instance = new GameObject(nameof(Loading_UIMgr), typeof(Loading_UIMgr)).GetComponent<Loading_UIMgr>();
                }
            }

            return instance;
        }
    }
    #endregion

    [Header("[Panel]")]
    [SerializeField] GameObject termsofUsePanel = null;
    [SerializeField] GameObject loadingPanel = null;

    [Header("[TermofUse]")]
    [SerializeField] Toggle toggleOne = null;
    [SerializeField] Toggle toggleTwo = null;
    [SerializeField] Toggle toggleThree = null;
    [SerializeField] Toggle toggleFour = null;

    [Header("[Loading Scene]")]
    [SerializeField] TextMeshProUGUI loding_version = null;
    [SerializeField] Slider slider = null;

    [Header("[Button]")]
    [SerializeField] Button selectAllAndStartBut = null;
    [SerializeField] Button startBut = null;

    public AsyncOperation asyncLoad;
    public bool isChecktermsoOfUse; // 유저 체크 했는지 안했는지 
    bool isToggleOne;
    bool isToggleTwo;
    bool isToggleThree;
    bool isToggleFour;

    TermsofUserCode termsofUserCode;

    void Awake()
    {
        SetStartBut(false);
        SetSelectAllStartBut(false);

        termsofUserCode = new TermsofUserCode();

        // === Toggle Event ===
        toggleOne.onValueChanged.AddListener(SetToggleOne);
        toggleTwo.onValueChanged.AddListener(SetToggleTwo);
        toggleTwo.onValueChanged.AddListener(SetToggleThree);
        toggleFour.onValueChanged.AddListener(SetToggleFour);

        // button event
        startBut.onClick.AddListener(delegate {  });
        selectAllAndStartBut.onClick.AddListener(delegate { });
    }

    void Start()
    {

    }

    IEnumerator LoadAsynSceneCoroutine()
    {
        asyncLoad = SceneManager.LoadSceneAsync(SceneName.mainScene);
        asyncLoad.allowSceneActivation = false;

        float time = 0f;

        while (!asyncLoad.isDone)
        {
            time += Time.time;

            slider.value = time / 10f;

            if(time >= 20)
            {
                asyncLoad.allowSceneActivation = true;
            }

            yield return null;
        }
    }


    void SetToggleOne(bool isOn)
    {
        if(isOn)
        {
            termsofUserCode.SetUseGameService((int)isCheck.check);
            isToggleOne = true;
        }
        else
        {
            termsofUserCode.SetUseGameService((int)isCheck.uncheck);
            isToggleOne = false;
        }

        print($"toggleOne : {isToggleOne}");
    }

    void SetToggleTwo(bool isOn)
    {
        if (isOn)
        {
            termsofUserCode.SetGetUserInfo((int)isCheck.check);
            isToggleTwo = true;
        }
        else
        {
            termsofUserCode.SetGetUserInfo((int)isCheck.uncheck);
            isToggleTwo = false;
        }

        print($"toggleTwo : {isToggleTwo}");
    }

    void SetToggleThree(bool isOn)
    {
        if (isOn)
        {
            termsofUserCode.SetUserGetEvent((int)isCheck.check);
            isToggleThree = true;
        }
        else
        {
            termsofUserCode.SetUserGetEvent((int)isCheck.uncheck);
            isToggleThree = false;
        }

        print($"toggleThree : {isToggleThree}");
    }

    void SetToggleFour(bool isOn)
    {
        if (isOn)
        {
            termsofUserCode.SetUserNightGetEvent((int)isCheck.check);
            isToggleFour = true;
        }
        else
        {
            termsofUserCode.SetUserNightGetEvent((int)isCheck.uncheck);
            isToggleFour = false;
        }

        print($"isToggleFour : {isToggleFour}");
    }

    void OnClickStartBut()
    {
        SetTermofUsePanel(false);
        SetLoadingPanel(true);

        if (isToggleOne == true && isToggleTwo == true)
        {
            SetStartBut(true);
        }

        if (loadingPanel.activeSelf == true)
        {
            StartCoroutine(LoadAsynSceneCoroutine());
        }
    }

    void OnCheckAllSelectToggle(bool isOn)
    {
        if(isOn)
        {
            termsofUserCode.SetUseGameService((int)isCheck.check);
            termsofUserCode.SetGetUserInfo((int)isCheck.check);
            termsofUserCode.SetUserGetEvent((int)isCheck.check);
            termsofUserCode.SetUserNightGetEvent((int)isCheck.check);

            print($"UIMgr termsofUsrCOde : {termsofUserCode.SetUseGameService((int)isCheck.check)}");
            termsofUserCode.GetUserCheck();

            SetStartBut(true);
        }
    }

    public void CheckTermsOfUsePanel()
    {
        if(isChecktermsoOfUse) // 유저가 체크 했으면 
        {
            SetTermofUsePanel(false);
        }
        else
        {
            SetTermofUsePanel(true);
        }
    }

    public void SetTermofUsePanel(bool active) => termsofUsePanel.gameObject.SetActive(active);
    void SetLoadingPanel(bool active) => loadingPanel.gameObject.SetActive(active);

    void SetStartBut(bool active) => startBut.interactable = active;
    void SetSelectAllStartBut(bool active) => selectAllAndStartBut.interactable = active;

    public void SetVerText(string ver) => loding_version.text = "Ver : " + ver;

    void FistPanelSet()
    {
        SetTermofUsePanel(false);
        SetLoadingPanel(false);
    }

}