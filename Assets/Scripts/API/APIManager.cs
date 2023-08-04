using System;
using System.Collections.Generic;
using APIModels;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class APIManager : MonoSingleton<APIManager>
{
    private string _id;
    private string _authToken;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public async UniTask<String> GetGameVersionAPI()
    {
        string gameVersion = null;
        Version_req requestBody = new();

        await CallAPI<Dictionary<string, object>, Version_req>(APIUrls.VersionApi, requestBody, apiResponse =>
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

    public async UniTask GetMasterDataAPI()
    {
        await CallAPI<Dictionary<string, object>, MasterData_req>(APIUrls.MasterDataApi, new MasterData_req(), HandleMasterDataResponse) ;
    }

    private void HandleMasterDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<MasterDataResponse>(apiResponse.responseBody);

        APIDataSO.Instance.SetResponseData(APIDataDicKey.MeleeMonstser, responseBody.masterDataDic.MeleeMonstser);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.RangedMonster, responseBody.masterDataDic.RangedMonster);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.BOSS, responseBody.masterDataDic.BOSS);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.PlayerStatus, responseBody.masterDataDic.PlayerStatus);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.StageSpawnMonster, responseBody.masterDataDic.StageSpawnMonster);
    }

    public class MasterDataResponse
    {
        public MasterDataArray masterDataDic { get; set; }
    }

    public class MasterDataResList
    {
        public List<MonsterData_res> MeleeMonstser { get; set; }
        public List<MonsterData_res> RangedMonster { get; set; }
        public MonsterData_res BOSS { get; set; }
        public List<PlayerStatus_res> PlayerStatus { get; set; }
        public List<StageSpawnMonsterData_res> StageSpawnMonster { get; set; }
    }

    public class MasterDataArray
    {
        public MonsterData_res[] MeleeMonstser { get; set; } = new MonsterData_res[4];
        public MonsterData_res[] RangedMonster { get; set; } = new MonsterData_res[4];
        public MonsterData_res BOSS { get; set; }
        public PlayerStatus_res[] PlayerStatus { get; set; } = new PlayerStatus_res[4];
        public StageSpawnMonsterData_res[] StageSpawnMonster { get; set; } = new StageSpawnMonsterData_res[4];
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
            APIDataSO.Instance.SetResponseData(APIDataDicKey.GameData, NewGameData());
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
        GameData curUserInfo = APIDataSO.Instance.GetValueByKey<GameData>(APIDataDicKey.GameData);

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
                APIDataSO.Instance.SetResponseData(APIDataDicKey.PlayerData, playerData);
            }
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

            APIDataSO.Instance.SetResponseData(APIDataDicKey.RankingData, rankingDataList);
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

    public async UniTask StageUpToServer(int stageNum, int score)
    {
        var userData = GetApiSODicUerData();
        string id = userData.ID;
        string authToken = userData.AuthToken;

        StageData stageData = new StageData
        {
            ID = id,
            AuthToken = authToken,
            StageNum = stageNum
        };

        StageClear stageClear = new StageClear
        {
            ID = id,
            AuthToken = authToken,
            Score = score
        };

        UniTask callStageApi = CallAPI<Dictionary<string, object>, StageData>(APIUrls.StageApi, stageData, null);
        UniTask callStageClear = CallAPI<Dictionary<string, object>, StageClear>(APIUrls.StageClear, stageClear, null);

        await UniTask.WhenAll(callStageApi, callStageClear);
    }

    private void HandleStageDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        var responseBody = JsonConvert.DeserializeObject<Dictionary<string, object>>(apiResponse.responseBody);

        if(responseBody.TryGetValue("stageData", out object stageDataObj))
        {
            List<StageInfo> stageDataList =
                JsonConvert.DeserializeObject<List<StageInfo>>(stageDataObj.ToString());

            APIDataSO.Instance.SetResponseData(APIDataDicKey.StageData, stageDataList);
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
}