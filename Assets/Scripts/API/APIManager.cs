using System;
using System.Collections.Generic;
using APIModels;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class APIManager : MonoSingleton<APIManager>
{
    public APIDataSO apidata = null;

    private string _id;
    private string _authToken;

    private void Awake()
    {
        if (apidata == null) apidata = Resources.Load<APIDataSO>("APIData");

        DontDestroyOnLoad(this.gameObject);
    }

    public async UniTask<String> GetGameVersionAPI()
    {
        string gameVersion = null;
        GetVersion requestBody = new GetVersion();

        await CallAPI<Dictionary<string, object>, GetVersion>(APIUrls.VersionApi, requestBody, apiResponse =>
        {
            if (apiResponse?.Data is Dictionary<string, object> data)
            {
                if (data.ContainsKey("gameVer"))
                {
                    gameVersion = data["gameVer"].ToString();
                }
            }
        });

        return gameVersion;
    }

    public async UniTask CreateAccountAPI(User user)
    {
        await CallAPI<Dictionary<string, object>, User>(APIUrls.CreateAccountApi, user, null);
    }

    public async UniTask LoginAPI(User user)
    {
        _id = user.ID;

        await CallAPI<Dictionary<string, object>, User>(APIUrls.LoginApi, user, HandleLoginResponse);
    }

    private void HandleLoginResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        if (responseBody.TryGetValue("authToken", out var authTokenObj))
        {
            _authToken = authTokenObj as string;
        }

        if (_authToken != null && _authToken != string.Empty)
        {
            apidata.SetResponseData("GameData", NewGameData());
        }
        else
        {
            Debug.LogError("Failed to retrieve authToken from API response.");
        }
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

    public GameData GetApiSODicUerData()
    {
        GameData curUserInfo = apidata.GetValueByKey<GameData>("GameData");

        if (curUserInfo == null)
        {
            Debug.LogError("No GameData found in APIDataSO.");
        }

        return curUserInfo;
    }

    public async UniTask GetGameDataAPI()
    {
        try
        {
            await CallAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi, GetApiSODicUerData(), HandleGameDataResponse);
        }
        catch (Exception e)
        {
            Debug.LogError($"Error calling GameData API: {e.Message}");
        }
    }

    private void HandleGameDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        if (responseBody.TryGetValue("playerData", out var playerDataObj) && playerDataObj is JArray)
        {
            foreach (var playerDataJToken in playerDataObj as JArray)
            {
                PlayerData playerData = playerDataJToken.ToObject<PlayerData>();
                apidata.SetResponseData("PlayerData", playerData);
            }
        }
        else
        {
            Debug.LogError("Failed PlayerData from API response.");
        }
    }

    public async UniTask GetRankingAPI()
    {
        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.RankingApi, GetApiSODicUerData(), HandleRankingDataResponse);
    }

    private void HandleRankingDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        if (responseBody.TryGetValue("rankingData", out object rankingDataObj))
        {
            List<RankingData> rankingDataList =
                JsonConvert.DeserializeObject<List<RankingData>>(rankingDataObj.ToString());

            apidata.SetResponseData("RankingData", rankingDataList);
        }
        else
        {
            Debug.LogError("Failed RankingData from API response.");
        }
    }

    public async UniTask GetStageAPI(int stageNum)
    {
        StageData stageData = new StageData
        {
            ID = GetApiSODicUerData().ID,
            AuthToken = GetApiSODicUerData().AuthToken,
            StageNum = stageNum,
        };

        await CallAPI<Dictionary<string, object>, StageData>(APIUrls.StageApi, stageData, HandleStageDataResponse);
    }

    private void HandleStageDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        if(responseBody.TryGetValue("stageData", out object stageDataObj))
        {
            List<StageInfo> stageDataList =
                JsonConvert.DeserializeObject<List<StageInfo>>(stageDataObj.ToString());

            apidata.SetResponseData("StageData", stageDataList);
        }
        else
        {
            Debug.LogError("Failed StageData from API response.");
        }
    }

    private async UniTask CallAPI<T, TRequest>(string apiUrl, TRequest requestBody, Action<APIResponse<T>> handler)
    {
        try
        {
            var apiResponse = await APIWebRequest.PostAsync<T>(apiUrl, requestBody);

            if (apiResponse == null)
            {
                Debug.LogError("API Response Data is null");
            }

            handler?.Invoke(apiResponse);
        }

        catch (UnityWebRequestException e)
        {
            Debug.LogError($"API request failed : {e.Message}");
        }
    }

    public async UniTask StageUpToServer(string name, int stageNum, int score, float time)
    {
        //아래 그냥 ctrl c + v 이므로 새로 작성해야함
        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.StageApi, GetApiSODicUerData(), HandleRankingDataResponse);
    }

}