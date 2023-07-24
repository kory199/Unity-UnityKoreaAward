namespace APIServer;

public enum ResultCode
{
    None = 0,

    InsertFail = 10,
    InsertException = 11,
    DeleteException = 12,
    GetByException = 13,

    // === SuccessCode 50 ~ ===
    CreateAccountSuccess = 50,
    LoginSuccess = 51,
    CreateGameDataSuccess = 52,
    LoadGameDataSuccess = 53,
    LoadRankingDataSuccess = 55,

    // === Common Error 100 ~ ===             
    UnhandleException = 101,
    RedisFailException = 102,
    RedisDelectFailException = 103,
    InValidRequestHttpBody = 104,
    AuthTokenFailWrongAuthToken = 106,

    // === Account ErrorCode 200~ ===
    CreateAccountFailInsert = 200,
    FailedtoCreateAccount = 201,

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

    // === Ranking Data 400 ~ ===
    LoadRankingDataFail = 400,
    LoadRankingDataFailException = 401,
    LoadRankingDataforUserFail = 402,

}