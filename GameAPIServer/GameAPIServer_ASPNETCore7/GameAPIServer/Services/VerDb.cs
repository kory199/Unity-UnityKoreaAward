using GameAPIServer.DBModel;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using System.Drawing.Printing;
using ZLogger;

namespace GameAPIServer.Services;

public class VerDb : IVerDb
{
    private readonly ILogger<VerDb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    private static String gameVersion;
    private static String masterDataVersion;

    public VerDb(ILogger<VerDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> VerifyMasterDataVer()
    {
        try
        {
            var masterVer = await _queryFactory.Query(VersionDbTable.masterdata_ver)
                .Select(VersionDbTable.masterdata_ver)
                .OrderByDesc(VersionDbTable.masterdata_ver).FirstAsync<Int32>();

            if(masterVer == null)
            {
                _logger.ZLogError($"[VersionDb.VerifyMasterDataVer] ErrorCode : {ErrorCode.VerifyMasterDataFail}, masterDataVersion :{masterVer}");
            }

            var masterdataVer = VersionStr(masterVer);
            SetMasterDataVersion(masterdataVer);
            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e, $"[VersionDb.VerifyMasterDataVer] ErrorCode : {ErrorCode.VerifyMasterDataFailException}");
            
           return ErrorCode.VerifyMasterDataFailException;
        }
    }

    public async Task<ErrorCode> VerifyGameVer()
    {
        try
        {
            var gameVer = await _queryFactory.Query(VersionDbTable.game_ver)
                .Select(VersionDbTable.game_ver)
                .OrderByDesc(VersionDbTable.game_ver).FirstOrDefaultAsync<Int32>();

            if (gameVer == 0)
            {
                _logger.ZLogError($"[VersionDb.VerifyGameVer] ErrorCode : {ErrorCode.VerifyMasterDataFail}, masterDataVersion :{gameVer}");
            }

            var verStr = VersionStr(gameVer);
            SetGameVersion(verStr);
            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[VersionDb.VerifyGameVer] ErrorCode : {ErrorCode.VerifyMasterDataFailException}");

            return ErrorCode.VerifyMasterDataFailException;
        }
    }

    public String VersionStr(Int32 version)
    {
        string versionString = $"{version / 10000}.{(version % 1000) / 10}.{version % 10}";
        return versionString;
    }

    public String GetGameVersion() => gameVersion;
    private String SetGameVersion(String ver) => gameVersion = ver;

    public String GetMasterDataVersion() => masterDataVersion;
    private String SetMasterDataVersion(String ver) => masterDataVersion = ver; 

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.VerDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();
    
    public void Dispose() => Close();
}