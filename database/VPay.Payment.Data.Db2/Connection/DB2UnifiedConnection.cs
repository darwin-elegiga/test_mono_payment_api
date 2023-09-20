using System;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Threading;
using System.Threading.Tasks;
using IBM.Data.Db2;
using Microsoft.Extensions.Logging;
// ReSharper disable InconsistentNaming

namespace VPay.Payment.Data.Db2.Connection;

public class DB2UnifiedConnection : IDisposable
{
    private readonly bool _isLocal;
    private bool _disposed;
    private ILogger Logger { get; }

    /// <summary>
    /// This is the passed in Connection string
    /// </summary>
    protected string ConnectionString { get; }

    /// <summary>
    /// This is the DbConnection 
    /// </summary>
    protected DbConnection? DbConnection { get; private set; }

    public DB2UnifiedConnection(string connectionString, bool isLocal, ILogger logger)
    {
        _isLocal = isLocal;
        ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        if (string.IsNullOrWhiteSpace(ConnectionString))
        {
            throw new ArgumentException("Must have a value", nameof(connectionString));
        }

        Logger = logger;
    }

    /// <summary>
    /// This will create a connection object
    /// </summary>
    /// <returns></returns>
    protected DbConnection GetConnectionObject()
    {
        return _isLocal
            ? new DB2Connection(ConnectionString)
            : new OdbcConnection(ConnectionString);
    }

    /// <summary>
    /// This will open a connection to the database asynchronously
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>This returns an open connection of type IDbConnection</returns>
    public async Task<DbConnection> GetOpenConnectionAsync(CancellationToken cancellationToken = default)
    {
        if (DbConnection == null || DbConnection.State == ConnectionState.Closed)
        {
            DbConnection = GetConnectionObject();
            await DbConnection.OpenAsync(cancellationToken);
        }

        return DbConnection;
    }

    /// <summary>
    /// This will test if a connection can be opened to the database
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>This returns true if a connection to the database can be opened</returns>
    public async Task<bool> CanConnectAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var connection = await GetOpenConnectionAsync(cancellationToken);
            return connection.State == ConnectionState.Open;
        }
        catch (Exception ex)
        {

            Logger.LogInformation(ex, "Could not connect to the DB2 Database");
            return false;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    // Protected implementation of Dispose pattern.
    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            DisposeCurrentConnection();
        }

        _disposed = true;
    }

    protected virtual void DisposeCurrentConnection()
    {
        DbConnection?.Close();
        DbConnection?.Dispose();

        DbConnection = null;
    }
}
