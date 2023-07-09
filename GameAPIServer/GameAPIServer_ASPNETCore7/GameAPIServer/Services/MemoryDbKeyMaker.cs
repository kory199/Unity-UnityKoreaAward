namespace GameAPIServer.Services;

public class MemoryDbKeyMaker
{
    private const string loginID = "UID_";
    private const string userLockKey = "ULock_";

    private const string itemID = "Item_";
    private const string npcID = "NPCList_";

    public static string MakeUIDKey(string id) => loginID + id;
    public static string MakeUserLockKey(string id) => userLockKey + id;

    public static string MakeItemKey(string id) => itemID + id;
    public static string MakeNPCKey(string id) => npcID + id;
}