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
        if (responseDataDic.TryGetValue(key, out object value))
        {
            if (value is T tValue)
            {
                PrintValue(tValue);
                return tValue;
            }
            else
            {
                Debug.LogError($"Error: Value found for key '{key}', but it is not of the expected type.");
                return default;
            }
        }

        Debug.LogError($"Error: No value found for key '{key}' in the dictionary.");
        return default;
    }

    public static void PrintValue<T>(T value)
    {
        if (value is IEnumerable enumerable)
        {
            foreach (var item in enumerable)
            {
                PrintProperties(item);
            }
        }
        else
        {
            PrintProperties(value);
        }
    }

    private static void PrintProperties(object obj)
    {
        var properties = obj.GetType().GetProperties();
        StringBuilder sb = new StringBuilder();

        foreach (var prop in properties)
        {
            var propName = prop.Name;
            var propValue = prop.GetValue(obj, null);
            sb.Append($"{propName}: {propValue}, ");
        }

        // Remove the trailing comma and space, then print the entire line
        if (sb.Length > 2)
        {
            sb.Length -= 2;
        }

        Debug.Log(sb.ToString());
    }
}

