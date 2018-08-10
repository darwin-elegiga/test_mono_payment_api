using System;
using System.Data;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using VPay.Payment.Common;
using VPay.Payment.Common.MySql;

namespace VPay.Payment.MySql
{
    public class MySqlPaymentOps : IMySqlPaymentOps, IHealthCheck
    {
        private readonly MySqlConnectionConfig _connectionConfig;
        private MySqlConnection _dbConnection;

        public MySqlPaymentOps(MySqlConnectionConfig connectionConfig)
        {
            _connectionConfig = connectionConfig ?? throw new ArgumentNullException(nameof(connectionConfig));

        }

        public string Component { get; } = "MySql";

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

        private async Task<MySqlConnection> GetOpenConnection()
        {

            if (_dbConnection == null)
            {
                _dbConnection = new MySqlConnection(ConvertToConnectionString(_connectionConfig));
                await _dbConnection.OpenAsync();
            }

            return _dbConnection;
        }

        private string ConvertToConnectionString(MySqlConnectionConfig settings)
        {
            Enum.TryParse(settings.SslMode, out MySqlSslMode sslMode);

            var sb = new MySqlConnectionStringBuilder
            {
                Server = settings.Hostname,
                Port = settings.Port,
                UserID = settings.UserName,
                Password = settings.Password,
                Database = settings.Database,
                SslMode = sslMode,
                CheckParameters = settings.CheckParameters
            };
            return sb.ConnectionString;
        }

    }
}
