using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField] private int _stageNum = 0;
    [SerializeField] private int _spawnMeleeNum = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int _spawnRangedNum = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int _score = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int _deathMonsters = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private float _time = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private SpawnManager _spawnManager;
    #region Uinity lifeCycle
    private void Awake()
    {
        //체인 등록
        InGameManager.Instacne.RegisterParams(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.Max);
    }
    private void Start()
    {
        _stageNum = 1;
        //start 체인 
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonster);
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetRangedMonster);
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMonsterSpawn);
        //Next 체인
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);
        //End 체인 
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.End, SendStageData);

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Stage " + _stageNum + " Start");

            CallStage(EnumTypes.StageStateType.Start);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Next Stage Setting");

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
        if (_stageNum >= 5) //5 대신 서버 스테이지 맥스값 넣어줘야함
        {
            InGameManager.Instacne.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.End);
            return;
        }
        Debug.Log("Stage Up ...");
        _stageNum++;
    }
    public int GetStageNum() => _stageNum;
    private void SetMeleeMonster() => _spawnMeleeNum = StageDataTest.Instacne.GetMeleeMonsterNum(_stageNum);
    private void SetRangedMonster() => _spawnRangedNum = StageDataTest.Instacne.GetRangedMonsterNum(_stageNum);
    private void SetMonsterSpawn()
    {
        if (_stageNum < 4)
            _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum);
        else
            _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum,true,"BossOne");
    }
    private async void SendStageData()
    {
        Debug.Log("Send StageEndData to Server ...");
        await APIManager.Instacne.StageUpToServer("nickname", _stageNum, _score, _time);
    }

    public void PlayerDeath()
    {
        //게임 오버 코루틴
        StartCoroutine(Co_GameOverUI());

        //서버 데이터 전달 
        SendStageData();

        //씬 이동
        GameManager.Instacne.MoveScene("SceneLobby");
    }
    public void MonsterDeath()
    {
        _deathMonsters++;
        Debug.Log("DeathMonsterCount : " + _deathMonsters);
        if (_deathMonsters >= (_spawnMeleeNum + _spawnRangedNum) * 1 * 60)
        {
            SetStageNum();
            CallStage(EnumTypes.StageStateType.Start);
            _deathMonsters = 0;
            Debug.Log("stageNum : " + _stageNum);
        }
    }
    public void BossDeath()
    {
        //게임 클리어 코루틴
        StartCoroutine(Co_GameOverUI());

        //서버 데이터 전달 
        SendStageData();

        //씬 이동
        GameManager.Instacne.MoveScene("SceneLobby");
    }
    IEnumerator Co_GameOverUI()
    {
        yield return null;
        //게임 오버시 띄울 창 켜거나 이펙트 만들기
    }
    IEnumerator Co_GameClearUI()
    {
        yield return null;
        //보스 클리어시 띄울 창 켜거나 이펙트 만들기
    }
}
public class StageDataTest : MonoSingleton<StageDataTest>
{
    public int[] MeleeMonsterNum = new int[6] { 0, 1, 1, 2, 2, 3 };
    public int[] RangedMonsterNum = new int[6] { 0, 0, 1, 1, 2, 2 };

    public int GetMeleeMonsterNum(int idx) => MeleeMonsterNum[idx];
    public int GetRangedMonsterNum(int idx) => RangedMonsterNum[idx];
}