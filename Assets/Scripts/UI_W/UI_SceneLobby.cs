using System;
using System.Collections;
using System.Collections.Generic;
using APIModels;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UI_SceneLobby : UIBase
{
    private string _basePath = "UI/";
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
        await UniTask.Delay(TimeSpan.FromSeconds(2));
        bool result = await APIManager.Instance.GetStageAPI();
    }

    public async void OnClick_GameStart()
    {
        bool playGameTask = await APIManager.Instance.PlayGameAPI();

        if (playGameTask)
        {
            GameManager.Instance.MoveScene("SceneInGame");
            GameManager.Instance.SceneState = SceneState.Game;

            OnHide();

            // LodingBar Clon Method 
            // 사용 방법 : 위에 OnHide() 위 코드 두줄 주석 처리 후, 아래 메서드 주석 풀어주기!
            //CreateLodingBar();
        }
    }

    // Canvas는 임시로 First 로 해둠, 변경 편하게 해주세요 
    private void CreateLodingBar()
    {
        UI_LodingBar uI_lodingBar = null;
        if(uI_lodingBar == null)
        {
            uI_lodingBar = UIManager.Instance.CreateObject
                <UI_LodingBar>("UI_LodingBar", EnumTypes.LayoutType.First);
        }

        uI_lodingBar.OnShow();
    }

    public void OnClick_Explane()
    {

    }
    public void OnClick_UnlockList()
    {

    }
    UI_GPopupOption _gPopupOption = null;
    public void OnClick_Options()
    {
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
        OnHide();
        if (_rankUi == null)
        {
            _rankUi = UIManager.Instance.CreateObject
              <RankUI>("Popup_Ranking", EnumTypes.LayoutType.Middle);
            _rankUi.uI_SceneLobby = this;
        }
        _rankUi.OnShow();
    }
    public void OnClick_ApplicationQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }

    public async void OnClick_LogOut()
    {
        bool result = await APIManager.Instance.LogOutAPI();
        if(result)
        {
            GameManager.Instance.MoveScene("SceneTitle");
            OnHide();
        }
    }
}