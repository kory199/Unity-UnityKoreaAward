namespace GameAPIServer;

public enum ErrorCode
{
    None = 0,

    // === Common 100 ~ ===             
    UnhandleException = 101,            
    RedisFailException = 102,           
    InValidRequestHttpBody = 103,       
    AuthTokenFailWrongAuthToken = 106,  

    // === MaterData 150 ~ ===
    LoadMaterDataException = 150,        

    // === Account 200 ~ ===
    CreateAccountFailDuplicate = 200,    
    CreateAccountFailException = 201,    
    LoginFailException = 202,            
    LoginFailUserNotExist = 203,         
    LoginFailPwNotMatch = 204,           
    LoginFailSetAuthToken = 205,         
    AuthTokenMismatch = 206,             
    AuthTokenNotFound = 207,             
    AuthTokenFailWrongKeyword = 208,     
    AuthTokenFailSetNx = 209,            
    AccountIdMismatch = 210,             
    DuplicatedLogin = 211,               
    CreateAccountFailInsert = 212,       
    LoginFailAddRedis = 214,
    LoginFailAddRedisException = 215,
    CheckAuthFailNotExist = 216,         
    CheckAuthFailNotMatch = 217,         
    CheckAuthFailException = 210,        

    // === Versoin Check 250 ~ ===
    VerifyMasterDataFail = 220,
    VerifyMasterDataFailException = 221,
    CreateVersionException = 225,
    UserVersionInfoNotFound = 220,
    UserVersionMishMatch = 230,
    DeleteUserVersionFail = 235,
    VerifyVersionException = 240,

    // === User Game Data 300 ~ ===
    UserGameInfoNotFound = 300,
    CreateGameDataFailInsert = 301,
    CreateGameDataFailException = 302,
    UserGameDataException = 305,
    UserGoldUpdateFail = 306,
    UserGoldUpdateFailException = 307,
    UpdatUserGameDataFail = 308,
    UpdatUserGameDataFailException = 309,
    LoadGameDataFailException = 310,
    DeleteGameDataFail = 315,
    CreateUserGameDB = 316,

    // === User Item 320 ~ ===
    CreateItemDataFailInsert = 320,
    CreateItemDataFailException = 321,
    UserItemInfoNotFound = 321,
    LoadItemDataFailException = 325,
    DeleteItmeDataFail = 330,
    GetUserListFail = 340,
    EnhanceMaxCountOverException = 350,
    EnhanceFailException = 351,
    UpdateItemDataFailException = 355,

    // === GetDB 400~ ===
    GetAccountDBConnectionFail = 400,      
    GetGameDBConnectionFail = 401,         

    // === Mail 500 ~ ===
    CreateMailFail = 501,
    CreateMailFailException = 502,
    LoadMailDataFailException = 520,
    DeleteMailDataFailException = 530,
    OpenMailDataFailException = 540,
    GetMailListFail = 550,
    UpdatMailStateTypeException = 560,
    UpdateExpFail = 561,
    UpdateExpFailException = 562,
    MailPageIndexOutException = 580,

    // === Attendance 600~ ===
    SaveAttendanceDataFailException = 601,
    DeleteAttendanceDataFailException = 610,
    SandAttendanceItemMailFailException = 620,
    CheckAttendanceDayException = 630,
    UpdateContinuousAttendanceFail = 631,
    UpdateContinuousAttendanceFailException = 632,
    DoubleAttendanceException = 640,
    OverDatedAttendanceException = 650,

    // === InAppPay 700~ ===
    ReceiptDulicateAuthenticationException = 701,
    ReceiptException = 710,

    // === Enhance 750 ~ ===
    UnenchantableItemException = 750,

    // === Dungeon Stage 800 ===
    LoadStageFail = 801,
    LoadStageFailException = 802,
    LoadStageListFail = 803,
    LoadStageListFailException = 804,
    StageSelectionFail = 810,
    StageSelectionFailException = 811,
    StageItemStageCodeOverfail = 812,
    LoadStageItemFail = 813,
    LoadStageItemFailException = 814,

    // === Redis UserState 900 ~ ===
    UserStateUpdataFail = 901,

}