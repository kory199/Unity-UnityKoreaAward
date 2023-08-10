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
    UI_GPopupOption gPopupOption = null;
    public void OnClick_Options()
    {
        Debug.Log("click button");
        OnHide();
        if (gPopupOption == null)
        {
            gPopupOption = UIManager.Instance.CreateObject
              <UI_GPopupOption>("GPopup_Options", EnumTypes.LayoutType.Global);
            gPopupOption.uI_SceneLobby = this;
        }
            gPopupOption.OnShow();
    }
    public void OnClick_RankingList()
    {

    }
    public void OnClick_ApplicationQuit()
    {

    }
}
