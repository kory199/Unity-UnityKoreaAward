using System.Data;

namespace APIServer.Services;

public interface IDbConnectionHandler : IDisposable
{
    public IDbConnection GetConnection();
    public void Open();
    public void Close();
}