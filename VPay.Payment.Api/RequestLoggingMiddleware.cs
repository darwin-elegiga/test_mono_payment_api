using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;

namespace VPay.Payment.Api
{
    /// <summary>
    /// This will used to log the timings in a semantic logging format so we can track in graylog
    /// <remarks>
    /// Based off of https://blog.getseq.net/smart-logging-middleware-for-asp-net-core/
    /// </remarks>
    /// </summary>
    public class RequestLoggingMiddleware
    {
        private const string MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";

        private readonly ILogger _logger;

        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));

            _logger = loggerFactory
                .CreateLogger<RequestLoggingMiddleware>();
        }

        /// <summary>
        /// This method is called by asp.net core handler for middleware.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            var start = Stopwatch.GetTimestamp();
            string userName = null;
            try
            {
                await _next(httpContext);
                userName = httpContext.User?.FindFirstValue(ClaimTypes.Name);

                var elapsedMs = GetElapsedMilliseconds(start, Stopwatch.GetTimestamp());

                var statusCode = httpContext.Response?.StatusCode;
                var level = LogLevel.Debug;

                var path = GetPath(httpContext);

                if (statusCode < 500)
                {
                    if (path.StartsWith("/api", StringComparison.OrdinalIgnoreCase) && !path.StartsWith("/api/health", StringComparison.OrdinalIgnoreCase) && !path.StartsWith("/api/about", StringComparison.OrdinalIgnoreCase))
                    {
                        level = LogLevel.Information;
                    }
                }
                else
                {
                    level = LogLevel.Error;
                }

                using (_logger.BeginScope(new Dictionary<string, object> { ["UserName"] = userName }))
                {
                    _logger.Log(level, MessageTemplate, httpContext.Request.Method.Replace(Environment.NewLine, string.Empty), path, statusCode, elapsedMs);
                }
            }
            // Never caught, because `LogException()` returns false, so exceptions will continue through
            catch (Exception ex) when (LogException(httpContext, GetElapsedMilliseconds(start, Stopwatch.GetTimestamp()), userName, ex)) { }
        }

        /// <summary>
        /// Must return <value>false</value>. This will log the exception and the request information and return false so
        /// the exception will proceed down the chain
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="elapsedMs"></param>
        /// <param name="userName"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        private bool LogException(HttpContext httpContext, double elapsedMs, string userName, Exception ex)
        {
            using (_logger.BeginScope(new Dictionary<string, object> { ["UserName"] = userName }))
            {
                _logger.LogError(ex, MessageTemplate, httpContext.Request.Method.Replace(Environment.NewLine, string.Empty), GetPath(httpContext), 500, elapsedMs);
            }

            return false;
        }

        /// <summary>
        /// Will convert the find the difference between start and stop time in milliseconds
        /// </summary>
        /// <remarks>
        /// Start and Stop are in number of ticks. To convert to milliseconds, you must divide by <see cref="Stopwatch.Frequency"/>
        /// </remarks>
        /// <param name="start">Start time in Ticks</param>
        /// <param name="stop">Stop time in Ticks</param>
        /// <returns></returns>
        private double GetElapsedMilliseconds(long start, long stop)
        {
            return (stop - start) * 1000 / (double)Stopwatch.Frequency;
        }

        private static string GetPath(HttpContext httpContext)
        {
            return httpContext.Features.Get<IHttpRequestFeature>()?.RawTarget ?? httpContext.Request.Path.ToString();
        }
    }
}
