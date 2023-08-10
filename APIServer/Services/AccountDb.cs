using APIServer.DbModel;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SqlKata;
using SqlKata.Execution;
using System.Data;
using ZLogger;

namespace APIServer.Services;

public class AccountDb : BaseDb<Account>, IAccountDb
{
    public AccountDb(ILogger<AccountDb> logger, IOptions<DbConfig> dbConfig)
        : base(logger, dbConfig.Value.AccountDb, AccountDbTable.Account)
    {
    }

    public async Task<ResultCode> CreateAccountAsync(String id, String pw)
    {
        try
        {
            var saltValue = Security.SaltString();
            var hashingPassword = Security.MakeHashingPassWord(saltValue, pw);

            _logger.ZLogDebug($"[{GetType().Name}.CreateAccountAsync] ID: {id}, SaltValue : {saltValue}, HashingPassword:{hashingPassword}");

            var account = new Account
            {
                id = id,
                salt_value = saltValue,
                hashed_password = hashingPassword
            };

            return await ExecuteInsertAsync(account);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.CreateAccountAsync] ResultCode : {ResultCode.FailedtoCreateAccount}");

            return ResultCode.FailedtoCreateAccount;
        }
    }

    public async Task<(ResultCode, Int64)> VerifyAccountAsync(String id, String pw)
    {
        try
        {
            var accountInfo = await _queryFactory.Query(_tableName)
                          .Where(AccountDbTable.id, id)
                          .WhereRaw("BINARY " + AccountDbTable.id + " = ?", id)
                          .FirstOrDefaultAsync<Account>();

            if (accountInfo == null || accountInfo.account_id == 0)
            {
                return (ResultCode.LoginFailUserNotExist, 0 );
            }

            var hashingPassword = Security.MakeHashingPassWord(accountInfo.salt_value, pw);
            
            if (accountInfo.hashed_password != hashingPassword)
            {
                _logger.ZLogError($"[{GetType().Name}.VerifyAccountAsync] ResultCode : {ResultCode.LoginFailPwNotMatch}, ID : {id}");

                return (ResultCode.LoginFailPwNotMatch, 0);
            }

            return (ResultCode.None, accountInfo.account_id);
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.VerifyAccountAsync] ResultCode : {ResultCode.LoginFailException}, ID : {id}");

            return (ResultCode.LoginFailException, 0);
        }
    }

    public void Dispose() => _dbConnectionHandler.Dispose();
}