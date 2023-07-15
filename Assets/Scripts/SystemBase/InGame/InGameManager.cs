using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumTypes;
public class InGameManager : MonoSingleton<InGameManager>
{
    private Dictionary<EnumTypes.InGameParamType, InGameParamBase> _parameters = new();

    private void Awake()
    {
        RegisterParams();
    }

    private void RegisterParams()
    {
        _parameters.Add(EnumTypes.InGameParamType.Player, new InGamePlayerParams());
    }
    public void InvokeCallBacks(EnumTypes.InGameParamType type, int callBackIndex)
    {
        InGameParamBase param = null;

        switch (type)
        {
            case EnumTypes.InGameParamType.Player:
                if (_parameters.TryGetValue(type, out param) == false)
                {
                    return;
                }
                InGamePlayerParams players = param as InGamePlayerParams;
                players.InvokeCallBack(callBackIndex);
                break;
        }
    }
    /// <summary>
    /// 게임 변수 타입에 맞춰 action 지정
    /// </summary>
    /// <param name="type"></param>
    /// <param name="action"></param>
    public void AddActionType<TEnum>(EnumTypes.InGameParamType param, TEnum actionType, UnityAction action)
    {
        int idx = GetEnumNumber(actionType);
        _parameters[param].AddCallBack(idx, action);
    }
    private void Start()
    {
        AddActionType(EnumTypes.InGameParamType.Player, AAAA.a, () => Debug.Log("더블샷"));
        _parameters[EnumTypes.InGameParamType.Player].InvokeCallBack(EnumTypes.PlayerSkiils.DoubleShot);
    }
}
