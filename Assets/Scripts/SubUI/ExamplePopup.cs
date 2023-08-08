using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamplePopup : MonoBehaviour, IPopupBase
{
    public Action Callback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public Action ExitCallback { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void Init(Action exitCallback = null, Action callBack = null)
    {
        if(exitCallback==null)
        {
            Callback = exitCallback;
        }
        if (Callback == null)
        {
            Callback = callBack;
        }
    }
}
