using System;
using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;
using VPay.Payment.Common;
using VPay.Payment.Common.Db2;

namespace VPay.Payment.Db2
{
    public class DbPaymentOps : IDbPaymentOps, IHealthCheck
    {
        private readonly Db2ConnectionConfig _connectionConfig;
        private OdbcConnection _dbConnection;
        
        public DbPaymentOps(Db2ConnectionConfig connectionConfig)
        {
            _connectionConfig = connectionConfig ?? throw new ArgumentNullException(nameof(connectionConfig));
            
        }

        public string Component { get; } = "Db2";

        public async Task<bool> IsHealthy()
        {
            try
            {
                var connection = await GetOpenConnection();
                return connection.State == ConnectionState.Open;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public void Dispose()
        {
            _dbConnection?.Close();
            _dbConnection?.Dispose();
        }

        private async Task<OdbcConnection> GetOpenConnection()
        {

            if (_dbConnection == null)
            {
                _dbConnection = new OdbcConnection(ConvertToConnectionString(_connectionConfig));
                await _dbConnection.OpenAsync();
            }

            return _dbConnection;
        }

        private string ConvertToConnectionString(Db2ConnectionConfig connectionConfig)
        {
            return $"DSN={connectionConfig.Dsn};UID={connectionConfig.UserName};PWD={connectionConfig.Password};System={connectionConfig.Hostname}";
        }

    }
}
