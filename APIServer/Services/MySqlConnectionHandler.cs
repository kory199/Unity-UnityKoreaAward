using Microsoft.Extensions.Options;
using MySqlConnector;
using System.Data;

namespace APIServer.Services;

public class MySqlConnectionHandler : IDbConnectionHandler
{
    private IDbConnection _dbConn;
    private String _connectionString;

    public MySqlConnectionHandler(String connectionString)
    {
        _dbConn = new MySqlConnection(connectionString);
        _connectionString = connectionString;
    }

    public IDbConnection GetConnection()
    {
        if(_dbConn == null)
        {
            _dbConn = new MySqlConnection(_connectionString);
        }

        return _dbConn;
    }

    public void Open()
    {
        if(_dbConn.State != ConnectionState.Open)
        {
            _dbConn.Open();
        }
    }

    public void Close()
    {
        if(_dbConn.State != ConnectionState.Closed)
        {
            _dbConn.Close();
        }
    }

    public void Dispose()
    {
        Close();
        _dbConn?.Dispose();
    }
}