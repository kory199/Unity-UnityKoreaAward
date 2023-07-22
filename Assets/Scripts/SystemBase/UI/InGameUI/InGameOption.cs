using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InGameOption : UIBase
{
    //UI DepthControll
    private IProcess.NextProcess _optionProcess = IProcess.NextProcess.Continue;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        UIManager.Instance.processManager.processingUIStack.Push(this);
    }
    public void OnClickInGameOptionButton()
    {
        Debug.Log("Clicked InGameOptionButton");
        if (_optionProcess == IProcess.NextProcess.Continue)
        {
            _optionProcess = IProcess.NextProcess.Break;
        }
        else
        {
            _optionProcess = IProcess.NextProcess.Continue;
        }
    }
   
    public override IProcess.NextProcess ProcessInput()
    {
        return _optionProcess;
    }
}
