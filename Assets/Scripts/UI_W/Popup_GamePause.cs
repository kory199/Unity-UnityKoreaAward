using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_GamePause : UIBase
{
    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }
    public void OnClick_ReturnGame()
    {
        OnHide();
        Time.timeScale = 1;
    }
}
