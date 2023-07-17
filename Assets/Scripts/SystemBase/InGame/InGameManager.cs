using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static EnumTypes;

/// <summary>
/// 환경 변수와 콜백 함수 사용 예시
/// 변경 사용 예시  콜백 함수
///_parameters[EnumTypes.InGameParamType.Player].InvokeCallBack(EnumTypes.PlayerSkiils.DoubleShot);
/// </summary>
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
    /// 사용예시
    /// ex) 게임 변수, 콜백 함수 이름, 콜백 함수 
    ///     AddActionType(EnumTypes.InGameParamType.Player, PlayerSkiils.DoubleShot, () => Debug.Log("더블샷"));
    /// </summary>
    /// <typeparam name="TEnum">Enum타입</typeparam>
    /// <param name="param">게임 변수</param>
    /// <param name="actionType">콜백 함수 타입</param>
    /// <param name="action">콜백 함수</param>
    public void AddActionType<TEnum>(EnumTypes.InGameParamType param, TEnum actionType, UnityAction action)
    {
        int idx = GetEnumNumber(actionType);
        _parameters[param].AddCallBack(idx, action);
    }
    public void AddActionType<TEnum>(EnumTypes.InGameParamType param, int idx, UnityAction action)
    {
        _parameters[param].AddCallBack(idx, action);
    }
}
