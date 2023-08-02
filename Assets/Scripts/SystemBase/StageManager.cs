using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class StageManager : MonoBehaviour
{
    [SerializeField] private int _stageNum = 0;
    [SerializeField] private int _spawnMeleeNum = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private int _spawnRangedNum = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private int _score = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private int _deathMonsters = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private float _time = 0; //=>��ũ���ͺ� ������Ʈ���� �о���� ������� ���濹��
    [SerializeField] private SpawnManager _spawnManager;
    #region Uinity lifeCycle
    private void Awake()
    {
        //ü�� ���
        InGameManager.Instacne.RegisterParams(EnumTypes.InGameParamType.Stage,(int)EnumTypes.StageStateType.Max);
    }
    private void Start()
    {
        _stageNum = 1;
        //start ü�� 
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMeleeMonster);
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetRangedMonster);
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Start, SetMonsterSpawn);
        //Next ü��
        InGameManager.Instacne.AddActionType(EnumTypes.InGameParamType.Stage, EnumTypes.StageStateType.Next, SetStageNum);
        //End ü�� 
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
        if (_stageNum >= 5) //5 ��� ���� �������� �ƽ��� �־������
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
    private void SetMonsterSpawn() => _spawnManager.SettingMonsterSpawnNum(_spawnMeleeNum, _spawnRangedNum);
    private async void SendStageData()
    {
        Debug.Log("Send StageEndData to Server ...");
        await APIManager.Instacne.StageUpToServer("nickname",_stageNum, _score, _time);
    }

    public void PlayerDeath()
    {
        //���� ���� �ڷ�ƾ
        StartCoroutine(Co_GameOverUI());
                
        //���� ������ ���� 
        SendStageData();

        //�� �̵�
        GameManager.Instacne.MoveScene("SceneLobby");
    }
    public void MonsterDeath()
    {
        _deathMonsters++;
        if(_deathMonsters>=(_spawnMeleeNum+_spawnRangedNum)*1*60 )
        {
            SetStageNum();
            CallStage(EnumTypes.StageStateType.Start);
            _deathMonsters = 0;
        }
    }
    public void BossDeath()
    {
        //���� Ŭ���� �ڷ�ƾ
        StartCoroutine(Co_GameOverUI());

        //���� ������ ���� 
        SendStageData();

        //�� �̵�
        GameManager.Instacne.MoveScene("SceneLobby");
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
}
public class StageDataTest : MonoSingleton<StageDataTest>
{
    public int[] MeleeMonsterNum = new int[6] { 0, 1, 1, 2, 2, 3 };
    public int[] RangedMonsterNum = new int[6] { 0, 0, 1, 1, 2, 2 };

    public int GetMeleeMonsterNum(int idx) => MeleeMonsterNum[idx];
    public int GetRangedMonsterNum(int idx) => RangedMonsterNum[idx];
}