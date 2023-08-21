using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GPopupOption : UIBase
{
    private void OnEnable()
    {
    }


    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    public UI_SceneLobby uI_SceneLobby = null;
    public void OnClick_GoBack()
    {
        OnHide();
        switch (GameManager.Instance.SceneState)
        {
            case SceneState.Lobby:
                uI_SceneLobby.OnShow();
                break;
            case SceneState.Game:

                break;
        }
    }
}
