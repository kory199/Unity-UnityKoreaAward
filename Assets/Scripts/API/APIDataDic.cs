using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class APIDataDic
{
    private static Dictionary<string, object> responseDataDic = new();

    public static object GetValueByKey(string key)
    {
        responseDataDic.TryGetValue(key, out object value);
        return value;
    }

    public static void SetResponseData(string key, Dictionary<string, object> data)
    {
        responseDataDic[key] = data;
    }

    public static void DebugValueByKey(string key)
    {
        if (responseDataDic.TryGetValue(key, out object value))
        {
            Debug.Log($"Key: {key}, Value: {value}");
        }
        else
        {
            Debug.Log($"Key: {key} not found in the dictionary.");
        }
    }
}