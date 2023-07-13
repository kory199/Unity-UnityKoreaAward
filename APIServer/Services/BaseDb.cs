using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System.Data;
using ZLogger;

namespace APIServer.Services;

public abstract class BaseDb<T> where T : class
{
    protected readonly ILogger _logger;
    protected QueryFactory _queryFactory;
    protected IDbConnection _dbConn;
    protected readonly IDbConnectionHandler _dbConnectionHandler;
    protected SqlKata.Compilers.MySqlCompiler _compiler;
    protected readonly String _tableName;

    public BaseDb(ILogger logger, String connectionString, String tableName)
    {
        _logger = logger;
        _dbConnectionHandler = new MySqlConnectionHandler(connectionString);
        _dbConn = _dbConnectionHandler.GetConnection();
        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
        _tableName = tableName;
    }

    protected async Task<ResultCode> ExecuteInsertAsync(T DbModel)
    {
        try
        {
            var count = await _queryFactory.Query(_tableName)
                .InsertAsync(DbModel);

            if (count != 1)
            {
                return ResultCode.InsertException;
            }

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e,
                $"[{GetType().Name}.InsertAsync] ResultCode : {ResultCode.InsertException}");

            return ResultCode.InsertException;
        }
    }

    protected async Task<ResultCode> ExecuteDeleteAsync(String columnName, Object value)
    {
        try
        {
            await _queryFactory.Query()
                .Where(columnName, value)
                .DeleteAsync();

            return ResultCode.None;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.DeleteAsync] ResultCode : {ResultCode.DeleteException}");

            return ResultCode.DeleteException;
        }
    }

    protected async Task<T> ExecuteGetByAsync(String columnName, object value)
    {
        try
        {
            var item = await _queryFactory.Query(_tableName)
                .Where(columnName, value)
                .FirstOrDefaultAsync<T>();

            return item;
        }
        catch (Exception e)
        {
            _logger.ZLogError(e, $"[{GetType().Name}.GetByAsync] ResultCode : {ResultCode.GetByException}");

            return null;
        }
    }
}