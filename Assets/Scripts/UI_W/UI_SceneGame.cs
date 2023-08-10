using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneGame : UIBase
{
    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;

    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }
    Popup_GamePause _popup_GamePause = null;
    public void OnClick_GamePause()
    {
        Time.timeScale = 0;
        if (_popup_GamePause == null)
            _popup_GamePause = UIManager.Instance.CreateObject<Popup_GamePause>("Popup_GamePause", EnumTypes.LayoutType.Middle);
        _popup_GamePause.OnShow();
    }
}
