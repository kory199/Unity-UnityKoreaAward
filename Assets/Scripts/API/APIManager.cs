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

    public async UniTask<string> GetGameVersionAPI()
    {
        string gameVersion = null;
        Version_req requestBody = new();

        await CallAPI<Dictionary<string, object>, Version_req>(APIUrls.VersionApi, requestBody, APISuccessCode.LoadGameVersionSuccess ,apiResponse =>
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
        await CallAPI<MasterDataResponse, MasterData_req>(APIUrls.MasterDataApi, new MasterData_req(), APISuccessCode.LoadMasterDataSuccess, HandleMasterDataResponse);
    }

    private void HandleMasterDataResponse(APIResponse<MasterDataResponse> apiResponse)
    {
        APIDataSO.Instance.SetResponseData(APIDataDicKey.MeleeMonster, apiResponse.Data.masterDataDic.MeleeMonstser);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.RangedMonster, apiResponse.Data.masterDataDic.RangedMonster);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.BOSS, apiResponse.Data.masterDataDic.BOSS);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.PlayerStatus, apiResponse.Data.masterDataDic.PlayerStatus);
        APIDataSO.Instance.SetResponseData(APIDataDicKey.StageSpawnMonster, apiResponse.Data.masterDataDic.StageSpawnMonster);
    }

    public class MasterDataResponse
    {
        public MasterDataArray masterDataDic { get; set; }
    }

    public class MasterDataArray
    {
        public MonsterData_res[] MeleeMonstser { get; set; } = new MonsterData_res[4];
        public MonsterData_res[] RangedMonster { get; set; } = new MonsterData_res[4];
        public MonsterData_res BOSS { get; set; }
        public PlayerStatus_res[] PlayerStatus { get; set; } = new PlayerStatus_res[4];
        public StageSpawnMonsterData_res[] StageSpawnMonster { get; set; } = new StageSpawnMonsterData_res[4];
    }

    public async UniTask<bool> CreateAccountAPI(User user)
    {
        bool result = await CallAPI<Dictionary<string, object>, User>(APIUrls.CreateAccountApi, user, APISuccessCode.CreateAccountSuccess, null);

        if (result == false)
        {
            return false;
        }

        return result;
    }

    public async UniTask<bool> LoginAPI(User user)
    {
        _id = user.ID;

        bool result = await CallAPI<Dictionary<string, object>, User>(APIUrls.LoginApi, user, APISuccessCode.LoginSuccess, HandleLoginResponse);

        if (!result)
        {
            Debug.LogError("Failed to create account");
        }
        else
        {
            await GetGameDataAPI();
        }

        return result;
    }

    private void HandleLoginResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        if (apiResponse.Data.TryGetValue("authToken", out var authTokenObj))
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
        await CallAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi, GetApiSODicUerData(), APISuccessCode.LoadGameDataSuccess, HandleGameDataResponse);
    }

    private void HandleGameDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        if (apiResponse.Data.TryGetValue("playerData", out var playerDataObj) && playerDataObj is JArray)
        {
            foreach (var playerDataJToken in playerDataObj as JArray)
            {
                PlayerData playerData = playerDataJToken.ToObject<PlayerData>();
                APIDataSO.Instance.SetResponseData(APIDataDicKey.PlayerData, playerData);
            }
        }
    }

    public async UniTask<bool> GetRankingAPI()
    {
        bool result = await CallAPI<Dictionary<string, object>, GameData>(APIUrls.RankingApi, GetApiSODicUerData(), APISuccessCode.LoadRankingDataSuccess, HandleRankingDataResponse);

        if (result == false)
        {
            return false;
        }

        return result;
    }

    private void HandleRankingDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        if (apiResponse.Data.TryGetValue("rankingData", out object rankingDataObj))
        {
            List<RankingData> rankingDataList =
                JsonConvert.DeserializeObject<List<RankingData>>(rankingDataObj.ToString());

            if (rankingDataList != null && rankingDataList.Count > 0)
            {
                APIDataSO.Instance.SetResponseData(APIDataDicKey.RankingData, rankingDataList);
            }
            else
            {
                Debug.LogError("RankingDataList is null or empty.");
            }
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

        await CallAPI<Dictionary<string, object>, StageData>(APIUrls.StageApi, stageData, APISuccessCode.LoadStageSuccess, HandleStageDataResponse);
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

        UniTask callStageApi = CallAPI<Dictionary<string, object>, StageData>(APIUrls.StageApi, stageData, APISuccessCode.LoadStageSuccess, null);
        UniTask callStageClear = CallAPI<Dictionary<string, object>, StageClear>(APIUrls.StageClear, stageClear, APISuccessCode.UpdateScoreSuccess, null);

        await UniTask.WhenAll(callStageApi, callStageClear);
    }

    private void HandleStageDataResponse(APIResponse<Dictionary<string, object>> apiResponse)
    {
        if (apiResponse.Data.TryGetValue("stageData", out object stageDataObj))
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

    private async UniTask<bool> CallAPI<T, TRequest>(string apiUrl, TRequest requestBody, APISuccessCode successCode, Action<APIResponse<T>> handler)
    {
        try
        {
            var apiResponse = await APIWebRequest.PostAsync<T>(apiUrl, requestBody);

            if (apiResponse == null || apiResponse.Data == null)
            {
                Debug.LogError("API Response Data is null");
                return false;
            }

            if(apiResponse.Result != (int)successCode)
            {
                Debug.LogWarning("API Response Not Success");
                return false;
            }
            
            handler?.Invoke(apiResponse);
            return true;
        }
        catch (UnityWebRequestException e)
        {
            Debug.LogError($"API request failed : {e.Message}");
            return false;
        }
    }
}