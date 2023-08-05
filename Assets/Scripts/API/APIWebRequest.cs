using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

public class APIWebRequest
{
    public static async UniTask<APIResponse<T>> PostAsync<T>(string url, object requestBody)
    {
        if(APIUrls.IsValidUrl(url) == false)
        {
            Debug.LogError($"Invalid URL: {url}. Please check APIUrls.");
        }

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
}