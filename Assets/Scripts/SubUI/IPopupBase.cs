using System;

public interface IPopupBase
{
    Action Callback { get; set; }
    Action ExitCallback { get; set; }
    void Init(Action exitCallback = null, Action callBack = null);
}