using UnityEngine;

public class Popup_GamePause : UIBase
{
    UI_CountDown countDown;

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    public void OnClick_ReturnGame()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        Time.timeScale = 1;
    }

    public void OnClick_Option()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        UI_GPopupOption _gPopupOption = null;

        if (_gPopupOption == null)
        {
            _gPopupOption = UIManager.Instance.CreateObject<UI_GPopupOption>("GPopup_Options", EnumTypes.LayoutType.Global);
        }
        _gPopupOption.OnShow();
    }

    public async void Onclick_ExitGame()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        StageManager.Instance.UIScneeGameOnHide();
        await GameManager.Instance.MoveSceneWithAction(EnumTypes.ScenesType.SceneLobby, OnHide);
    }
}