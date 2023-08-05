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
    #region Uinity lifeCycle
    private void Awake()
    {
        // 서버로부터 몬스터 스폰 정보 요청 (임시)
        RequestMonsterInfo();
        
        //체인 등록
        InGameManager.Instance.RegisterParams(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.Max);
    }
    private void Start()
    {
        _stageNum = 1;
        //start 체인 
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonster);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetRangedMonster);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMonsterSpawn);
        //Next 체인
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);
        //End 체인 _SSH 임시 주석처리
        // InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.End, SendStageData);

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
    }
    public int GetStageNum() => _stageNum;

    // Spawn Logic Edit _SSH
    private void SetMeleeMonster() => _spawnMeleeNum = GetMonsterInfo(_stageNum, EnumTypes.MonsterType.MeleeMonster);
    private void SetRangedMonster() => _spawnRangedNum = GetMonsterInfo(_stageNum, EnumTypes.MonsterType.RangedMonster);
    private void SetMonsterSpawn()
    {
        if (_stageNum < 4)
            _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum);
        else
            _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum, true, "BossOne");
    }
    private async void SendStageData()
    {
        Debug.Log("Send StageEndData to Server ...");

        await APIManager.Instacne.StageUpToServer(_stageNum, _score);
    }

    public async void PlayerDeath()
    {
        //게임 오버 코루틴
        StartCoroutine(Co_GameOverUI());

        //서버 데이터 전달 _SSH 임시 주석처리
        // SendStageData();

        //씬 이동 : Logic edit _SSH
        await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
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
    public async void BossDeath()
    {
        //게임 클리어 코루틴
        StartCoroutine(Co_GameOverUI());

        //서버 데이터 전달 _SSH 임시 주석처리
        // SendStageData();

        //씬 이동 : Logic edit _SSH
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

    public async void RequestMonsterInfo()
    {
        await APIManager.Instance.GetMasterDataAPI();
    }

    public int GetMonsterInfo(int stageNum, EnumTypes.MonsterType monsterType)
    {
        StageSpawnMonsterData_res[] stageSpawnMonsterData_Res = APIDataSO.Instance.GetValueByKey<StageSpawnMonsterData_res[]>(APIDataDicKey.StageSpawnMonster);

        if (stageSpawnMonsterData_Res == null)
        {
            Debug.LogError("StageSpawnMonsterData_res is Null");
        }

        if (monsterType == EnumTypes.MonsterType.MeleeMonster)
        {
            return stageSpawnMonsterData_Res[stageNum].meleemonster_spawn;
        }
        else if (monsterType == EnumTypes.MonsterType.RangedMonster)
        {
            return stageSpawnMonsterData_Res[stageNum].rangedmonster_spawn;
        }
        else
        {
            Debug.Log("The monster format doesn't match");
            return 0;
        }
    }
}

// 확인 후 삭제 필요 _SSH
public class StageDataTest : MonoSingleton<StageDataTest>
{
    public int[] MeleeMonsterNum = new int[6] { 0, 1, 1, 2, 2, 3 };
    public int[] RangedMonsterNum = new int[6] { 0, 0, 1, 1, 2, 2 };

    public int GetMeleeMonsterNum(int idx) => MeleeMonsterNum[idx];
    public int GetRangedMonsterNum(int idx) => RangedMonsterNum[idx];
}
