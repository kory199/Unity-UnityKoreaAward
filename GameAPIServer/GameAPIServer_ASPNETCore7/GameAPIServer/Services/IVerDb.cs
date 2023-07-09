using System;
namespace GameAPIServer.Services;

public interface IVerDb
{
    public Task<ErrorCode> VerifyMasterDataVer();

    public Task<ErrorCode> VerifyGameVer();

    public String VersionStr(Int32 version);

    public String GetGameVersion();

    public String GetMasterDataVersion();
}