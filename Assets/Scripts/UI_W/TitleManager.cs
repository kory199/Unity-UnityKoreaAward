using APIModels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("[PanelObj]")]
    [SerializeField] GameObject[] panels = null;
    // start : 0, account : 1, rank : 2, option : 3

    [Header("[Start]")]
    [SerializeField] Button accountBut = null;
    [SerializeField] Button rankBut = null;
    [SerializeField] Button startBut = null;
    [SerializeField] Button optionBut = null;
    [SerializeField] Button exitBut = null;
    [SerializeField] TextMeshProUGUI loginInfotext = null;
    [SerializeField] TextMeshProUGUI versionText = null;

    [Header("Option")]
    [SerializeField] Button o_goBackBut = null;

    private GameObject curPanel = null;

    private void Awake()
    {
        panels[0].gameObject.SetActive(true);
        loginInfotext.gameObject.SetActive(false);

        GetGameVersion();
        GetMasterData();

        // === StartPanel Button Event ===
        accountBut.onClick.AddListener(OnClickAccount);
        rankBut.onClick.AddListener(OnClickRank);
        startBut.onClick.AddListener(GoLobbyScene);
        optionBut.onClick.AddListener(delegate { ShowUI(panels[3]);});
        exitBut.onClick.AddListener(delegate { OnExitBut(); });

        // === OptionPanel Button Event ===
        o_goBackBut.onClick.AddListener(delegate { ShowUI(panels[0]); });
    }

    private void OnClickAccount() => ShowUI(panels[1]);
    private void OnClickRank() => ShowUI(panels[2]);

    private async void GetGameVersion()
    {
        versionText.text = "Ver :  " +  await APIManager.Instance.GetGameVersionAPI(); ;
    }

    private async void GetMasterData()
    {
        await APIManager.Instance.GetMasterDataAPI();
    }

    private void ShowUI(GameObject showUIPanel)
    {
        showUIPanel.SetActive(true);
    }

    private void UpdateStartButtonState()
    {

        if (APIManager.Instance.IsLogin == true)
        {
            startBut.interactable = true;
            rankBut.interactable = true;
            loginInfotext.gameObject.SetActive(false);
            //await APIManager.Instance.GetGameDataAPI();
        }
        else
        {
            startBut.interactable = false;
            rankBut.interactable = false;
            loginInfotext.gameObject.SetActive(true);
            loginInfotext.text = "Please Log in";
        }
    }

    private async void GoLobbyScene()
    {
        if (startBut.interactable)
        {
            await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
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