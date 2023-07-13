using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class APIWebRequest
{
    public static async UniTask<APIResponse<T>> PostAsync<T>(string url, object requestBody)
    {
        string jsonBody = JsonConvert.SerializeObject(requestBody);

        using (var request = new UnityWebRequest(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");

            await request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
                return null;
            }

            string responseBody = request.downloadHandler.text;
            APIResponse<T> response = APIResponse<T>.FromJson(responseBody);

            if (response != null)
            {
                var responseDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseBody);
                responseDict.Remove("Result");
                responseDict.Remove("ResultMessage");
                APIDataDic.SetResponseData(typeof(T).Name, responseDict);

                if (response.Data is Dictionary<string, object> data)
                {
                    response.Data = (T)(object)data;
                }
            }

            return response;
        }
    }
}