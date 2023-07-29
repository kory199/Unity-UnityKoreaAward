namespace APIServer.Services;

public interface IVerDb
{
    public Task<ResultCode> VerifyGmaeVer();

    public String VersionStr(Int32 version);

    public String GetGameVersiom();
}