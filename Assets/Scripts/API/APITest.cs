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

        await CallAPI<Dictionary<string, object>, AccountRequest>(APIUrls.LoginApi, newPlayer, HandleLoginResponse);
    }

    private void HandleLoginResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        //DebugUtility.Log("Login responseBody", responseBody);

        if (responseBody.TryGetValue("authToken", out var authTokenObj))
        {
            string authToken = authTokenObj as string;

            if (!string.IsNullOrEmpty(authToken))
            {
                TokenManager.Instacne.SaveToken(authToken);
            }
        }


        //GetGameDataAPI();
        GetRanking();

    }

    private async UniTask GetGameDataAPI()
    {
       //GameData gameData = new GameData
       //{
       //    ID = TokenManager.Instacne.GetID(),
       //    AuthToken = TokenManager.Instacne.GetToken()
       //};

        
        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi, NewGameData(), HandleGameDataResponse);
    }

    private async UniTask GetRanking()
    {
        //GameData gameData = new GameData
        //{
        //    ID = TokenManager.Instacne.GetID(),
        //    AuthToken = TokenManager.Instacne.GetToken()
        //};

        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.RankingApi, NewGameData(), HandleRankingDataResponse);
    }

    private GameData NewGameData()
    {
        GameData gameData = new GameData
        {
            ID = TokenManager.Instacne.GetID(),
            AuthToken = TokenManager.Instacne.GetToken()
        };

        return gameData;
    }

    private async UniTask CallAPI<T, TRequest>(string apiUrl, TRequest requestBody, Action<APIResponse<T>> handler)
    {
        try
        {
            var apiResponse = await APIWebRequest.PostAsync<T>(apiUrl, requestBody);
#if UNITY_EDITOR
            //DebugUtility.Log("ApiResponse", apiResponse);
#endif

            if (apiResponse == null)
            {
                Debug.LogError("API Response is null");
            }

            handler?.Invoke(apiResponse);
        }
        catch (UnityWebRequestException e)
        {
            Debug.LogError($"API request failed : {e.Message}");
        }
    }

    private void HandleGameDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        APIWebRequest.ParseResponseBodyToModel<PlayerData>(apiResponse.responseBody, "playerData");
    }

    private void HandleRankingDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        APIWebRequest.ParseResponseBodyToModel<RankingData[]>(apiResponse.responseBody, "rankingData");
    }
}