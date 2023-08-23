using System;
using APIModels;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UI_SceneLobby : UIBase
{
    //private string _basePath = "UI/";
    private void OnEnable()
    {
        GameManager.Instance.SceneState = SceneState.Lobby;
    }

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    protected override void Awake()
    {
        DevelopmentModeLogin();
        SoundMgr.Instance.BGMPlay(EnumTypes.StageBGMType.Lobby);
    }

    private async void DevelopmentModeLogin()
    {
        bool MasterDataResult = await APIManager.Instance.GetMasterDataAPI();

        if(MasterDataResult)
        {
            User devUser = new User
            {
                ID = "Wally3",
                Password = "1234!"
            };

            bool loginResult = await APIManager.Instance.LoginAPI(devUser);

            if (loginResult)
            {
                bool gameDataResult = await APIManager.Instance.GetGameDataAPI();
                if (gameDataResult)
                {
                    Debug.Log("Login Successful!");
                }
            }

            GetStageNum();
        }
    }

    private async void GetStageNum()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        bool result = await APIManager.Instance.GetStageAPI();
    }

    public async void OnClick_GameStart()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        bool playGameTask = await APIManager.Instance.PlayGameAPI();

        if (playGameTask)
        {
            OnHide();
            GameManager.Instance.MoveScene("SceneInGame");
            GameManager.Instance.SceneState = SceneState.Game;
        }
    }

    public void OnClick_Explane()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
    }
    public void OnClick_UnlockList()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
    }
    UI_GPopupOption _gPopupOption = null;
    public void OnClick_Options()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        if (_gPopupOption == null)
        {
            _gPopupOption = UIManager.Instance.CreateObject
              <UI_GPopupOption>("GPopup_Options", EnumTypes.LayoutType.Global);
            _gPopupOption.uI_SceneLobby = this;
        }
        _gPopupOption.OnShow();
    }
    RankUI _rankUi = null;
    public void OnClick_RankingList()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        if (_rankUi == null)
        {
            _rankUi = UIManager.Instance.CreateObject
              <RankUI>("Popup_Ranking", EnumTypes.LayoutType.Middle);
            _rankUi.uI_SceneLobby = this;
        }
        _rankUi.OnShow();
    }
    public async void OnClick_ApplicationQuit()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        bool result = await APIManager.Instance.LogOutAPI();
        if(result)
        {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }
    }

    public async void OnClick_LogOut()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        bool result = await APIManager.Instance.LogOutAPI();
        if(result)
        {
            GameManager.Instance.MoveScene("SceneTitle");
            OnHide();
        }
    }
}