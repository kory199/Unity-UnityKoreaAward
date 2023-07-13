using Microsoft.Extensions.Options;
using SqlKata.Execution;
using System.Data;

namespace APIServer.Services;

public abstract class BaseDb
{
    protected readonly ILogger _logger;
    protected IDbConnection _dbConn;
    protected readonly IDbConnectionHandler _dbConnectionHandler;
    protected SqlKata.Compilers.MySqlCompiler _compiler;
    protected QueryFactory _queryFactory;

    public BaseDb(ILogger logger, String connectionString)
    {
        _logger = logger;
        _dbConnectionHandler = new MySqlConnectionHandler(connectionString);
        _dbConn = _dbConnectionHandler.GetConnection();
        _compiler = new SqlKata.Compilers.MySqlCompiler();
        _queryFactory = new SqlKata.Execution.QueryFactory(_dbConn, _compiler);
    }
}