using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class APIDataDic
{
    private static Dictionary<string, object> responseDataDic = new();

    public static void SetResponseData(string key, object data) => responseDataDic[key] = data;

    public static bool CheckIfKeyExists(string key)  => responseDataDic.ContainsKey(key);

    public static T GetValueByKey<T>(string key)
    {
        if(responseDataDic.ContainsKey(key))
        {
            object value = responseDataDic[key];

            if (value is T tValue)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var property in typeof(T).GetProperties())
                {
                    sb.Append($"{property.Name}: {property.GetValue(tValue)}, ");
                }

                Debug.Log(sb.ToString());
                return tValue;
            }
            else
            {
                Debug.LogError($"The object for key '{key}' is not of the expected type");
                return default;
            }
        }
        else
        {
            Debug.LogError($"The key '{key}' does not exist in the dictionary");
            return default;
        }
    }
}