using System.Collections;
using Newtonsoft.Json;
using System.Collections.Generic;
using UnityEngine;

public class DebugUtility 
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void Log<T>(string title, T message)
    {
        if (message is IDictionary dictionary)
        {
            LogDictionary(title, dictionary);
        }
        else
        {
            Debug.Log($"{title} : {message}");
        }
    }

    private static void LogDictionary(string title, IDictionary dictionary)
    {
        var output = new System.Text.StringBuilder();
        output.Append($"{title} : ");

        foreach (DictionaryEntry entry in dictionary)
        {
            output.Append($"Key: {entry.Key}, Value: {entry.Value} | ");
        }

        Debug.Log(output.ToString());
    }
}