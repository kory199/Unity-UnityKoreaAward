using System.Collections.Generic;
using APIModels;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [SerializeField] Button[] stageBut = null;

    private int _stageNum = 0;

    private void Awake()
    {
        GetSategInof();

        for (int i = 0; i < stageBut.Length; i++)
        {
            int index = i + 1;
            stageBut[i].onClick.AddListener(() => GoInGameScene(index));
        }
    }

    private async void GoInGameScene(int index)
    {
        GameManager.Instance.SetStageNum(index);
        await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneInGame);
    }

    private async void GetSategInof()
    {
        await APIManager.Instance.GetStageAPI(_stageNum);

        List<StageInfo> stageInfo = APIDataSO.Instance.GetValueByKey<List<StageInfo>>(APIDataDicKey.StageData);

        foreach (Button btn in stageBut)
        {
            btn.interactable = false;
        }

        foreach (StageInfo stage in stageInfo)
        {
            Debug.Log($"stage : {stage.stage_id}");
            Debug.Log($"is_achieved : {stage.is_achieved}");

            int stageID = stage.stage_id;
            if (stageID > 0 && stageID <= stageBut.Length)
            {
                //stageBut[stageID - 1].interactable = stage.is_achieved;
                stageBut[stageID - 1].interactable = true;
            }
        }
    }
}