using System.Collections;
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
    }

    private async void GetSategInof()
    {
        await APIManager.Instacne.GetStageAPI(_stageNum);

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
                stageBut[stageID - 1].interactable = stage.is_achieved; 
            }
        }
    }
}