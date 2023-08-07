using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "APIData", menuName = "ScriptableObjects/APIDataSO", order = 1)]
public class APIDataSO : ScriptableObject
{
    private static APIDataSO instance;

    public static APIDataSO Instance
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<APIDataSO>("APIData");
            }
            return instance;
        }
    }

    public Dictionary<string, object> responseDataDic = new();

    public event Action OnResponseDataChanged;

    public void SetResponseData(string key, object data)
    {
        responseDataDic[key] = data;
        //Debug.Log($"Data added successfully : {key}, {data}");
        OnResponseDataChanged?.Invoke();
    }

    public T GetValueByKey<T>(string key)
    {
        if (responseDataDic.TryGetValue(key, out object value) && value is T tValue)
        {
            return tValue;
        }

        return default;
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