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

        public async Task<AuthenticationResult> AuthenticateUser(AuthenticationParam param)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL SEWADM.SP_AUTHWEB(?,?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PAUTHCODE", param.Id);
                cmd.Parameters.AddWithValue("PPWD", param.PassPhrase);
                cmd.Parameters.AddWithValue("PRMTADDR", param.IpAddress);
                cmd.Parameters.AddWithValue("PREQBY", "WEBSERVICE");

                var errorMessageParam = cmd.Parameters.Add("ERRMSG", OdbcType.Char, 256);
                errorMessageParam.Direction = ParameterDirection.InputOutput;
                errorMessageParam.Value = "";

                var resultParam = cmd.Parameters.Add("PRCOD", OdbcType.Char, 5);
                resultParam.Direction = ParameterDirection.InputOutput;
                resultParam.Value = "";

                await cmd.ExecuteNonQueryAsync();
                
                return new AuthenticationResult()
                {
                    Result = resultParam.Value?.ToString().Trim(),
                    ErrorMessage = errorMessageParam.Value?.ToString().Trim()
                };
            }

        }

        public async Task<RemoteLoginResult> RemoteLogin(string username, string password, string source)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL VPAYSEC.VPAY_REMOTE_LOGIN(?,?,?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                
                cmd.Parameters.AddWithValue("USERID", username);
                cmd.Parameters.AddWithValue("PASSWORD", password);

                var resultParam = cmd.Parameters.Add("RETURNCODE", OdbcType.Char, 10);
                resultParam.Direction = ParameterDirection.InputOutput;
                resultParam.Value = "";

                var errorMessageParam = cmd.Parameters.Add("ERRMSG", OdbcType.Char, 256);
                errorMessageParam.Direction = ParameterDirection.InputOutput;
                errorMessageParam.Value = "";


                cmd.Parameters.AddWithValue("GUID", "");

                var tokenParam = cmd.Parameters.Add("TOKEN", OdbcType.Char, 128);
                tokenParam.Direction = ParameterDirection.InputOutput;
                tokenParam.Value = "";

                cmd.Parameters.AddWithValue("SOURCE", source);


                await cmd.ExecuteNonQueryAsync();

                return new RemoteLoginResult()
                {
                    ReturnCode = resultParam.Value?.ToString().Trim(),
                    ErrorMessage = errorMessageParam.Value?.ToString().Trim(),
                    Token = tokenParam.Value?.ToString().Trim()
                };
            }

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
            if (string.IsNullOrWhiteSpace(connectionConfig.DefaultLibraries))
            {

                return $"DSN={connectionConfig.Dsn};UID={connectionConfig.UserName};PWD={connectionConfig.Password};System={connectionConfig.Hostname}";
            }

            return $"DSN={connectionConfig.Dsn};UID={connectionConfig.UserName};PWD={connectionConfig.Password};System={connectionConfig.Hostname};DefaultLibraries={connectionConfig.DefaultLibraries}";
        }
        
    }
}
