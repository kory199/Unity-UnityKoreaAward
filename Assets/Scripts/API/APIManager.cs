using System;
using System.Collections.Generic;
using System.Linq;
using APIModels;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

public class APIManager : MonoSingleton<APIManager>
{
    private string _id;
    private string _authToken;

    public bool IsLogin { get; private set; }
    public Dictionary<string, object> MansterDataDic = new Dictionary<string, object>();

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public async UniTask<string> GetGameVersionAPI()
    {
        string gameVersion = null;
        Version_req requestBody = new();

        bool result = await CallAPI<Dictionary<string, object>, Version_req>(APIUrls.VersionApi, requestBody, APISuccessCode.LoadGameVersionSuccess ,apiResponse =>
        {
            if (apiResponse?.Data is Dictionary<string, object> data)
            {
                if (data.ContainsKey("gameVer"))
                {
                    gameVersion = data["gameVer"].ToString();
                }
            }
        });

        if(result)
        {
            return gameVersion;
        }
        else
        {
            return null;
        }
    }

    public async UniTask<bool> GetMasterDataAPI()
    {
        bool result = await CallAPI<MasterDataResponse, MasterData_req>(APIUrls.MasterDataApi, new MasterData_req(), APISuccessCode.LoadMasterDataSuccess, HandleMasterDataResponse);

        if (result == false)
        {
            return false;
        }

        return result;
    }

    private void HandleMasterDataResponse(APIResponse<MasterDataResponse> apiResponse)
    {
        SetDicData(MasterDataDicKey.MeleeMonster, apiResponse.Data.masterDataDic.MeleeMonster);
        SetDicData(MasterDataDicKey.RangedMonster, apiResponse.Data.masterDataDic.RangedMonster);
        SetDicData(MasterDataDicKey.BOSS, apiResponse.Data.masterDataDic.BOSS);
        SetDicData(MasterDataDicKey.PlayerStatus, apiResponse.Data.masterDataDic.PlayerStatus);
        SetDicData(MasterDataDicKey.StageSpawnMonster, apiResponse.Data.masterDataDic.StageSpawnMonster);
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

        bool result = await CallAPI<Dictionary<string, object>, User>(APIUrls.LoginApi, user, APISuccessCode.LoginSuccess, (apiResponse) =>
        {
            if (apiResponse.Data.TryGetValue("authToken", out var authTokenObj))
            {
                _authToken = authTokenObj as string;
            }
        });

        if (result == false)
        {
            IsLogin = false;
            return false;
        }

        IsLogin = true;
        return result;
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

    public async UniTask<bool> LogOutAPI()
    {
        bool result = await CallAPI<Dictionary<string, object>, GameData>(APIUrls.LogOut, NewGameData(), APISuccessCode.LogOutSuccess, null);

        if (result)
        {
            _id = "";
            _authToken = "";
            return result;
        }
        else
        {
            return false;
        }
    }

    public async UniTask<PlayerData> GetGameDataAPI()
    {

        PlayerData playerData = null;

        bool result = await CallAPI<Dictionary<string, object>, GameData>(APIUrls.GameDataApi, NewGameData(), APISuccessCode.LoadGameDataSuccess, (apiResponse) =>
        {
            if (apiResponse.Data.TryGetValue("playerData", out var playerDataObj) && playerDataObj is JObject)
            {
                playerData = (playerDataObj as JObject).ToObject<PlayerData>();
            }
        });

        return playerData;
    }

    public async UniTask<(List<RankingData>, string)> GetRankingAPI()
    {
        List<RankingData> rankingList = null;

        bool result = await CallAPI<Dictionary<string, object>, GameData>(APIUrls.RankingApi, NewGameData(), APISuccessCode.LoadRankingDataSuccess, (apiResponse) =>
        {
            if (apiResponse.Data.TryGetValue("rankingData", out object rankingDataObj))
            {
                rankingList = JsonConvert.DeserializeObject<List<RankingData>>(rankingDataObj.ToString());

                if (rankingList == null || rankingList.Count == 0)
                {
                    Debug.LogError("RankingDataList is null or empty.");
                }
            }
            else
            {
                Debug.LogError("Failed RankingData from API response.");
            }
        });

        if (result)
        {
            return (rankingList,_id);
        }
        else
        {
            return (null, null);
        }
    }

    public async UniTask<int> GetStageAPI(int stageNum)
    {
        StageData stageData = new StageData
        {
            ID = NewGameData().ID,
            AuthToken = NewGameData().AuthToken,
            StageNum = stageNum,
        };

        int lastStageId = 0;

        bool result = await CallAPI<Dictionary<string, object>, StageData>(APIUrls.StageApi, stageData, APISuccessCode.LoadStageSuccess, (apiResponse) =>
        {
            if (apiResponse.Data.TryGetValue("stageData", out object stageDataObj))
            {
                List<StageInfo> stageDataList = JsonConvert.DeserializeObject<List<StageInfo>>(stageDataObj.ToString());
                if (stageDataList != null && stageDataList.Count > 0)
                {
                    lastStageId = stageDataList.Last().stage_id;
                }
            }
        });

        return lastStageId;
    }

    public async UniTask StageUpToServer(int stageNum, int score)
    {
        string id = NewGameData().ID;
        string authToken = NewGameData().AuthToken;

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

    private void SetDicData(MasterDataDicKey key, object data) => MansterDataDic[key.ToString()] = data;

    public T GetValueByKey<T>(string key)
    {
        if(MansterDataDic.TryGetValue(key, out object value) && value is T tValue)
        {
            return tValue;
        }

        return default;
    }
}