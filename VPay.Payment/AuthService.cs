using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.Security;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class AuthService : IAuthService
    {
        private readonly IDb2Context _db;
        private readonly ILogger<AuthService> _logger;
        private readonly PaymentConfig _config;

        public AuthService(IDb2Context db, ILogger<AuthService> logger, PaymentConfig config)
        {
            _db = db;
            _config = config;

            _logger = logger;
        }

        public async Task<AuthenticationResult> TestAuthentication(AuthenticationParam param)
        {
            param.Id = param.Id ?? "";
            param.PassPhrase = param.PassPhrase ?? "";

            if (param.Id.Length > 10 || param.PassPhrase.Length > 10)
            {
                _logger.LogWarning("Id or Passphrase is greather than 10 characters: {Id}", param.Id);
            }

            var result = await _db.GetRepository<ISecurity>().AuthenticateWebUserAsync(new AuthenticateUserParam()
            {
                UserId = param.Id,
                Password = param.PassPhrase,
                IpAddress = param.IpAddress
            });

            return new AuthenticationResult()
            {
                Result = result.ReturnCode,
                ErrorMessage = result.ErrorMessage
            };
        }

        public async Task<UserSessionInfo> Login(AuthenticationParam param)
        {
            _logger.LogInformation($"AuthService: Login: ValidateIP");

            if (_config.ValidateIP)
            {
                var ipValidate = await TestAuthentication(param);
                if (ipValidate == null || ipValidate.Result != null)
                {
                    return null;
                }
            }

            UserSessionInfo userSession = null;


            _logger.LogInformation($"AuthService: Try Login for {param.UserId}");

            var loginTry = await DoLogin(param.UserId, param.Password, "WEBSERVICE");

            if (loginTry != null)
            {
                userSession = loginTry;
                userSession.Source = 'S';
            }


            _logger.LogInformation($"AuthService: Completed Login for {param.UserId}");

            return userSession;
        }

        public async Task<bool> IsAuthorized(string userId, string webServiceName, string action)
        {
            var secObjName = $"UNIVERSE|WS_PUBLIC|{webServiceName}".ToUpper();

            var result = await _db.GetRepository<ISecurity>().SecurityCheckAsync(new SecurityCheckParam()
            {
                UserId = userId,
                Action = action,
                SecurityObjectName = secObjName
            });

            _logger.LogInformation($"AuthService: IsAuthorized: Result:{result?.Result}, Description:{result?.Description}");

            return result?.Result == "0000";
        }

        public async Task<UserSessionInfo> DoLogin(string name, string password, string source)
        {
            // Force userid to uppercase
            name = name.ToUpper();

            if (name.Length > 10 || password.Length > 10)
            {
                _logger.LogWarning("UserName or Password is greather than 10 characters: {UserName}", name);
                return null;
            }

            _logger.LogInformation($"AuthService: DoLogin: Starting for {name}");

            var remoteLogin = await _db.GetRepository<ISecurity>().RemoteLoginAsync(new RemoteLoginParam()
            {
                UserId = name,
                Password = password,
                Source = source
            });

            _logger.LogInformation($"AuthService: DoLogin: ReturnCode: {remoteLogin.ReturnCode}, ErrorMessage: {remoteLogin.ErrorMessage}");

            if (!string.Equals(remoteLogin.ReturnCode, "OK"))
            {
                _logger.LogWarning("Error Response from login [{ErrorMessage}] - Name: {UserName}", remoteLogin.ErrorMessage, name);
                return null;
            }

            return new UserSessionInfo
            {
                UserName = name,
                Token = remoteLogin.Token
            };
        }
    }
}
