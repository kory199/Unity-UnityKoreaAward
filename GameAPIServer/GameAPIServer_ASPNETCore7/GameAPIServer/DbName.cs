using System;

namespace GameAPIServer;

public class AccountDbTable
{
    public const string Account = "account";
}

public class GameDbTable
{
    public const string player_data = "player_data";
    public const string player_item = "player_item";
    public const string attendance = "attendance";
    public const string mail = "mail";
    public const string inapp = "inapp";
    public const string masterdata_version = "masterdata_version";
    public const string player_version = "player_version";
    public const string dungeon_stage = "dungeon_stage";
}

public class MasterDataTable
{
    public const string item = "item";
    public const string item_attribute = "item_attribute";
    public const string attendance_reward = "attendance_reward";
    public const string in_app_product = "in_app_product";
    public const string stage_item = "stage_item";
    public const string stage_attack_npc = "stage_attack_npc";
}

public class VersionDbTable
{
    public const string masterdata_ver = "masterdata_ver";
    public const string game_ver = "game_ver";
    public const string created_at = "created_at";
}

public class DbColumn
{

    public const string player_id = "player_id";

    public const string code = "code";
    public const string name = "name";
    public const string count = "count";

    public const string attribute = "attribute";

    public const string item_code = "item_code";
    public const string item_name = "item_name";
    public const string item_count = "item_count";

    public const string exp = "exp";

    public const string receipt_number = "receipt_number";
}