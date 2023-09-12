using System;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VPay.Payment.Api.Validation;
using VPay.Payment.Common;

namespace VPay.Payment.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        private ILogger _logger;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers(opt =>
                {
                    opt.EnableEndpointRouting = false;
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                })
                .AddJsonSettings();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new ValidationFailedResult(actionContext.ModelState);
                };
            });

            services
                .AddFluentValidationSettings()
                .AddApiVersioningService()
                .AddOptions()
                .AddSwaggerGenService()
                .SetupAuth()
                .SetupDb2(Configuration);

            services.Configure<PaymentConfig>(Configuration.GetSection("PaymentSettings"));
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<PaymentConfig>>().Value);

            services.AddTransient<IHealthCheckService, HealthCheckService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ITradingPostService, TradingPostService>();
            services.AddTransient<ILegacyTransactionService, LegacyTransactionService>();
            services.AddTransient<ILegacyValidationService, LegacyValidationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IHostApplicationLifetime appLifetime)
        {
            app.UseGlobalExceptionHandler(c =>
            {
                ConfigureExceptionHandler(c, env.IsDevelopment());
            });

            app.Map("/error", x => x.Run(y => throw new Exception()));

            app
                .UseStaticFiles()
                .UseMiddleware<RequestLoggingMiddleware>()
                .UseAuthentication()
                .UseMiddleware<AttachUserToLoggingMiddleware>()
                .UseMvc()
                .UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //The version after /swagger/ in the URL must match the "name" established when defining the API documentation in calls to SwaggerDoc
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Payment v1");
            });

            _logger = loggerFactory.CreateLogger<Startup>();


            appLifetime.ApplicationStarted.Register(OnStartup);
            appLifetime.ApplicationStopping.Register(OnShutdown);
            appLifetime.ApplicationStopped.Register(OnShutdownComplete);
        }


        private void OnStartup()
        {
            _logger.LogInformation("Startup occurred for VPay.Payment.Api");
        }

        private void OnShutdown()
        {
            _logger.LogInformation("Shutdown occurred for VPay.Payment.Api");
        }

        private void OnShutdownComplete()
        {
            _logger.LogInformation("Shutdown completed for VPay.Payment.Api");
        }


        private void ConfigureExceptionHandler(ExceptionHandlerConfiguration config, bool isDevelopment)
        {
            config.ContentType = "application/json";

            if (isDevelopment)
            {
                config.ResponseBody(s => JsonConvert.SerializeObject(new
                {
                    message = s.Message,
                    exceptionType = s.GetType().FullName,
                    stacktrace = s.StackTrace,
                    innerException = s.InnerException == null ? null : new
                    {
                        message = s.InnerException.Message,
                        exceptionType = s.InnerException.GetType().FullName,
                        stackTrace = s.InnerException.StackTrace
                    }
                }));
            }
            else
            {
                config.ResponseBody(s => JsonConvert.SerializeObject(new
                {
                    message = "An error occurred while processing your request"
                }));
            }
        }
    }
}
