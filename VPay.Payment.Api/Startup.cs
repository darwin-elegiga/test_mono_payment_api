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
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using VPay.Payment.Api.Validation;
using VPay.Payment.Common;
using Swashbuckle.AspNetCore.Swagger;
using System.Reflection;
using FluentValidation.AspNetCore;
using System.IO;
using VPay.Payment.Api.Auth;

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
                .AddMvc(opt =>
                {
                    opt.EnableEndpointRouting = false;
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson()
                .AddFluentValidationSettings();

            services.AddControllers().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                options.SerializerSettings.Converters.Add(new DecimalJsonConverter());
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;

            });
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new ValidationFailedResult(actionContext.ModelState);
                };
            });

            services
                .AddSwaggerGenService()
                .AddSwaggerGenNewtonsoftSupport()
                .AddOptions()
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
            app.UseExceptionHandler("/error").WithConventions(x =>
            {
                ConfigureExceptionHandler(x, env.IsDevelopment());
            });

            app.Map("/error", x => x.Run(y => throw new Exception()));

            app.UseStaticFiles()
                .UseMiddleware<RequestLoggingMiddleware>()
                .UseAuthentication()
                .UseMiddleware<AttachUserToLoggingMiddleware>()
                .UseMvc()
                .UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                //The version after /swagger/ in the URL must match the "name" established when defining the API documentation in calls to SwaggerDoc
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Payment-Api");
            });

            _logger = loggerFactory.CreateLogger<Startup>();
            app.UseRouting();
            //app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

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
                config.MessageFormatter(s => JsonConvert.SerializeObject(new
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
                config.MessageFormatter(s => JsonConvert.SerializeObject(new
                {
                    message = "An error occurred while processing your request"
                }));
            }
        }
    }
}
