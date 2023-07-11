﻿namespace APIServer;

public enum ResultCode
{
    None = 0,

    // === Common Error 100 ~ ===             
    UnhandleException = 101,
    RedisFailException = 102,
    RedisDelectFailException = 103,
    InValidRequestHttpBody = 104,
    AuthTokenFailWrongAuthToken = 106,

    // === Account SuccessCode 150 ~ ===
    CreateAccountSuccess = 150,

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

    // === User Game Data ===
}