using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using APIModels;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField] private int _stageNum = 0;
    [SerializeField] private int _spawnMeleeNum = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private int _spawnRangedNum = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private int _score = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private int _deathMonsters = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private float _time = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private SpawnManager _spawnManager;

    private int _onclickNum;

    #region Uinity lifeCycle
    private void Awake()
    {
        // �����κ��� ���� ���� ���� ��û (�ӽ�)
        RequestMonsterInfo();

        //ü�� ���
        InGameManager.Instance.RegisterParams(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.Max);
    }
    private void Start()
    {
        _stageNum = 1;
        //start ü�� 
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonster);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetRangedMonster);
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMonsterSpawn);
        //Next ü��
        InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);

        //End ü�� 
        //InGameManager.Instance.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.End, SendStageData);
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
        if (_stageNum >= 5) //5 ��� ���� �������� �ƽ��� �־������
        {
            InGameManager.Instance.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.End);
            return;
        }
        Debug.Log("Stage Up ...");
        _stageNum++;
    }
    public int GetStageNum() => _stageNum;

    // Spawn Logic Edit
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

        //await APIManager.Instance.StageUpToServer(_stageNum, _score);
    }

    public async void PlayerDeath()
    {
        //���� ���� �ڷ�ƾ
        StartCoroutine(Co_GameOverUI());

        //���� ������ ���� 
        SendStageData();

        //�� �̵� : Logic edit 
        await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
    }
    public void MonsterDeath()
    {
        _deathMonsters++;

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
        //���� Ŭ���� �ڷ�ƾ
        StartCoroutine(Co_GameOverUI());

        //���� ������ ���� 
        //SendStageData();

        //�� �̵� : Logic edit 
        await GameManager.Instance.LoadScene(EnumTypes.ScenesType.SceneLobby);
    }
    IEnumerator Co_GameOverUI()
    {
        yield return null;
        //���� ������ ��� â �Ѱų� ����Ʈ �����
    }
    IEnumerator Co_GameClearUI()
    {
        yield return null;
        //���� Ŭ����� ��� â �Ѱų� ����Ʈ �����
    }

    public async void RequestMonsterInfo()
    {
        await APIManager.Instance.GetMasterDataAPI();
    }

    public int GetMonsterInfo(int stageNum, EnumTypes.MonsterType monsterType)
    {
        StageSpawnMonsterData_res[] stageSpawnMonsterData_Res = APIManager.Instance.GetValueByKey<StageSpawnMonsterData_res[]>(MasterDataDicKey.StageSpawnMonster.ToString());

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
            Debug.LogError("The monster format doesn't match");
            return 0;
        }
    }
}