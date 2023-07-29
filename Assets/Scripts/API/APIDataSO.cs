using System.Collections.Generic;
using UnityEditor;
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

    public void RemoveKey(string key)
    {
        if (responseDataDic.ContainsKey(key))
        {
            responseDataDic.Remove(key);
            EditorUtility.SetDirty(this); // Notify Unity that this object has been modified.
        }
    }
}