using System.Collections.Generic;
using UnityEngine.Events;
using static EnumTypes;
public abstract class InGameParamBase
{
    protected List<UnityAction> _callbacks = null;

    public InGameParamBase(int num)
    {
        _callbacks = new();
        RegisterCallbacks(num);
    }
    protected abstract void RegisterCallbacks(int num);

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
    public virtual void InvokeCallBack(int index)
    {
        if (index < 0 || index >= _callbacks.Count)
        {
            return;
        }
        _callbacks[index]?.Invoke();
    }
    #region callback 함수 관리
    public virtual void AddCallBack(int index, UnityAction callbackFunc)
    {
        if (_callbacks[index] == null)
            _callbacks[index] = callbackFunc;
        else
            _callbacks[index] += callbackFunc;
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
    public InGamePlayerParams(int num) : base(num)
    {
    }

    protected override void RegisterCallbacks(int num)
    {
        for (int i = 0; i < num; i++)
        {
            _callbacks.Add(null);
        }
    }

}