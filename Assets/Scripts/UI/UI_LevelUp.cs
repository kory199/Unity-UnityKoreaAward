using Cysharp.Threading.Tasks;
using UnityEngine;
using System;

public class UI_LevelUp : UIBase
{
    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
        OnShowTime();
    }

    private async void OnShowTime()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(0.5));
        OnHide();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }
}