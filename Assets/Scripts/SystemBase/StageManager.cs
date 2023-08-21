using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using APIModels;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField] private int _stageNum = 0;
    [SerializeField] private int _spawnMeleeNum = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int _spawnRangedNum = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int _score = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private int _deathMonsters = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private float _time = 0; //=>스크립터블 오브젝트에서 읽어오는 방식으로 변경예정
    [SerializeField] private SpawnManager _spawnManager;
    [SerializeField] private UI_SceneGame _uI_SceneGame;
    private int _onclickNum;

    #region Uinity lifeCycle
    private void Awake()
    {
        // 플레이어 UI 켜기
        if (_uI_SceneGame == null)
            _uI_SceneGame = UIManager.Instance.CreateObject<UI_SceneGame>("UI_SceneGame", EnumTypes.LayoutType.First);
        _uI_SceneGame.OnShow();
        //체인 등록
        InGameManager.Instance.RegisterParams(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.Max);
    }
    private void Start()
    {
        //start 체인
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMonsterSpawnum);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMonsterSpawn);

        //Next 체인
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);

        //End 체인
        //InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.End, SendStageData);
        StartCoroutine(Co_GameStart());
    }
    IEnumerator Co_GameStart()
    {
        yield return new WaitForSeconds(3f);
        CallStage(EnumTypes.StageStateType.Start);
    }
    #endregion
    public void CallStage(EnumTypes.StageStateType stageType)
    {
        InGameManager.Instance.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)stageType);
    }
    public void SetStageNum()
    {
        if (_stageNum >= 5) //5 대신 서버 스테이지 맥스값 넣어줘야함
        {
            InGameManager.Instance.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.End);
            return;
        }
        Debug.Log("Stage Up ...");
        _stageNum++;
        // _uI_SceneGame.SetStageNum(_stageNum);
        SendStageData();
    }
    public int GetStageNum() => _stageNum;

    // Spawn Logic Edit
    private void SetMonsterSpawnum() => GetMonsterInfo(_stageNum);
    private void SetMonsterSpawn()
    {
        if (_stageNum < 4)
            _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum);
        else
            _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum, true, "BossOne");
    }
    public async void StageClear()
    {
        UIManager.Instance.CreateObject<Popup_StageClear>("Popup_StageClear", EnumTypes.LayoutType.Middle);

        bool result = await APIManager.Instance.LogOutAPI();
        if (result == false)
        {
            Debug.LogWarning("StageClear to Server Fail");
        }
    }
    private async void SendStageData()
    {
        Debug.Log("Send StageEndData to Server ...");

        _score = 777;
        //  _score = GameManager.Instance.playerData.score;
        await APIManager.Instance.StageUpToServer(_stageNum, _score);
    }

    public async void PlayerDeath()
    {
        //게임 오버 코루틴
        StartCoroutine(Co_GameOverUI());

        //서버 데이터 전달
        SendStageData();

        //씬 이동 : Logic edit 
        await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
    }
    public void MonsterDeath()
    {
        //_deathMonsters++;
        _deathMonsters += 10;

        //  Debug.Log("DeathMonsterCount : " + _deathMonsters);
        if (_deathMonsters >= (_spawnMeleeNum + _spawnRangedNum) * 1 * 60)
        {
            SetStageNum();


            CallStage(EnumTypes.StageStateType.Start);
            _deathMonsters = 0;
            // Debug.Log("stageNum : " + _stageNum);
        }
    }

    public async void BossDeath()
    {
        //게임 클리어 코루틴
        StartCoroutine(Co_GameOverUI());

        //서버 데이터 전달 
        //SendStageData();

        //씬 이동 : Logic edit 
        await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
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

    public void GetMonsterInfo(int stageNum)
    {
        StageSpawnMonsterData_res[] stageSpawnMonsterData_Res = APIManager.Instance.GetValueByKey<StageSpawnMonsterData_res[]>(MasterDataDicKey.StageSpawnMonster.ToString());

        if (stageSpawnMonsterData_Res == null)
        {
            Debug.LogError("StageSpawnMonsterData_res is Null");
        }

        _spawnMeleeNum = stageSpawnMonsterData_Res[stageNum].meleemonster_spawn;
        _spawnRangedNum = stageSpawnMonsterData_Res[stageNum].rangedmonster_spawn;
    }
}