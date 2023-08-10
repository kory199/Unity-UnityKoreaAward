namespace APIServer.Services;

public class PingMessage
{
    public const String PlayGame = "PlayerGame";
    public const String InternetError = "InternetError";
    public const String PlayerQuit = "PlayerQuit";
    public const String Crash = "Crash";
    public const String NormalExit = "NormalExit";
}

public enum PingInfo
{
    PlayGame,
    InternetError,
    PlayerQuit,
    Crash,
    NormalExit
}