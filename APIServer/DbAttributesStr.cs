namespace APIServer;

public class AccountDbTable
{
    public const String Account = "account";
    public const String id = "id";
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

public class StageTable
{
    public const String player_stage = "player_stage";
    public const String stage_id = "stage_id";
    public const String is_achieved = "is_achieved";
}

public class StageInfoTable
{
    public const String stage_id = "stage_id";
    public const String prev_stage_id = "prev_stage_id";
}