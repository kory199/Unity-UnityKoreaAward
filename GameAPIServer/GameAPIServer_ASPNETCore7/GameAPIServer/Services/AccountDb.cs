using System.Data;
using GameAPIServer.DBModel;
using Microsoft.Extensions.Options;
using MySqlConnector;
using SqlKata.Execution;
using ZLogger;

namespace GameAPIServer.Services;

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

        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }

    public async Task<ErrorCode> CreateAccountAsync(String id, String pw)
    {
        try
        {
            var saltValue = Security.SaltString();

            var hashingPassword = Security.MakeHashingPassWord(saltValue, pw);

            _logger.ZLogDebug(
                $"[CreateAccount] ID: {id}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");

            var count = await _queryFactory.Query(AccountDbTable.Account).InsertAsync(new {
                id = id,
                salt_value = saltValue,
                hashed_password = hashingPassword
            });

            if(count != 1)
            {
                return ErrorCode.CreateAccountFailInsert; 
            }

            return ErrorCode.None;
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[AccountDb.CreateAccount] ErrorCode : {ErrorCode.CreateAccountFailException}, ID :{id}");

            return ErrorCode.CreateAccountFailException;
        }
    }

    public async Task<Tuple<ErrorCode, Int64>> VerifyAccount(string id, string pw)
    {
        try
        {
            var accountInfo = await _queryFactory.Query(AccountDbTable.Account).Where("id", id).FirstOrDefaultAsync<Account>();

            if(accountInfo == null || accountInfo.GetAccountId() == 0)
            {
                return new Tuple<ErrorCode, Int64>(ErrorCode.LoginFailUserNotExist, 0);
            }

            var hashingPasswrod = Security.MakeHashingPassWord(accountInfo.GetSaltValue(), pw);

            if(accountInfo.GetHashedPassword() != hashingPasswrod)
            {
                _logger.ZLogError(
                    $"[AccountDb.VerifyAccount] ErrorCode : {ErrorCode.LoginFailPwNotMatch}, ID : {id}");

                return new Tuple<ErrorCode, Int64>(ErrorCode.LoginFailPwNotMatch, 0);
            }

            return new Tuple<ErrorCode, Int64>(ErrorCode.None, accountInfo.GetAccountId());
        }
        catch(Exception e)
        {
            _logger.ZLogError(e,
                $"[AccountDb.VerifyAccount] ErrorCode : {ErrorCode.LoginFailException}, ID : {id}");

            return new Tuple<ErrorCode, Int64>(ErrorCode.LoginFailException, 0);
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