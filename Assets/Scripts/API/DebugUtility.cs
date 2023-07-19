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
        else if (message is APIResponse<T> apiResponse)
        {
            if (apiResponse.Data is IDictionary dataDictionary)
            {
                LogDictionary($"{title} Data", dataDictionary);
            }
            else
            {
                Debug.Log($"{title} Data: {apiResponse.Data.ToString()}");
            }

            var responseBodyDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);
            LogDictionary($"{title} responseBody", responseBodyDictionary);
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