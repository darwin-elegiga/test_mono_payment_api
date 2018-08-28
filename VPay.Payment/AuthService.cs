using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Payment.Common;
using VPay.Payment.Common.CommunicationStrings;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;
using VPay.Payment.Common.Login;
using VPay.Payment.Common.Models;
using VPay.Payment.Common.MySql;

namespace VPay.Payment
{
    public class AuthService : IAuthService
    {
        private readonly IDbPaymentOps _db;
        private readonly IMySqlPaymentOps _mySql;
        private readonly ILogger<AuthService> _logger;
        private readonly PaymentConfig _config;

        /// <summary>
        /// This is used to determine if this is suppose to be a session or a single run
        /// </summary>
        private const int _tokenLength = 64;

        public AuthService(IDbPaymentOps db, IMySqlPaymentOps mySql, ILogger<AuthService> logger, PaymentConfig config)
        {
            _db = db;
            _mySql = mySql;
            _config = config;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            _logger = logger;
        }

        public async Task<bool> IsAuthenticated(AuthenticationValues av, string ipAddress)
        {
            var result = await _db.AuthenticateUser(new AuthenticationParam()
            {
                Id = av.Id,
                PassPhrase = av.PassPhrase,
                IpAddress = ipAddress
            });

            return result.Result == "0";
        }

        public async Task<AuthenticationResult> TestAuthentication(AuthenticationParam param)
        {
            var result = await _db.AuthenticateUser(param);

            return result;
        }

        public async Task<UserSessionInfo> Login(AuthenticationParam param)
        {
            if (_config.ValidateIP)
            {
                var ipValidate = await TestAuthentication(param);
                if (ipValidate == null || ipValidate.Result != null)
                {
                    return null;
                }
            }

            UserSessionInfo userSession = null;

            if (param.Token?.Length >= _tokenLength)
            {
                userSession = await GetTokenAndUserId(param.Token);
            }

            if (userSession == null)
            {
                var loginTry = await DoLogin(param.UserId, param.Password, "", "", "WEBSERVICE");

                if (loginTry != null)
                {
                    userSession = await GetTokenAndUserId(loginTry.Uniqueid);
                    userSession.Source = 'S';
                }
            }
            else
            {
                userSession.Source = 'P'; // Set source to web(P)age
            }

            return userSession;
        }

        public async Task<bool> IsAuthorized(string userId, string webServiceName, string action)
        {
            var result = await _db.CheckUserSecurity(new SecurityCheckParam()
            {
                UserId = userId,
                Action = action,
                WebServiceName = webServiceName
            });

            return result?.Code == "0000";
        }

        public async Task<SecurityCheckResult> GetBalanceRequest(StandardRequest sr)
        {
            var stringManipule = new ServiceString(
                sr.CommonData ?? new CommonData(),
                sr.CardData ?? new CardData(),
                sr.CheckData ?? new CheckData(),
                sr.Claim ?? new Claim(),
                sr.CorrespondenceData ?? new CorrespondenceData(),
                sr.CoveredItem ?? new CoveredItem(),
                sr.Merchant ?? new Merchant(),
                sr.Payment ?? new Common.DataWebService.Payment(),
                sr.SwitchTransaction ?? new SwitchTransaction());


            var result = await _db.BalanceRequest("WSQATEST", "QATEST01WS18", "10.120.202.129",
                stringManipule.GenerateStringForISeriesCall());

            return result;
        }

