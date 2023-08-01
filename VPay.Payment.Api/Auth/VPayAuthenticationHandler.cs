using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Auth
{
    public class VPayAuthenticationHandler : AuthenticationHandler<VPayAuthenticationOptions>
    {
        private const string AuthorizationHeaderName = "Authorization";
        private const string BasicSchemeName = "Basic";
        private readonly IAuthService _authenticationService;
        private readonly IHttpContextAccessor _accessor;


        public VPayAuthenticationHandler(
            IOptionsMonitor<VPayAuthenticationOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IHttpContextAccessor accessor,
            IAuthService authenticationService)
            : base(options, logger, encoder, clock)
        {
            _accessor = accessor;
            _authenticationService = authenticationService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(AuthorizationHeaderName))
            {
                //Authorization header not in request
                return AuthenticateResult.NoResult();
            }

            if (!AuthenticationHeaderValue.TryParse(Request.Headers[AuthorizationHeaderName], out AuthenticationHeaderValue headerValue))
            {
                //Invalid Authorization header
                return AuthenticateResult.NoResult();
            }

            if (!BasicSchemeName.Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
            {
                //Not Basic authentication header
                return AuthenticateResult.NoResult();
            }

            byte[] headerValueBytes = Convert.FromBase64String(headerValue.Parameter);
            string userAndPassword = Encoding.UTF8.GetString(headerValueBytes);
            string[] parts = userAndPassword.Split(':');
            if (parts.Length != 4)
            {
                return AuthenticateResult.Fail("Invalid VPay authentication header");
            }

            var ip = "0.0.0.0";

            var remoteIp = _accessor.HttpContext.Connection.RemoteIpAddress;
            var map = remoteIp.MapToIPv4();

            ip = map.ToString();

            var av = new AuthenticationParam()
            {
                Id = parts[0],
                PassPhrase = parts[1],
                IpAddress = ip,
                UserId = parts[2],
                Password = parts[3]
            };

            using (Logger.BeginScope(new Dictionary<string, object>
            {
                ["SourceIp"] = av.IpAddress,
                ["UserName"] = av.UserId,
                ["AuthId"] = av.Id
            }))
            {
                var user = await _authenticationService.Login(av);

                if (user == null)
                {
                    if (Request.Path.StartsWithSegments("/api/legacy", StringComparison.OrdinalIgnoreCase) &&
                        Request.Method == "POST")
                    {
                        try
                        {
                            Request.EnableBuffering();

                            using (var reader = new MemoryStream())
                            {
                                await Request.Body.CopyToAsync(reader);
                                var bodyAsText = Encoding.UTF8.GetString(reader.ToArray());

                                TryToLogFailureBody(av.UserId, bodyAsText);
                            }
                        }
                        finally
                        {
                            // Workaround so MVC action will be able to read body as well
                            Request.Body.Seek(0, SeekOrigin.Begin);
                        }
                    }

                    return AuthenticateResult.Fail("Invalid username or password");
                }

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Token),
                    new Claim(ClaimTypes.System, user.Source.ToString())
                };
                var identity = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
        }



        private void TryToLogFailureBody(string userName, string bodyAsText)
        {
            try
            {
                var bodyObj = JsonConvert.DeserializeObject<LegacyRequest>(bodyAsText);
                var body = bodyObj?.Envelope?.Body;
                if (body != null)
                {
                    StandardRequest result;

                    if (body.GetTransactionDetails?.Request != null)
                    {
                        result = new StandardRequest()
                        {
                            CommonData = new CommonData()
                            {
                                User = body.GetTransactionDetails.Request.User,
                                TransNumber = body.GetTransactionDetails.Request.TransNumber
                            }
                        };
                        Logger.LogWarning("Login Failed [{UserName}]: \n{UserRequestBody}", userName, result.ToDisplayString());
                    }
                    else if (body.GetReasonCodes?.Request != null)
                    {
                        result = new StandardRequest()
                        {
                            CommonData = new CommonData()
                            {
                                User = body.GetReasonCodes.Request.User,
                                TransNumber = body.GetReasonCodes.Request.TransNumber
                            }
                        };
                    }
                    else
                    {
                        result = body.LoadPan?.Request ??
                                 body.BalanceRequest?.Request ??
                                 body.GetPanNumber?.Request ??
                                 body.OpenPreAuth?.Request ??
                                 body.UnloadPan?.Request ??
                                 body.StopPay?.Request ??
                                 body.CancelFax?.Request ??
                                 body.ChangeFaxNumber?.Request ??
                                 body.HoldFax?.Request ??
                                 body.ReleaseFax?.Request ??
                                 body.ResendFax?.Request;
                    }

                    if (result != null)
                    {
                        Logger.LogWarning("Login Failed [{UserName}]: \n{UserRequestBody}", userName, result.ToDisplayString());
                    }

                }
            }
            catch
            {
                //do nothing because we just are trying to log if bad values
            }
        }
    }

}
