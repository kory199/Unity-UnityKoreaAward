using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using APIModels;

public class LobbyManager : MonoBehaviour
{
    [Header("[PanelObj]")]
    [SerializeField] GameObject[] panels = null;
    // LobbyPanel : 0, StagePanel : 1, InventoryPanel : 2, Option : 3, Lodding : 4

    [Header("[LobbyPanel]")]
    [SerializeField] Button stageBut = null;
    [SerializeField] Button inventoryBut = null;
    [SerializeField] Button OptionBut = null;
    [SerializeField] Button goTitleBut = null;

    [Header("[StagePanel]")]
    [SerializeField] Button s_goBackBut = null;
    [SerializeField] Button[] stageClickBut = null;

    [Space]
    [SerializeField] APIDataSO apidataSO = null;

    private GameObject curPanel = null;

    private void Awake()
    {
        if (apidataSO == null) apidataSO = Resources.Load<APIDataSO>("APIData");

        curPanel = panels[0];
        ShowUI(curPanel);

        stageBut.onClick.AddListener(OnClickStage);
        inventoryBut.onClick.AddListener(delegate { ShowUI(panels[2]); });
        OptionBut.onClick.AddListener(delegate { ShowUI(panels[3]); });
        goTitleBut.onClick.AddListener(delegate { GoTitleScene(); });

        stageClickBut[0].onClick.AddListener(delegate { GetStageData(); });
    }

    private void GetStageData()
    {
        List<StageInfo> stageInfoList = apidataSO.GetValueByKey<List<StageInfo>>("StageData");
    }

    private async void OnClickStage()
    {
        ShowUI(panels[1]);
        await APIManager.Instacne.GetStageAPI(0);
    }

    private void ShowUI(GameObject showUIPanel)
    {
        foreach (GameObject panel in panels)
        {
            panel.SetActive(panel == showUIPanel);
        }
    }

    private async void GoTitleScene()
    {
        await SceneAndUIManager.Instacne.LoadScene(EnumTypes.ScenesType.SceneTitle);
    }
}