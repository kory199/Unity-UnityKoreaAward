using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using APIModels;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class APIManager : MonoSingleton<APIManager>
{
    public APIDataSO apidata = null;

    private string _id;
    private string _authToken;

    private void Awake()
    {
        if(apidata == null)
        {
            apidata = Resources.Load<APIDataSO>("APIData");
        }
    }

    public async UniTask CreateAccpuntAPI(User user)
    {
        await CallAPI<Dictionary<string, object>, User>(APIUrls.CreateAccountApi, user, null);
    }

    public async UniTask LoginAPI(User user)
    {
        _id = user.ID;

#if UNITY_EDITOR
        Debug.Log($"_id : {_id}");
#endif
        await CallAPI<Dictionary<string, object>, User>(APIUrls.LoginApi, user, HandleLoginResponse);
    }

    private void HandleLoginResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        if (responseBody.TryGetValue("authToken", out var authTokenObj))
        {
            _authToken = authTokenObj as string;
        }

        if (_authToken != null)
        {
            apidata.SetResponseData("GameData", NewGameData());
        }
        else
        {
            Debug.LogError("Failed to retrieve authToken from API response.");
        }
    }

    public async UniTask GetGameDataAPI()
    {
         await CallAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi, NewGameData(), HandleGameDataResponse);
    }

    private void HandleGameDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        // APIWebRequest.ParseResponseBodyToModel<PlayerData>(apiResponse.responseBody, "playerData");
    }

    public async UniTask GetRanking()
    {
        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.RankingApi, NewGameData(), HandleRankingDataResponse);
    }

    private void HandleRankingDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        // APIWebRequest.ParseResponseBodyToModel<RankingData[]>(apiResponse.responseBody, "rankingData");
    }

    private GameData NewGameData()
    {
        GameData gameData = new GameData
        {
            ID = _id,
            AuthToken = _authToken
        };

        return gameData;
    }

    private async UniTask CallAPI<T, TRequest>(string apiUrl, TRequest requestBody, Action<APIResponse<T>> handler)
    {
        try
        {
            var apiResponse = await APIWebRequest.PostAsync<T>(apiUrl, requestBody);

            if(apiResponse == null)
            {
                Debug.LogError("API Respon is null");
            }

            handler?.Invoke(apiResponse);
        }
        catch (UnityWebRequestException e)
        {
            Debug.LogError($"API request failed : {e.Message}");
        }
    }
}