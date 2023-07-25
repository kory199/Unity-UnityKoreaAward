using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "APIData", menuName = "ScriptableObjects/APIDataSO", order = 1)]
public class APIDataSO : ScriptableObject
{
    public Dictionary<string, object> responseDataDic = new();

    public void SetResponseData(string key, object data)
    {
        responseDataDic[key] = data;
    }

    public bool ChekcIfKeyExists(string key)
    {
        return responseDataDic.ContainsKey(key);
    }

    public T GetValueByKey<T>(string key)
    {
        if(responseDataDic.TryGetValue(key, out object value) && value is T tValue)
        {
            PrintValueProperties(tValue);
            return tValue;
        }

        Debug.LogError($"Error with : {key}");
        return default;
    }

    private void PrintValueProperties<T>(T value)
    {
        StringBuilder sb = new StringBuilder();

        foreach(var property in typeof(T).GetProperties())
        {
            sb.Append($"{property.Name} : {property.GetValue(value)}, ");
        }

        Debug.Log(sb.ToString());
    }
}