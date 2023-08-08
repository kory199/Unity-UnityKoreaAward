using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_GPopupOption : UIBase
{
    private void OnEnable()
    {
        OnShow();
    }


    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }


    public void OnClick_GoBack()
    {
        switch (GameManager.Instance.sceneState)
        {
            case SceneState.Lobby:
                GameObject lobby = UIManager.Instance.CreateObject<GameObject>("UI_SceneLobby", EnumTypes.LayoutType.First);
                break;
            case SceneState.Game:
                break;
        }
        OnHide();
    }
}
