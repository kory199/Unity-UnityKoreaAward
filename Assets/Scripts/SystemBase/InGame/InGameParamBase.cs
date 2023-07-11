using System.Collections.Generic;
using UnityEngine.Events;

public abstract class InGameParamBase
{
    protected List<UnityAction> _callbacks = null;

    public InGameParamBase()
    {
        _callbacks = new();
        RegisterCallbacks();
    }

    protected abstract void RegisterCallbacks();
    public virtual void InvokeCallBack(int _index)
    {
        if(_index < 0 || _index >= _callbacks.Count)
        {
            return;
        }

        _callbacks[_index]?.Invoke();
    }

    public virtual void AddCallBack(int _index, UnityAction _callbackFunc)
    {
        _callbacks[_index] += _callbackFunc;
    }

    public virtual void SubCallBack(int _index, UnityAction _callbackFunc)
    {
        _callbacks[_index] -= _callbackFunc;
    }

    public virtual void AllClearCallBack()
    {
        for(int i = 0; i < _callbacks.Count; i++)
        {
            _callbacks[i] = null;
        }
    }
}

public class InGamePlayerParams : InGameParamBase
{
    private UnityAction OnPlayerDeath = null;
    private UnityAction OnPlayerLevelUp = null;

    public InGamePlayerParams() : base()
    {
    }

    protected override void RegisterCallbacks()
    {
        _callbacks.Add(OnPlayerDeath);
        _callbacks.Add(OnPlayerLevelUp);
    }

}