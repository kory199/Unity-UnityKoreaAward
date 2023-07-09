using GameAPIServer.StateType;

namespace GameAPIServer.DBModel;

public class Mail
{
    public Int64 code { get; set; }
    public Int64 player_id { get; set; }
    public String title { get; set; } = "";
    public String content { get; set; } = "";
    public Int64 exp { get; set; }
    public MailType type { get; set; }
    public MailStateType StateType { get; set; }
    public Int32 item_code { get; set; }
    public Int32 count { get; set; }
    public DateTime created_at { get; set; }
}

public class LoadMail
{
    public String title { get; set; } = "";
    public Int64 exp { get; set; }
}

public class ReadMailContent
{
    public String title { get; set; } = "";
    public String content { get; set; } = "";
}