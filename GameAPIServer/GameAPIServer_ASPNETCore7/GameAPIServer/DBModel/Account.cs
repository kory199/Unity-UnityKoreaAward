namespace GameAPIServer.DBModel;

public class Account
{
    private Int64 account_id;
    private String salt_value = "";
    private String hashed_password = "";

    public Int64 GetAccountId() => account_id;
    public Int64 SetAccountId(Int64 value) => account_id = value;

    public String GetHashedPassword() => hashed_password;
    public void SetHashedPassword(String value) => hashed_password = value;

    public String GetSaltValue() => salt_value;
    public void SetSaltValue(String value) => salt_value = value; 
}