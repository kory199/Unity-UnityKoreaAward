namespace APIServer.DbModel;

public class Account
{
    public Int64 account_id { get; set; }
    public String id { get; set; } = "";
    public String salt_value { get; set; } = "";
    public String hashed_password { get; set; } = "";
}