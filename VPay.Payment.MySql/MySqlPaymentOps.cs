using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using VPay.Payment.Common;
using VPay.Payment.Common.MySql;
using VPay.Payment.Common.Permissions;

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

        public async Task<SessionEntry> GetSessionEntryBySessionId(string sessionId)
        {
            var connResults = new List<SessionEntry>();

            var conn = await GetOpenConnection();

            using (var cmd = new MySqlCommand("GET_SESSION_TABLE", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_Session", sessionId);

                var read = await cmd.ExecuteReaderAsync();

                while (await read.ReadAsync())
                {
                    connResults.Add(new SessionEntry()
                    {
                        Id = read.GetInt32(0),
                        UserName = read.GetStringSafe(1),
                        Token = read.GetStringSafe(2),
                        SessionId = read.GetStringSafe(3),
                        DateHit = read.GetDateTime(4),
                        Active = !read.IsDBNull(5) && read.GetBoolean(5)
                    });
                }

                if (connResults.Count != 1)
                {
                    return null;
                }
            }

            return connResults.FirstOrDefault();
        }

        public async Task<bool> InsertSessionEntry(SessionEntry entity)
        {
            var conn = await GetOpenConnection();

            using (var cmd = new MySqlCommand("CREATE_SESSION_TABLE_RECORD", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_User", entity.UserName);
                cmd.Parameters.AddWithValue("p_Token", entity.Token);
                cmd.Parameters.AddWithValue("p_Session", entity.SessionId);
                cmd.Parameters.AddWithValue("p_Date", entity.DateHit);
                cmd.Parameters.AddWithValue("p_Active", entity.Active);
                cmd.Parameters.AddWithValue("p_Data", entity.Data);

                var read = await cmd.ExecuteNonQueryAsync();

                return true;
            }

        }

        public async Task<Webucf> GetWebUfcByUserName(string userName)
        {
            var connResults = new List<Webucf>();

            var conn = await GetOpenConnection();

            using (var cmd = new MySqlCommand("GET_WEBUCF", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("p_wuuser", userName);

                var read = await cmd.ExecuteReaderAsync();

                while (await read.ReadAsync())
                {
                    connResults.Add(new Webucf()
                    {
                        Keyid = read.GetInt32(0),
                        Wuuser = read.GetStringSafe(1),
                        Wuusrc = read.GetStringSafe(2),
                        Wusessid = read.GetStringSafe(3),
                        Wulast = read.GetNullableDateTime(4),
                        Wupass = read.GetString(5)
                    });
                }

                if (connResults.Count != 1)
                {
                    return null;
                }
            }

            return connResults.FirstOrDefault();
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
                CheckParameters = settings.CheckParameters,
                ConnectionTimeout = 60
            };
            return sb.ConnectionString;
        }

    }
}
