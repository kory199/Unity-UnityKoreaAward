using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class APIDataDic
{
    private static Dictionary<string, object> responseDataDic = new();

    public static void SetResponseData(string key, object data) => responseDataDic[key] = data;

    public static bool CheckIfKeyExists(string key) => responseDataDic.ContainsKey(key);

    public static int GetCount() => responseDataDic.Count;

    public static T GetValueByKey<T>(string key)
    {
        if (responseDataDic.TryGetValue(key, out object value) && value is T tValue)
        {
            PrintValueProperties(tValue);
            return tValue;
        }

        Debug.LogError($"Error with '{key}'");
        return default;
    }

    private static void PrintValueProperties<T>(T value)
    {
        StringBuilder sb = new StringBuilder();
        foreach (var property in typeof(T).GetProperties())
        {
            sb.Append($"{property.Name}: {property.GetValue(value)}, ");
        }

        Debug.Log(sb.ToString());
    }
}

