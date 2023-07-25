using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StageManager : MonoBehaviour
{
    [SerializeField] private int stageNum = 0;
    [SerializeField] private int spawnMeleeNum = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int spawnRangedNum = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정

    #region Uinity lifeCycle
    private void Awake()
    {
        //체인 등록
        InGameManager.Instacne.RegisterParams(EnumTypes.InGameParamType.Stage,(int)EnumTypes.StageStateType.Max);

    }
    private void Start()
    {
        //start 체인 
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonster);
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetRangedMonster);
        //Next 체인
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);
        //End 체인 
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.End, SendStageData);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space");

            stageNum++;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("a");

            CallStage(EnumTypes.StageStateType.Start);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("s");

            CallStage(EnumTypes.StageStateType.Next);
        }
    }
    #endregion
    public void CallStage(EnumTypes.StageStateType stageType)
    {
        InGameManager.Instacne.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)stageType);
    }
    public void SetStageNum()
    {
        if (stageNum >= 5) //5 대신 서버 스테이지 맥스값 넣어줘야함
        {
            InGameManager.Instacne.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.End);
            return;
        }
        Debug.Log("Stage Up ...");
        stageNum++;
    }
    public int GetStageNum() => stageNum;
    private void SetMeleeMonster() => spawnMeleeNum = StageDataTest.Instacne.GetMeleeMonsterNum(stageNum);
    private void SetRangedMonster() => spawnRangedNum = StageDataTest.Instacne.GetRangedMonsterNum(stageNum);
    private void SendStageData() => Debug.Log("Send StageEndData to Server ...");
}
public class StageDataTest : MonoSingleton<StageDataTest>
{
    public int[] MeleeMonsterNum = new int[6] { 0, 1, 1, 2, 2, 3 };
    public int[] RangedMonsterNum = new int[6] { 0, 0, 1, 1, 2, 2 };

    public int GetMeleeMonsterNum(int idx) => MeleeMonsterNum[idx];
    public int GetRangedMonsterNum(int idx) => RangedMonsterNum[idx];
}
