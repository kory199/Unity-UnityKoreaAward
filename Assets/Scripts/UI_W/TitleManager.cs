using System.Collections.Generic;
using APIModels;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [Header("[PanelObj]")]
    [SerializeField] GameObject[] panels = null;
    // start : 0, account : 1, rank : 2, option : 3

    [SerializeField] Button test = null;

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
        test.onClick.AddListener(() => GetMasterData_t());

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
        if (APIDataSO.Instance.GetValueByKey<GameData>(APIDataDicKey.GameData) != null)
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

    private void OnEnable()
    {
        UpdateStartButtonState();
        APIDataSO.Instance.OnResponseDataChanged += UpdateStartButtonState;
    }

    private void OnDisable()
    {
        APIDataSO.Instance.OnResponseDataChanged -= UpdateStartButtonState;
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
        APIDataSO.Instance.ClearResponseData();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    private void GetMasterData_t()
    {
        MonsterData_res[] monsterData = APIDataSO.Instance.GetValueByKey<MonsterData_res[]>(APIDataDicKey.MeleeMonster);

        if (monsterData == null)
        {
            Debug.LogError("MonsterData list is null!");
            return;
        }

        foreach (MonsterData_res a in monsterData)
        {
            Debug.Log("MeleeMonstser) Level: " + a.level + "Exp: " + a.exp + "Hp: " + a.hp);
        }

        MonsterData_res boss = APIDataSO.Instance.GetValueByKey<MonsterData_res>(APIDataDicKey.BOSS);
        Debug.Log($"BOSS) Level: {boss.level}, Exp: {boss.exp}, Hp: {boss.hp}");

        MonsterData_res[] rangedMonsterData = APIDataSO.Instance.GetValueByKey<MonsterData_res[]>(APIDataDicKey.RangedMonster);

        if (rangedMonsterData == null)
        {
            Debug.LogError("MonsterData list is null!");
            return;
        }

        foreach (MonsterData_res a in rangedMonsterData)
        {
            Debug.Log("RangedMonster) Level: " + a.level + "Exp: " + a.exp + "Hp: " + a.hp);
        }
    }
}