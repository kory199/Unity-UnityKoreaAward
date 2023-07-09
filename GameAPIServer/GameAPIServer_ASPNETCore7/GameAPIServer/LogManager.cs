using GameAPIServer.StateType;

namespace GameAPIServer;

public static class LogManager
{
    private static ILoggerFactory s_loggerFactory;

    public static Dictionary<EventType, EventId> EventIdDic = new()
    {
        { EventType.LoadVersion, new EventId((int)EventType.LoadVersion, "LoadVersion")},
        { EventType.CreateAccount, new EventId((int)EventType.CreateAccount, "CreateAccount")},
        { EventType.VersionCheck, new EventId((int)EventType.VersionCheck, "VersionCheck")},
        { EventType.Login, new EventId((int)EventType.Login, "Login")},
        { EventType.LoginAddRedis, new EventId((int) EventType.LoginAddRedis, "LoginAddRedis")},
        { EventType.CreateGameData, new EventId((int) EventType.CreateGameData, "CreateGameData")},
        { EventType.LoadGameData, new EventId((int) EventType.LoadGameData, "LoadGameData")},
        { EventType.CreateItemData, new EventId((int) EventType.CreateItemData, "CreateGameData")},
        { EventType.LoadItemData, new EventId((int) EventType.LoadItemData, "LoadItemData")},
        { EventType.LoadMail, new EventId((int) EventType.LoadMail, "LoadMail")},
        { EventType.GetAttendance, new EventId((int) EventType.GetAttendance, "GetAttendance")},
        { EventType.GetInAppPayItem, new EventId((int) EventType.GetInAppPayItem, "GetInAppPayItem")},
        { EventType.Enhance, new EventId((int) EventType.Enhance, "Enhance")},
        { EventType.GamePlay, new EventId((int) EventType.GamePlay, "GamePlay")},
        { EventType.FarmingItem, new EventId((int) EventType.FarmingItem, "FarmingItem")},
        { EventType.GameOver, new EventId((int) EventType.GameOver, "GameOver")},
        { EventType.GameChancel, new EventId((int) EventType.GameChancel, "GameChancel")},
    };

    public static void SetLoggerFactory(ILoggerFactory loggerFactory, string categoryName)
    {
        s_loggerFactory = loggerFactory;
        Logger = loggerFactory.CreateLogger(categoryName);
    }
    
    public static ILogger Logger { get; private set; }

    public static ILogger<T> GetLogger<T>() where T : class
    {
        return s_loggerFactory.CreateLogger<T>();
    }
}