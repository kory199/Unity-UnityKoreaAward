using APIServer.DbModel;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using System.Data;
using ZLogger;

namespace APIServer.Services;

public class AccountDb : IAccountDb
{
    private readonly IOptions<DbConfig> _dbConfig;
    private readonly ILogger<AccountDb> _logger;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;
    private QueryFactory _queryFactory;

    public AccountDb(ILogger<AccountDb> logger, IOptions<DbConfig> dbConfig)
    {
        _dbConfig = dbConfig;
        _logger = logger;

        Open();

        if (_dbConn == null)
        {
            throw new Exception("_dbConn Is Still Null After Calling Open()");
        }

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ResultCode> CreateAccountAsync(String id, String pw)
    {
        try
        {
            var saltValue = Security.SaltString();

            var hashingPassword = Security.MakeHashingPassWord(saltValue, pw);

            _logger.ZLogDebug($"[CreateAccount] ID: {id}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");

            var count = await _queryFactory.Query(AccountDbTable.Account).InsertAsync(new
            {
                id = id,
                salt_value = saltValue,
                hashed_password = hashingPassword
            });

            if (count != 1)
            {
                return ResultCode.CreateAccountFailInsert;
            }

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[AccountDb.CreateAccountAsync] ResultCode : {ResultCode.FailedtoCreateAccount}");

            return ResultCode.FailedtoCreateAccount;
        }
    }

    public async Task<ResultCode> VerifyAccountAsync(String id, String pw)
    {
        try
        {
            var accountInfo = await _queryFactory.Query(AccountDbTable.Account).Where(AccountDbTable.id).FirstOrDefaultAsync<Account>();

            if (accountInfo is null || accountInfo.account_id == 0)
            {
                return ResultCode.LoginFailUserNotExist;
            }

            var hashingPassword = Security.MakeHashingPassWord(accountInfo.salt_value, pw);

            if (accountInfo.hashed_password != hashingPassword)
            {
                _logger.ZLogError($"[AccountDb.VerifyAccountAsync] ResultCode : {ResultCode.LoginFailPwNotMatch}, ID : {id}");

                return ResultCode.LoginFailPwNotMatch;
            }

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[AccountDb.VerifyAccountAsync] ResultCode : {ResultCode.LoginFailException}, ID : {id}");

            return ResultCode.LoginFailException;
        }
    }

    private void Open()
    {
        _dbConn = new MySqlConnection(_dbConfig.Value.AccountDb);
        _dbConn.Open();
    }

    private void Close() => _dbConn.Close();

    public void Dispose() => Close();
}