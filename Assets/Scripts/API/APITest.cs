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
        //await LoginAPI();
    }

    private async UniTask LoginAPI()
    {
        var accountRequest = new AccountRequest { ID = "ParkHyeWon", Password = "1234" };
        var apiResponse = await CallAPI<AccountRequest, Dictionary<string, object>>(APIUrls.LoginApi, accountRequest);

        if (apiResponse != null && apiResponse.TryGetValue("authToken", out var tokenObject))
        {
            string authToken = tokenObject as string;
            TokenManager.Instacne.SaveToken(authToken);
            await GetGameDataAPI(authToken);
        }
    }

    private async UniTask GetGameDataAPI(string authToken)
    {
        var gameDataRequest = new GameData { ID = "ParkHyeWon", AuthToken = authToken };
        var playerData = await CallAPI<GameData, PlayerData>(APIUrls.GameDataApi, gameDataRequest);
    }

    private async UniTask<TResponse> CallAPI<TRequest, TResponse>(string apiUrl, TRequest requestBody)
    {
        var apiResponse = await APIWebRequest.PostAsync<APIResponse<Dictionary<string, object>>>(apiUrl, requestBody);

        TResponse data = default;

        if (apiResponse != null)
        {
            data = APIWebRequest.ParseResponseBodyToModel<TResponse>(apiResponse.responseBody, "data");
        }

        return data;
    }

    private string GetLowerClassName(object className) => className.GetType().Name.ToLower();

}