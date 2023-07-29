using APIServer.DbModel;
using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System;

namespace APIServer.Services;

public class MasterDataDb : BaseDb<MasterData>, IMasterDataDb
{
    public MasterDataDb (ILogger<MasterData> logger, IOptions<DbConfig> dbConfig)
        :base(logger, dbConfig.Value.MasterDataDb, MasterData_GameVersion.game_ver)
    {
    }

    public async Task<(ResultCode,String?)> VerifyGmaeVersionAsync()
    {
        var gameVer = await _queryFactory.Query(_tableName)
            .Select(MasterData_GameVersion.version)
            .OrderByDesc(MasterData_GameVersion.version)
            .FirstOrDefaultAsync<Int32>();

        if(gameVer == 0)
        {
            return (ResultCode.LoadGameVersionFail, null);
        }

        String verStr = VersionStr(gameVer);
        return (ResultCode.None, verStr);
    }

    private String VersionStr(Int32 verNum)
    {
        string versionStr = $"{verNum / 10000}.{(verNum % 10000) / 1000}.{verNum % 10}";
        return versionStr;
    }
}