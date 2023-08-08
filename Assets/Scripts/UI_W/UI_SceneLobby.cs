using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneLobby : UIBase
{
    private string _basePath = "UI/";
    private void OnEnable()
    {
        OnShow();
        GameManager.Instance.sceneState = SceneState.Lobby;
    }


    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    public void OnClick_GameStart()
    {
        GameManager.Instance.MoveScene("SceneInGame");
        GameManager.Instance.sceneState = SceneState.Game;
        OnHide();
        Destroy(this);
    }
    public void OnClick_Explane()
    {

    }
    public void OnClick_UnlockList()
    {

    }
    public void OnClick_Options()
    {
        UIManager.Instance.CreateObject<GameObject>("GPopup_Options", EnumTypes.LayoutType.Global);
        OnHide();
        Destroy(this);
    }
    public void OnClick_RankingList()
    {

    }
    public void OnClick_ApplicationQuit()
    {

    }
}
