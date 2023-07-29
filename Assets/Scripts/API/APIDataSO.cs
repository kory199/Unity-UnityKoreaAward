using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "APIData", menuName = "ScriptableObjects/APIDataSO", order = 1)]
public class APIDataSO : ScriptableObject
{
    public Dictionary<string, object> responseDataDic = new();
    public List<InspectorDictionaryElement> dictionaryElements = new List<InspectorDictionaryElement>();

    public event Action OnResponseDataChanged;

    public void SetResponseData(string key, object data)
    {
        responseDataDic[key] = data;
        OnResponseDataChanged?.Invoke();
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
            OnResponseDataChanged?.Invoke();
        }
    }

    public void ClearResponseData()
    {
        responseDataDic.Clear();
        EditorUtility.SetDirty(this);
        OnResponseDataChanged?.Invoke();
    }
}

#if UNITY_EDITOR
[InitializeOnLoad]
public class PlayModeStateChanged
{
    static PlayModeStateChanged()
    {
        EditorApplication.playModeStateChanged += ModeChanged;
    }

    static void ModeChanged(PlayModeStateChange mode)
    {
        if (mode == PlayModeStateChange.EnteredEditMode)
        {
            var dataSOs = AssetDatabase.FindAssets("t:APIDataSO");
            foreach (var guid in dataSOs)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var dataSO = AssetDatabase.LoadAssetAtPath<APIDataSO>(path);
                dataSO.ClearResponseData();
            }
        }
    }
}
#endif