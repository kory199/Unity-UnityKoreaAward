namespace APIServer;

public class MasterDataTable
{
    public const String game_ver = "game_ver";
    public const String monster_data = "monster_data";
    public const String player_status = "player_status";
    public const String stage_spawn_monster = "stage_spawn_monster";
    public const String version = "version";
    public const String id = "id";
}

public class AccountDbTable
{
    public const String Account = "account";
    public const String id = "id";
    public const String AccountId = "account_id";
}

public class ColumnUid
{
    public const String player_uid = "player_uid";
}

public class GameDbTable
{
    public const String player_data = "player_data";
    public const String player_uid = "player_uid";
    public const String id = "id";
    public const String score = "score";
    public const String created_at = "created_at";
}

public class AttendanceTable
{
    public const String player_attendance = "player_attendance";
}

public class StageTable
{
    public const String player_stage = "player_stage";
    public const String stage_id = "stage_id";
    public const String is_achieved = "is_achieved";
}

public class StageInfoTable
{
    public const String stage = "stage";
    public const String stage_id = "stage_id";
    public const String prev_stage_id = "prev_stage_id";
}