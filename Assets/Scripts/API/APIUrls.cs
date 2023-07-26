using System.Collections.Generic;

public class APIUrls
{
    private static readonly string url = "https://0fcf-101-235-202-157.ngrok-free.app/";
    public static readonly string CreateAccountApi = url + "CreateAccount";
    public static readonly string LoginApi = url + "Login";
    public static readonly string GameDataApi = url + "GameData";
    public static readonly string RankingApi = url + "Ranking";

    private static HashSet<string> validUrls = new HashSet<string>
    {
        CreateAccountApi,
        LoginApi,
        GameDataApi,
        RankingApi
    };

    public static bool IsValidUrl(string url)
    {
        return validUrls.Contains(url);
    }
}