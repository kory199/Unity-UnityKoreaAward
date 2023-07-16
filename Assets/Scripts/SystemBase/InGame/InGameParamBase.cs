using System.Collections.Generic;
using UnityEngine.Events;
using static EnumTypes;
public abstract class InGameParamBase
{
    protected List<UnityAction> _callbacks = null;

    public InGameParamBase()
    {
        _callbacks = new();
        RegisterCallbacks();
    }
    protected abstract void RegisterCallbacks();

    public int GetActionCount() => _callbacks.Count;
    public int GetEnumTypeNum<TEnum>(TEnum tenum) => GetEnumNumber(tenum);
    public void InvokeAll()
    {
        foreach (UnityAction action in _callbacks)
        {
            action?.Invoke();
        }
    }
    public virtual void InvokeCallBack<TEnum>(TEnum tenum)
    {
        int _index = GetEnumNumber(tenum);
        if (_index < 0 || _index >= _callbacks.Count)
        {
            return;
        }
        _callbacks[_index]?.Invoke();
    }
    #region callback 함수 관리
    public virtual void AddCallBack(int _index, UnityAction _callbackFunc)
    {
        _callbacks[_index] = _callbackFunc;
    }

    public virtual void SubCallBack(int _index, UnityAction _callbackFunc)
    {
        _callbacks[_index] -= _callbackFunc;
    }

    public virtual void AllClearCallBack()
    {
        for (int i = 0; i < _callbacks.Count; i++)
        {
            _callbacks[i] = null;
        }
    }
    #endregion
}

public class InGamePlayerParams : InGameParamBase
{
    public InGamePlayerParams() : base()
    {
    }

    protected override void RegisterCallbacks()
    {
        for (int i = 0; i < (int)EnumTypes.PlayerStateType.MAX; i++)
        {
            _callbacks.Add(null);
        }
    }

}