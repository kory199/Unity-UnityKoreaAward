namespace GameAPIServer.StateType;

public enum EventType
{
    LoadVersion = 1,
    CreateAccount = 11,
    VersionCheck = 15,
    Login = 21,
    LoginAddRedis = 22,
    CreateGameData = 23,
    LoadGameData = 25,
    CreateItemData = 26,
    LoadItemData = 27,
    LoadMail = 30,
    GetAttendance = 40,
    GetInAppPayItem = 50,
    Enhance = 60,
    GamePlay = 61,
    FarmingItem = 62,
    GameOver = 63,
    GameChancel = 65,
}