using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UI_SceneLobby : UIBase
{
    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    protected override void Awake()
    {
        SoundMgr.Instance.BGMPlay(EnumTypes.StageBGMType.Lobby);
    }

    public async void OnClick_GameStart()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        await GameManager.Instance.MoveSceneWithAction(EnumTypes.ScenesType.SceneInGame, OnHide);
        GameManager.Instance.SceneState = SceneState.Game;
    }

    public void OnClick_Explane()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
    }

    public void OnClick_UnlockList()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
    }

    UI_GPopupOption _gPopupOption = null;
    public void OnClick_Options()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        if (_gPopupOption == null)
        {
            _gPopupOption = UIManager.Instance.CreateObject
              <UI_GPopupOption>("GPopup_Options", EnumTypes.LayoutType.Global);
            _gPopupOption.uI_SceneLobby = this;
        }
        _gPopupOption.OnShow();
    }

    RankUI _rankUi = null;
    public void OnClick_RankingList()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        OnHide();
        if (_rankUi == null)
        {
            _rankUi = UIManager.Instance.CreateObject
              <RankUI>("Popup_Ranking", EnumTypes.LayoutType.Middle);
            _rankUi.uI_SceneLobby = this;
        }
        _rankUi.OnShow();
    }

    private void OnEnable()
    {
        GameManager.Instance.SceneState = SceneState.Lobby;
    }

    public void OnClick_ApplicationQuit()
    {
        SoundMgr.Instance.SFXPlay(EnumTypes.SFXType.Button);
        //bool result = await APIManager.Instance.LogOutAPI();
        //if(result)
        //{
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}