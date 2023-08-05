using System.Drawing.Printing;

namespace APIServer.Services;

public class MemoryDbKeyMaker
{
    private const String loginID = "UID_";
    private const String userLockKey = "ULock_";
    private const String ping = "Ping_";

    public static String MakeUIDKey(String id) => loginID + id;
    public static String MakeUserLockKey(String id) => userLockKey + id;
    public static String MakePingkey(String id) => ping + id;
}