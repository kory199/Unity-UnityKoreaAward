namespace APIServer;

public enum ResultCode
{
    None = 0,

    InsertFail = 10,
    InsertException = 11,
    DeleteException = 12,
    GetByException = 13,
    GetByListException = 14,

    // === SuccessCode 50 ~ ===
    LoadGameVersionSuccess = 50,
    CheckStatusSuccess = 51,
    LoadMasterDataSuccess = 52,
    CreateAccountSuccess = 53,
    LoginSuccess = 54,
    CreateGameDataSuccess = 55,
    LoadGameDataSuccess = 56,
    LoadRankingDataSuccess = 57,
    UpdateScoreSuccess = 58,
    LoadStageSuccess = 59,
    GetNewStageSuccess = 60,
    PingSuccess = 61,
    LogOutSuccess = 62,
    UserLogOut = 63,

    // === GameVersion 80~ ===
    LoadGameVersionFail = 80,
    LoadGameVersionFailException = 81,
    GameVersionResqustStringCheck = 82,

    // === Common Error 100 ~ ===             
    UnhandleException = 101,
    RedisFailException = 102,
    RedisDelectFailException = 103,
    RedisPingFail = 104,
    RedisPingException = 105,
    InValidRequestHttpBody = 106,
    AuthTokenFailWrongAuthToken = 107,

    // === Master Data 150 ~ ===
    LoadMasterMonsterDataFail = 150,
    LoadMasterMonsterDataFailException = 151,
    LoadStageSpawnMonsterDataFail = 152,
    LoadStageSpawnMonsterDataFailException = 153,
    LoadPlayerStatusDataFail = 154,
    LoadPlayerStatusDataFailException = 155,
    LoadMasterDataFailException = 156,
    RedisConnectionException = 157,

    // === Account ErrorCode 200~ ===
    CreateAccountFailInsert = 200,
    FailedtoCreateAccount = 201,
    VerifyAccountIdFail = 202,
    VerifyAccountIdFailException = 203,

    //CreateAccountFailException = 201,
    LoginFailUserNotExist = 202,
    LoginFailException = 203,
    LoginFailPwNotMatch = 204,
    LoginFailAddRedis = 206,
    LoginFailAddRedisException = 207,
    LoginFailSetAuthToken = 208,
    CheckAuthFailNotExist = 200,
    AuthTokenFailSetNx = 206,
    
    // === User Game Data 300 ~ ===
    CreateDefaultGameDataFailInsert = 301,
    CreateGameDataFailInsert = 302,
    CreateDefaultGameDataFailException = 303,
    CreateGameDataFailException = 304,
    PlayerGameDataNotFound = 305,
    LoadGameDataFailException = 306,
    DeleteGameDataFailException = 307,
    ChangedStatusFail = 308,
    ChangedStatusFailException = 309,
    CheckStatusFail = 310,
    CheckStatusFailException = 311,

    // === Ranking Data 400 ~ ===
    LoadRankingDataFail = 400,
    LoadRankingDataFailException = 401,
    LoadRankingDataforUserFail = 402,

    // === UpdatScore 500 ===
    UpdateScoreDataFail = 501,
    UpdateScoreDataNullException = 502,
    UpdateScoreDataFailException = 503,

    // === Stage Info 550 ===
    LoadStageInfoFail = 550,
    LoadStageInfoFailException = 551,

    // === Stage 600 ===
    CreateDefaultStageFailInsert = 600,
    CreateDefaultStageFailException = 601,
    LoadStageDataNotFound = 602,
    LoadStageDataFailException = 603,
    UpdateStageDataFail = 604,
    UpdateStageDataFailException = 605,
    CreateStageFailInsert = 606,
    CreateStageFailException = 607,
    CreateNewStageFailInsert = 608,
    StageNumNotMatch = 609,
}