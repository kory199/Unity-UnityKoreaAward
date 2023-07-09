using GameAPIServer.DBModel;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using ZLogger;

namespace GameAPIServer.Services;

public class VersionDb : IVersionDb
{
    private readonly ILogger<VersionDb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public VersionDb(ILogger<VersionDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<Tuple<ErrorCode, Int32>> VerifyMasterDataVersion()
    {
        try
        {
            var masterData_version = await _queryFactory.Query(GameDbTable.masterdata_version).Select("master_version").FirstOrDefaultAsync<MasterData_Version>();

            if (masterData_version == null)
            {
                _logger.ZLogError($"[VersionDb.VerifyMasterDataVersion] ErrorCode : {ErrorCode.VerifyMasterDataFail}, masterDataVersion :{masterData_version.master_version}");

                return new Tuple<ErrorCode, Int32>(ErrorCode.VerifyMasterDataFail, 0);
            }

            return new Tuple<ErrorCode, Int32>(ErrorCode.None, masterData_version.master_version);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[VersionDb.VerifyMasterDataVersion] ErrorCode : {ErrorCode.VerifyMasterDataFailException}");

            return new Tuple<ErrorCode, Int32>(ErrorCode.VerifyMasterDataFailException, 0);
        }
    }

    public async Task<ErrorCode> InsertUserVersion(Int64 user_id, Int32 user_Version)
    {
        try
        {
            var versionInfo = await _queryFactory.Query(GameDbTable.player_version).InsertAsync(new
            {
                player_id = user_id,
                version = user_Version
            });

            if(versionInfo != 1)
            {
                return ErrorCode.CreateVersionException;
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        { 
            _logger.ZLogError(e, $"[VersionDb.InsertUserVersion] ErrorCode : {ErrorCode.CreateVersionException}");

            return ErrorCode.CreateVersionException;
        }
    }

    public async Task<Tuple<ErrorCode, Int32>> VerifyUserVersion(Int64 user_id)
    {
        try
        {
            var userVersionInfo = await _queryFactory.Query(GameDbTable.player_version).Where(DbColumn.player_id, user_id).FirstOrDefaultAsync<User_Version>();

            if(userVersionInfo == null)
            {
                _logger.ZLogError($"[VersionDb.VerifyUserVersion] ErrorCode : {ErrorCode.UserVersionInfoNotFound}, UserID :{user_id}");
                
                return new Tuple<ErrorCode, int>(ErrorCode.UserVersionInfoNotFound, 0);
            }

            return new Tuple<ErrorCode, int>(ErrorCode.None, userVersionInfo.version);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[VersionDb.VerifyUserVersionException] ErrorCode : {ErrorCode.VerifyVersionException}, player_id :{user_id}");

            return new Tuple<ErrorCode, int>(ErrorCode.VerifyVersionException, 0);
        }
    }

    public async Task<ErrorCode> DeleteUserVersion(Int64 user_id)
    {
        try
        {
            await _queryFactory.Query(GameDbTable.player_version).Where(DbColumn.player_id, user_id).DeleteAsync();

            return ErrorCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[VersionDb.DeleteUserVersion] ErrorCode : {ErrorCode.DeleteUserVersionFail}");

            return ErrorCode.DeleteUserVersionFail;
        }
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.GameDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    public void Dispose() => Close();
}