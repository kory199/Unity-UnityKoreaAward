using APIServer.StateType;

namespace APIServer;

public class LogManager
{
    private static ILoggerFactory s_loggerFactory;
    public static ILogger Logger { get; private set; }

    public static Dictionary<EventType, EventId> EventIdDic = new()
    {
        { EventType.CreateAccount, new EventId((Int32)EventType.CreateAccount, "CreateAccount") },
        { EventType.Login, new EventId((Int32)EventType.Login, "Login") },
        { EventType.LogOut, new EventId((Int32)EventType.LogOut, "LogOut")},
    };

    public static void SetLoggerFactory(ILoggerFactory loggerFactory, String categoryName)
    {
        s_loggerFactory = loggerFactory;
        Logger = loggerFactory.CreateLogger(categoryName);
    }

    public static ILogger<T> GetLogger<T>() where T : class
    {
        return s_loggerFactory.CreateLogger<T>();
    }
}