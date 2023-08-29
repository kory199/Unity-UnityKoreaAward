using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup_StageClear : UIBase
{
    public void OnClick_Quit()
    {
        UI_GPopupOption uI_GPopupOption = FindObjectOfType<UI_GPopupOption>();
        Popup_GamePause popup_GamePause = FindObjectOfType<Popup_GamePause>();
        UI_Enhance uI_Enhance = FindObjectOfType<UI_Enhance>();
        UI_SceneGame uI_SceneGame = FindObjectOfType<UI_SceneGame>();

        if (uI_GPopupOption != null) uI_GPopupOption.OnHide();
        if (popup_GamePause != null) popup_GamePause.OnHide();
        if (uI_Enhance != null) uI_GPopupOption.OnHide();
        if (uI_SceneGame != null) uI_SceneGame.OnHide();

        OnHide();
        GameManager.Instance.EndStage(0);
    }

    public override IProcess.NextProcess ProcessInput()
    {
        return IProcess.NextProcess.Continue;
    }
}
