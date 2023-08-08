using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SceneLobby : UIBase
{
    private string _basePath = "UI/";
    private void Awake()
    {
        OnShow();
    }


    IProcess.NextProcess _nextProcess = IProcess.NextProcess.Continue;
    public override IProcess.NextProcess ProcessInput()
    {
        return _nextProcess;
    }

    public void OnClick_GameStart()
    {

    }
    public void OnClick_Explane()
    {

    }
    public void OnClick_UnlockList()
    {

    }
    public void OnClick_Options()
    {
        string path = string.Concat(_basePath, "GPopup_Options");
        GameObject option = Resources.Load<GameObject>(path);
        Instantiate(option);
    }
    public void OnClick_RankingList()
    {

    }
    public void OnClick_ApplicationQuit()
    {

    }
}
