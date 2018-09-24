using System.Data;
using System.Data.Odbc;
using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions;
using VPay.Payment.Common;
using VPay.Payment.Common.Db2;

namespace VPay.Payment.Db2
{
    public class DbPaymentOps : IDbPaymentOps, IHealthCheck
    {
        private readonly IDataConnection<OdbcConnection> _connection;
        private readonly IDb2Context _db2Context;

        public DbPaymentOps(IDataConnection<OdbcConnection> connection, IDb2Context context)
        {
            _connection = connection;
            _db2Context = context;

        }

        public string Component { get; } = "Db2";

        public async Task<bool> IsHealthy()
        {
            return await _db2Context.CanConnectAsync();
        }
        
        public void Dispose()
        {
            
        }

        public async Task<string> BalanceRequest(string auth, string password, string ip, string data)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYPGM.SP_WSBALREQUEST(?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHID", auth);
                cmd.Parameters.AddWithValue("PPWD", password);
                cmd.Parameters.AddWithValue("PIP", ip);
                cmd.Parameters.AddWithValue("PREQSTR", data);

                var descParameter = cmd.Parameters.Add("PRSPSTR", OdbcType.Char, 4000);
                descParameter.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return descParameter.Value?.ToString().Trim();
            }
        }

        public async Task<string> GetPan(string auth, string password, string ip, string data)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYPGM.SP_WSGETPANNUM(?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHID", auth);
                cmd.Parameters.AddWithValue("PPWD", password);
                cmd.Parameters.AddWithValue("PIP", ip);
                cmd.Parameters.AddWithValue("PREQSTR", data);

                var descParameter = cmd.Parameters.Add("PRSPSTR", OdbcType.Char, 4000);
                descParameter.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return descParameter.Value?.ToString().Trim();
            }
        }

        public async Task<string> LoadPan(string auth, string password, string ip, string data)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYPGM.SP_WSLOADPAND(?,?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHID", auth);
                cmd.Parameters.AddWithValue("PPWD", password);
                cmd.Parameters.AddWithValue("PIP", ip);
                cmd.Parameters.AddWithValue("PREQSTR", data);
                string temporaryCustData = new string(' ', 1024);
                cmd.Parameters.AddWithValue("CSTDTA", temporaryCustData); // todo: review better parameter values

                var descParameter = cmd.Parameters.Add("PRSPSTR", OdbcType.Char, 4000);
                descParameter.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return descParameter.Value?.ToString().Trim();
            }
        }

        public async Task<string> OpenPreAuth(string auth, string password, string ip, string data)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYPGM.SP_WSOPNPREAUTH(?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHID", auth);
                cmd.Parameters.AddWithValue("PPWD", password);
                cmd.Parameters.AddWithValue("PIP", ip);
                cmd.Parameters.AddWithValue("PREQSTR", data);

                var descParameter = cmd.Parameters.Add("PRSPSTR", OdbcType.Char, 4000);
                descParameter.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return descParameter.Value?.ToString().Trim();
            }
        }

        public async Task<string> StopPay(string auth, string password, string ip, string data)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYPGM.SP_WSSTOPPAY(?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHID", auth);
                cmd.Parameters.AddWithValue("PPWD", password);
                cmd.Parameters.AddWithValue("PIP", ip);
                cmd.Parameters.AddWithValue("PREQSTR", data);

                var descParameter = cmd.Parameters.Add("PRSPSTR", OdbcType.Char, 4000);
                descParameter.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return descParameter.Value?.ToString().Trim();
            }
        }

        public async Task<string> Unload(string auth, string password, string ip, string data)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYPGM.SP_WSUNLOADPAN(?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHID", auth);
                cmd.Parameters.AddWithValue("PPWD", password);
                cmd.Parameters.AddWithValue("PIP", ip);
                cmd.Parameters.AddWithValue("PREQSTR", data);

                var descParameter = cmd.Parameters.Add("PRSPSTR", OdbcType.Char, 4000);
                descParameter.Direction = ParameterDirection.Output;

                await cmd.ExecuteNonQueryAsync();

                return descParameter.Value?.ToString().Trim();
            }
        }

        private async Task<OdbcConnection> GetOpenConnection()
        {
            return await _connection.GetOpenConnectionAsync();
        }

    }
}
