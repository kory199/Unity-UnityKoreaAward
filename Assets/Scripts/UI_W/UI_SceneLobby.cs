using System.Collections;
using System.Collections.Generic;
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

    public void OnClick_GameStart()
    {
        GameManager.Instance.MoveScene("SceneInGame");
        GameManager.Instance.SceneState = SceneState.Game;

        OnHide();
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