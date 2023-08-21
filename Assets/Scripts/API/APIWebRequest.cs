using System.Text;
using APIModels;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class APIWebRequest
{
    public static async UniTask<APIResponse<T>> PostAsync<T>(string url, object requestBody)
    {
        if(APIUrls.IsValidUrl(url) == false)
        {
            Debug.LogError($"Invalid URL: {url}. Please check APIUrls.");
            return null;
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

            var jsonResponse = JObject.Parse(request.downloadHandler.text);

            int result = jsonResponse.Value<int>("result");
            string resultMessage = jsonResponse.Value<string>("resultMessage");

            T data;

            if (jsonResponse["masterDataDic"] != null)
            {
                var masterDataResponse = jsonResponse.ToObject<MasterDataResponse>();
                data = (T)(object)masterDataResponse;
            }
            else
            {
                data = jsonResponse.ToObject<T>();
            }

            APIResponse<T> response = new()
            {
                Result = result,
                ResultMessage = resultMessage,
                Data = data
            };

#if UNITY_EDITOR
            Debug.Log($"Response : {JsonConvert.SerializeObject(response)}");
#endif

            return response;
        }
    }
}