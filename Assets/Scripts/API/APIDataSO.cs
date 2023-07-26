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

    public T GetValueByKey<T>(string key)
    {
        if(responseDataDic.TryGetValue(key, out object value) && value is T tValue)
        {
            return tValue;
        }

        Debug.LogError($"Error with : {key}");
        return default;
    }
}