        public async Task<LoginService> DoLogin(string name, string password, string recordid, string recordtype, string source)
        {
            // Force userid to uppercase
            name = name.ToUpper();

            var ucfRecord = await _mySql.GetWebUfcByUserName(name);

            if (ucfRecord == null)
            {
                _logger.LogWarning("Not Found - Name: {UserName}", name);
                //    setLogError("No local entry");
                return null;
            }

            if (ucfRecord.Wupass == null)
            {
                _logger.LogWarning("No Localp - Name: {UserName}", name);
                //    setLogError("No local passwd");
                return null;
            }

            var hashPass = HashPassword(name, password);

            if (!string.Equals(ucfRecord.Wupass, hashPass))
            {
                //    setLogError("Not Allowed");
                _logger.LogWarning("Passed in password does not match - Name: {UserName}", name);
                return null;
            }

            var remoteLogin = await _db.RemoteLogin(name, password, source);
            
            if (!string.Equals(remoteLogin.ReturnCode, "OK"))
            {
                _logger.LogWarning("Error Response from login [{ErrorMessage}] - Name: {UserName}", remoteLogin.ErrorMessage, name);
                return null;
            }

            //            this.setUniqueid(str_token);
            //this.setRecordid(recordid);

            var sessionEntry = new SessionEntry()
            {
                UserName = name,
                Token = remoteLogin.Token,
                Active = true,
                DateHit = DateTime.Now,
                
            };
            var preHash = $"{name}{sessionEntry.DateHit.Ticks / TimeSpan.TicksPerSecond}";
            sessionEntry.SessionId = CalculateHash(SHA256.Create(), Encoding.Default.GetBytes(preHash));


            var value = await _mySql.InsertSessionEntry(sessionEntry);

            if (!value)
            {
                //    setLogError("No Session Record");
                return null;
            }

            return new LoginService()
            {
                Uniqueid = sessionEntry.SessionId
            };
        }

        public async Task<UserSessionInfo> GetTokenAndUserId(string token)
        {
            try
            {
                //    _logIt.LogInfo("GetToken from SessionID: " + cd.getToken());
                var sessionEntry = await _mySql.GetSessionEntryBySessionId(token);
                if (sessionEntry == null)
                {
                    //        _logIt.LogInfo("Login - No Token Found");
                    return null;
                }
                else if (sessionEntry.Active == false)
                {
                    //            _logIt.LogInfo("Login - Token is InActive");
                    return null;
                }
                else
                {
                    //            cd.setToken(st.getToken());
                    //            cd.setUser(st.getUserName());
                    return new UserSessionInfo()
                    {
                        Token = sessionEntry.Token,
                        UserName = sessionEntry.UserName
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not Get Token for login");
            }

            return null;
        }

        public string HashPassword(string userName, string password)
        {
            var result = "";
            var trimmedPassword = $"a{password}".Trim().Substring(1);

            try
            {
                //combines password and username padleft by 10
                var combinedValue = $"{trimmedPassword}{userName,10}";

                var inputLen = combinedValue.Length;

                var precursor = new byte[] {0, (byte) inputLen};

                var resultBytes = new byte[inputLen];

                //Encoding 1047 = cp1047 in the java code. This requires the CodePagesEncodingProvider be registed in Encoding.
                var encoding = Encoding.GetEncoding(1047);
                var trailor = encoding.GetBytes(combinedValue);

                // from JAVA: System.arraycopy(precursor, 0, result, 0, precursor.length);
                resultBytes[0] = precursor[0];
                resultBytes[1] = precursor[1];

                // from JAVA: System.arraycopy(trailor, 0, result, precursor.length, trailor.length-2);

                for (int i = 0; i < trailor.Length - 2; i++)
                {
                    resultBytes[i + 2] = trailor[i];
                }

                result = CalculateHash(SHA1.Create(), resultBytes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not hash password");
                result = "";
            }


            return result;
        }

        public string CalculateHash(HashAlgorithm algorithmm, byte[] inputBytes)
        {
            var hash = algorithmm.ComputeHash(inputBytes);

            return HexStringFromBytes(hash);
        }

        /// <summary>
        /// Convert an array of bytes to a string of hex digits
        /// </summary>
        /// <param name="bytes">array of bytes</param>
        /// <returns>String of hex digits</returns>
        public static string HexStringFromBytes(byte[] bytes)
        {
            var sb = new StringBuilder();
            foreach (var b in bytes)
            {
                var hex = b.ToString("x2");
                sb.Append(hex.ToUpper());
            }
            return sb.ToString();
        }
    }
}
