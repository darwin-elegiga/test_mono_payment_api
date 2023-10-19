using System.Data;
using System.Data.Odbc;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.Security;

namespace VPay.Payment.Api;

/// <inheritdoc />
/// <summary>
/// </summary>
public class Security : ISecurity
{
    private readonly IDataConnection<OdbcConnection> _connection;
    private readonly ILogger<Security> _logger;

    public Security(IDataConnection<OdbcConnection> connection, ILogger<Security> logger)
    {
        _connection = connection;
        _logger = logger;
    }

    public async Task<AuthenticateUserResult> AuthenticateWebUserAsync(AuthenticateUserParam param,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var connection = await _connection.GetOpenConnectionAsync(cancellationToken);

        using (var cmd = new OdbcCommand("CALL SEWADM.SP_AUTHWEB(?,?,?,?,?,?)", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("PAUTHCODE", param.UserId);
            cmd.Parameters.AddWithValue("PPWD", param.Password);
            cmd.Parameters.AddWithValue("PRMTADDR", param.IpAddress);
            cmd.Parameters.AddWithValue("PREQBY", "WEBSERVICE");

            var errorMessageParam = cmd.Parameters.Add("ERRMSG", OdbcType.Char, 256);
            errorMessageParam.Direction = ParameterDirection.InputOutput;
            errorMessageParam.Value = "";

            var resultParam = cmd.Parameters.Add("PRCOD", OdbcType.Char, 5);
            resultParam.Direction = ParameterDirection.InputOutput;
            resultParam.Value = "";

            await cmd.ExecuteNonQueryAsync(cancellationToken);

            return new AuthenticateUserResult
            {
                ReturnCode = resultParam.Value?.ToString().Trim(),
                ErrorMessage = errorMessageParam.Value?.ToString().Trim()
            };
        }

    }

    public async Task<RemoteLoginResult> RemoteLoginAsync(RemoteLoginParam param, CancellationToken cancellationToken = default(CancellationToken))
    {
        _logger.LogInformation("Security: Starting to Connect");
        var connection = await _connection.GetOpenConnectionAsync(cancellationToken);
        _logger.LogInformation("Security: Starting to Connected");

        using (var cmd = new OdbcCommand("CALL VPAYSEC.VPAY_REMOTE_LOGIN(?,?,?,?,?,?,?)", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("USERID", param.UserId);
            cmd.Parameters.AddWithValue("PASSWORD", param.Password);

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

            cmd.Parameters.AddWithValue("SOURCE", param.Source);

            await cmd.ExecuteNonQueryAsync(cancellationToken);

            _logger.LogInformation("Security: Authenticated");

            return new RemoteLoginResult()
            {
                ReturnCode = resultParam.Value?.ToString().Trim(),
                ErrorMessage = errorMessageParam.Value?.ToString().Trim(),
                Token = tokenParam.Value?.ToString().Trim()
            };
        }
    }

    public async Task<SecurityCheckResult> SecurityCheckAsync(SecurityCheckParam param,
        CancellationToken cancellationToken = default(CancellationToken))
    {
        var connection = await _connection.GetOpenConnectionAsync(cancellationToken);

        using (var cmd = new OdbcCommand("CALL VPAYSEC.SP_Check_Object_Security(?,?,?,?,?)", connection))
        {
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("PUSERNM", param.UserId.ToUpper());
            cmd.Parameters.AddWithValue("PPATH", param.SecurityObjectName.ToUpper());
            cmd.Parameters.AddWithValue("PACTION", param.Action.ToUpper());

            var resultParam = cmd.Parameters.Add("PRETCODE", OdbcType.Char, 4);
            resultParam.Direction = ParameterDirection.InputOutput;
            resultParam.Value = "0000";

            var descParameter = cmd.Parameters.Add("PRETDESC", OdbcType.Char, 256);
            descParameter.Direction = ParameterDirection.InputOutput;
            descParameter.Value = "";

            await cmd.ExecuteNonQueryAsync(cancellationToken);

            return new SecurityCheckResult()
            {
                Result = resultParam.Value?.ToString().Trim(),
                Description = descParameter.Value?.ToString().Trim()
            };
        }

    }
}
