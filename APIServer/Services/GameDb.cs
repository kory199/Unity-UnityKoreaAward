using Microsoft.Extensions.Options;
using System.Data;

namespace APIServer.Services;

public class GameDb
{
    private readonly ILogger<GameDb> _logger;
    private readonly IOptions<DbConfig> _dbConfig;
    private IDbConnection _dbConn;

    private SqlKata.Compilers.MySqlCompiler _compiler;

}