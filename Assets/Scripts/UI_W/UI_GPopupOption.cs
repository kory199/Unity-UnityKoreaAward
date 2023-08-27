using UnityEngine;
using UnityEngine.UI;

public class UI_GPopupOption : UIBase
{
    [SerializeField] Slider masterSlider = null;
    [SerializeField] Slider bgmSlider = null;
    [SerializeField] Slider sfxSlider = null;

    private void OnEnable()
    {
        masterSlider.onValueChanged.AddListener(OnChangedMasterControl);
        bgmSlider.onValueChanged.AddListener(OnChangedBGMControl);
        sfxSlider.onValueChanged.AddListener(OnChangedSFXControl);
    }

    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    public UI_SceneLobby uI_SceneLobby = null;
    public void OnClick_GoBack()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        Time.timeScale = 1;
        switch (GameManager.Instance.SceneState)
        {
            case SceneState.Lobby:
                uI_SceneLobby.OnShow();
                break;
            case SceneState.Game:

                break;
        }
    }

    private void OnChangedMasterControl(float sound)
    {
        SoundMgr.Instance.MasterControl(masterSlider);
    }

    private void OnChangedBGMControl(float sound)
    {
        SoundMgr.Instance.BGMControl(bgmSlider);
    }

    private void OnChangedSFXControl(float sound)
    {
        SoundMgr.Instance.SFXControl(sfxSlider);
    }
}