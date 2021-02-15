using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using VPay.Payment.Common;

namespace VPay.Payment.Api
{
    public class AttachUserToLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public AttachUserToLoggingMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;

            _logger = loggerFactory
                .CreateLogger<AttachUserToLoggingMiddleware>();
        }

        // IMyScopedService is injected into Invoke
        public async Task Invoke(HttpContext httpContext, IUserInfo userInfo)
        {
            var userName = userInfo.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                await _next(httpContext);
            }
            else
            {
                using (_logger.BeginScope(new Dictionary<string, object> { ["UserName"] = userName }))
                {
                    await _next(httpContext);
                }
            }
        }
    }
}
