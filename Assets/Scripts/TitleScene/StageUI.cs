using System.Collections;
using System.Collections.Generic;
using APIModels;
using UnityEngine;
using UnityEngine.UI;

public class StageUI : MonoBehaviour
{
    [SerializeField] Button[] stageBut = null;

    private int _stageNum = 0;

    private async void Awake()
    {
        await APIManager.Instacne.LoginAPI_TEST();
    }

    private void Start()
    {
        //GetSategInof();
    }

    private async void GetSategInof()
    {
        await APIManager.Instacne.GetStageAPI(_stageNum);


        List<StageInfo> stageInfo = APIDataSO.Instance.GetValueByKey<List<StageInfo>>(APIDataDicKey.StageData);
        if(stageInfo == null)
        {
            Debug.Log("null 이다");
        }
        foreach(StageInfo stage in stageInfo)
        {
            Debug.Log($"stage : {stage.stage_id}");
        }
    }
}