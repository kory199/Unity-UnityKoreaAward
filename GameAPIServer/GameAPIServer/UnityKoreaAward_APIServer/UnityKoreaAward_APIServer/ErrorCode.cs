namespace UnityKoreaAward_APIServer;

public enum ErrorCode
{
    None = 0,
    
    // Version Check 100~ === 


    // === Account 200~ ===
    CreateAccountFailInsert = 200,
    CreateAccountFailException = 201,
    LoginFailUserNotExist = 202,
    LoginFailException = 203,
    LoginFailPwNotMatch = 204,
    LoginFailSetAuthToken = 205,

    // === User Game Data ===
}