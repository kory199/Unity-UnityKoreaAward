namespace GameAPIServer.DBModel;

public class GetAttendanceItem
{
    public Int32 Code { get; set; }
    public String Name { get; set; } = "";
    public Int64 Count { get; set; }
}

public class GetInAppItem
{
    public Int32 Code { get; set; }
    public String Name { get; set; } = "";
    public Int64 ReceiptNo { get; set; }
    public Int64 Count { get; set; }
}

public class LoadGameDataInfo
{
    public Int64 player_id { get; set; }
    public Int32 exp { get; set; }
    public Int32 Level { get; set; }
    public Int32 Hp { get; set; }
    public Int32 Mp { get; set; }
    public Int64 Gold { get; set; }
}

public class ItemDataInfo
{
    public Int64 player_id { get; set; }
    public Int32 item_code { get; set; }
    public Int32 count { get; set; }
    public Int32 attack { get; set; }
    public Int32 defence { get; set; }
    public Int32 magic { get; set; }
    public Int32 enhance_count { get; set; }
}

public class UserInfo
{
    public Int32 Exp { get; set; }
    public Int32 Level { get; set; }
    public Int32 Hp { get; set; }
    public Int32 Mp { get; set; }
    public Int64 Gold { get; set; }
}