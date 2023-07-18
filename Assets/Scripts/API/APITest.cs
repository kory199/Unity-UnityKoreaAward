using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEditor.Purchasing;
using UnityEngine;
using System;
using APIModels;

public class APITest : MonoBehaviour
{
    private void Awake()
    {
        LoginAPI();
    }

    private async UniTask LoginAPI()
    {
        AccountRequest newPlayer = new AccountRequest { ID = "ParkHyeWon", Password = "1234" };
        TokenManager.Instacne.SaveID(newPlayer.ID);

        await CallLogainAPI<Dictionary<string, object>, AccountRequest>(APIUrls.LoginApi, newPlayer);
    }

    private async UniTask CallLogainAPI<T, TRequest>(string apiUrl, TRequest requestBody)
    {
        var apiResponse = await APIWebRequest.PostAsync<APIResponse<T>>(apiUrl, requestBody);

        if (apiResponse != null)
        {
            //Debug.Log($"API Response: {JsonConvert.SerializeObject(apiResponse)}");

            var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);
            if (responseBody.TryGetValue("authToken", out var authTokenObj))
            {
                string authToken = authTokenObj as string;
                if (!string.IsNullOrEmpty(authToken))
                {
                    TokenManager.Instacne.SaveToken(authToken);
                }
            }

            GetGameDataAPI();
        }
    }

    private async UniTask GetGameDataAPI()
    {
        GameData gameData = new GameData
        {
            ID = TokenManager.Instacne.GetID(),
            AuthToken = TokenManager.Instacne.GetToken()
        };

        await CallGameDataAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi, gameData);
    }

    private async UniTask CallGameDataAPI<T, TRequest>(string apiUrl, TRequest requestBody)
    {
        var apiResponse = await APIWebRequest.PostAsync<APIResponse<T>>(apiUrl, requestBody);

        if (apiResponse != null)
        {
            //Debug.Log($"API Response: {JsonConvert.SerializeObject(apiResponse)}");
            PlayerData player = APIWebRequest.ParseResponseBodyToModel<PlayerData>(apiResponse.responseBody, "playerData");
        }
    }

    private string GetLowerClassName(object className) => className.GetType().Name.ToLower();
}