using System.Collections;
using APIModels;
using UnityEngine;

public class StageManager : MonoSingleton<StageManager>
{
    [SerializeField] private int _spawnMeleeNum = 0; 
    [SerializeField] private int _spawnRangedNum = 0; 
    [SerializeField] private int _deathMonsters = 0;
    [SerializeField] private float _time = 0;
    [SerializeField] private SpawnManager _spawnManager;
    private UI_SceneGame _uI_SceneGame;
    private UI_Enhance _uI_Enhance;

    private int _stageNum;
    private int _score;
    private bool playGame;

    #region Uinity lifeCycle
    private void Awake()
    {
        // 플레이어 UI 켜기
        if (_uI_SceneGame == null)
            _uI_SceneGame = UIManager.Instance.CreateObject<UI_SceneGame>("UI_SceneGame", EnumTypes.LayoutType.First);
        _uI_SceneGame.OnShow();

        ChangedStatusToServer();
        ServerDataSet();
        CallCountDown();
        InitUI_Enhance();
        playGame = true;

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

        _uI_SceneGame.SetStageNum(_stageNum);
        _uI_SceneGame.SetLevel(_stageNum);
        _uI_SceneGame.SetScore(_score);

        StartCoroutine(Co_GameStart());
        PlayBGMForStage(_stageNum);
    }
    #endregion

    private async void ChangedStatusToServer()
    {
        bool result = await APIManager.Instance.PlayGameAPI();
        if(result == false)
        {
            Debug.LogWarning($"Status Changed Fail");
        }    
    }

    private void ServerDataSet()
    {
        _stageNum = GameManager.Instance.GetStageNum();
        _score = GameManager.Instance.playerData.score;
    }

    IEnumerator Co_GameStart()
    {
        yield return new WaitForSeconds(3f);
        CallStage(EnumTypes.StageStateType.Start);
    }

    private void CallCountDown()
    {
        UI_CountDown countDown = UIManager.Instance.CreateObject<UI_CountDown>("UI_CountDown", EnumTypes.LayoutType.Middle);
        countDown.OnShow();
    }

    public void CallStage(EnumTypes.StageStateType stageType)
    {
        InGameManager.Instance.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)stageType);
    }

    public void SetStageNum()
    {
        if (_stageNum >= 5)
        {
            InGameManager.Instance.InvokeCallBacks(EnumTypes.InGameParamType.Stage, (int)EnumTypes.StageStateType.End);
            return;
        }

        Debug.Log("Stage Up ...");

        _uI_Enhance.GetSkillPoint(_stageNum);
        _uI_Enhance.OnShow();

        if(playGame)
        {
            SendStageData();
        }

        PlayBGMForStage(_stageNum);

        //_uI_SceneGame.SetStageNum(_stageNum);
        //_uI_SceneGame.SetLevel(_stageNum);
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

        _score = GameManager.Instance.playerData.score;
        Debug.Log($"_score {_score}, _stageNum {_stageNum}");
        bool result = await APIManager.Instance.StageUpToServer(_stageNum, _score);

        if (result)
        {
            _stageNum++;
            _uI_SceneGame.SetStageNum(_stageNum);
            _uI_SceneGame.SetLevel(_stageNum);
        }
    }

    public void PlayerDeath()
    {
        //게임 오버 코루틴
        StartCoroutine(Co_GameOverUI());
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

    public void BossDeath()
    {
        //게임 클리어 코루틴
        StartCoroutine(Co_GameOverUI());
    }

    IEnumerator Co_GameOverUI()
    {
        playGame = false;
        _uI_SceneGame.OnHide();

        Popup_StageClear popup_StageFail = UIManager.Instance.CreateObject<Popup_StageClear>("Popup_StageFail", EnumTypes.LayoutType.Middle);
        popup_StageFail.OnShow();
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.StageFile);
        yield return new WaitForSeconds(1f);
        popup_StageFail.OnHide();
        //yield return null;
        //게임 오버시 띄울 창 켜거나 이펙트 만들기

        //서버 데이터 전달
        //SendStageData();

        MoveToLobby();
    }

    private async void MoveToLobby()
    {   
        await GameManager.Instance.MoveSceneWithAction(EnumTypes.ScenesType.SceneLobby, GetStageAPINum);
    }

    private async void GetStageAPINum()
    {
        await APIManager.Instance.GetStageAPI();
    }

    IEnumerator Co_GameClearUI()
    {
        _uI_SceneGame.OnHide();
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

    private void PlayBGMForStage(int stage)
    {
        EnumTypes.StageBGMType bgmType;
        switch (stage)
        {
            case 1: bgmType = EnumTypes.StageBGMType.Stage1; break;
            case 2: bgmType = EnumTypes.StageBGMType.Stage2; break;
            case 3: bgmType = EnumTypes.StageBGMType.Stage3; break;
            case 4: bgmType = EnumTypes.StageBGMType.Stage4; break;
            case 5: bgmType = EnumTypes.StageBGMType.Stage5; break;
            default: return;
        }

        SoundMgr.Instance.BGMPlay(bgmType);
    }

    public void UIScneeGameOnHide() => _uI_SceneGame.OnHide();

    private void InitUI_Enhance()
    {
        _uI_Enhance = UIManager.Instance.CreateObject<UI_Enhance>("UI_Enhance", EnumTypes.LayoutType.Middle);
        _uI_Enhance.OnHide();
    }
}