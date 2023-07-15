using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor.Purchasing;
using UnityEngine;

public class AccountRequest
{
    public string ID { get; set; }
    public string Password { get; set; }
}

public class GameData
{
    public string ID { get; set; }
    public string AuthToken { get; set; }
}

public class PlayerData
{
    public long player_uid { get; set; }
    public int exp { get; set; }
    public int hp { get; set; }
    public int score { get; set; }
    public int level { get; set; }
    public int status { get; set; }
}

public class APITest : MonoBehaviour
{
    private void Awake()
    {
        TestAPI();
    }

    private async void TestAPI()
    {
        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi,
            new GameData { ID = "ParkHyeWon", AuthToken = "5hl31tlfmuxphy43u6q8b8qze" });
    }

    private async UniTask CallAPI<T, TRequest>(string apiUrl, TRequest requestBody)
    {
        //Debug.Log($"Calling API: {apiUrl} with body: {JsonConvert.SerializeObject(requestBody)}");
        var apiResponse = await APIWebRequest.PostAsync<APIResponse<T>>(apiUrl, requestBody);

        if (apiResponse != null)
        {
            //Debug.Log($"API Response: {JsonConvert.SerializeObject(requestBody)}");
            PlayerData player = APIWebRequest.ParseResponseBodyToModel<PlayerData>(apiResponse.responseBody, "playerData");
        }
    }
}