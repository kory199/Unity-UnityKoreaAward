namespace UnityKoreaAward_APIServer.DBModel;

public class Account
{
    public Int64 account_id { get; set; } = 0;
    public String salt_value { get; set; } = "";
    public String hashed_password { get; set; } = "";
}
