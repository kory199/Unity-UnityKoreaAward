namespace GameAPIServer.DBModel;

public class Item
{
    public Int32 code { get; set; }
    public String name { get; set; }
    public Int32 attribute { get; set; }
    public Int64 sell { get; set; }
    public Int64 buy { get; set; }
    public Int32 use_lv { get; set; }
    public Int64 attack { get; set; }
    public Int64 defence { get; set; }
    public Int64 magic { get; set; }
    public Int32 enhance_max_count { get; set; }
}

public class ItemAttribute
{
    public String name { get; set; }
    public Int32 code { get; set; }
}

public class AttendanceReward
{
    public Int32 code { get; set; }
    public Int32 item_code { get; set; }
    public Int32 count { get; set; }
}

public class InAppProduct
{
    public Int32 code { get; set; }
    public Int32 item_code { get; set; }
    public String item_name { get; set; } = "";
    public Int64 item_count { get; set; }
}

public class StageItem
{
    public Int32 code { get; set; }
    public Int32 item_code { get; set; }
}

public class StageAttackNpc
{
    public Int32 code { get; set; }
    public Int32 npc_code { get; set; }
    public Int32 count { get; set; }
    public Int32 exp { get; set; }
}