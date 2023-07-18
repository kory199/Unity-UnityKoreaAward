using System;
using System.Collections.Generic;
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

            return response;
        }
    }

    public static T ParseResponseBodyToModel<T>(string responseBody, string key)
    {
        var temporaryResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
        var dataArray = JArray.Parse(temporaryResponse[key].ToString());

        T data = default;
        foreach (var dataT in dataArray)
        {
            data = dataT.ToObject<T>();
        }

        APIDataDic.SetResponseData(key, data);
        T getData = APIDataDic.GetValueByKey<T>(key);

        return data;
    }
}