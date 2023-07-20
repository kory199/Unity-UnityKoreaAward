using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class APIWebRequest
{
    public static async UniTask<APIResponse<T>> PostAsync<T>(string url, object requestBody)
    {
        byte[] bodyRaw = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(requestBody));

        using (var request = new UnityWebRequest(url, "POST"))
        {
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error :{request.error}");
                return null;
            }

            APIResponse<T> response = new()
            {
                responseBody = request.downloadHandler.text,
                Data = JsonConvert.DeserializeObject<T>(request.downloadHandler.text),
            };

#if UNITY_EDITOR
            Debug.Log($"responseBody : {request.downloadHandler.text}");
#endif

            return response;
        }
    }

    public static T ParseResponseBodyToModel<T>(string responseBody, string key)
    {
        var temporaryResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);

        if (temporaryResponse == null || !temporaryResponse.ContainsKey(key))
        {
            Debug.LogError($"The key '{key}' was not present in the response.");
            return default;
        }

        var dataArray = JArray.Parse(temporaryResponse[key]?.ToString());

        if (typeof(T).IsArray)
        {
            Type elementType = typeof(T).GetElementType();
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var dataT in dataArray)
            {
                list.Add(dataT.ToObject(elementType));
            }

            // Create an array of the correct type and copy the elements into it.
            Array array = Array.CreateInstance(elementType, list.Count);
            for (int i = 0; i < list.Count; i++)
            {
                array.SetValue(list[i], i);
            }

            APIDataDic.SetResponseData(key, array);
            APIDataDic.GetValueByKey<Array>(key);
            return (T)(object)array;
        }
        else if (typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>))
        {
            Type elementType = typeof(T).GetGenericArguments()[0];
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var dataT in dataArray)
            {
                list.Add(dataT.ToObject(elementType));
            }

            APIDataDic.SetResponseData(key, list);
            return (T)(object)list;
        }
        else
        {
            if (dataArray.Count > 0)
            {
                T data = dataArray[0].ToObject<T>();
                APIDataDic.SetResponseData(key, data);
                return data;
            }
            else
            {
                Debug.LogError($"The key '{key}' did not have any associated data in the response.");
                return default;
            }
        }
    }


}