using System;
using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using VPay.Payment.Api.Validation;
using VPay.Payment.Common;
using VPay.Payment.Common.Models;

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
                    opt.Filters.Add(typeof(ValidatorActionFilter));
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddJsonSettings()
                .AddFluentValidationSettings();

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new ValidationFailedResult(actionContext.ModelState);
                };
            });

            services
                .AddApiVersioningService()
                .AddOptions()
                .AddSwaggerGenService()
                .SetupAuth()
                .SetupDb2(Configuration);
            
            services.Configure<PaymentConfig>(Configuration.GetSection("PaymentSettings"));
            services.AddScoped(cfg => cfg.GetService<IOptionsSnapshot<PaymentConfig>>().Value);

            services.AddTransient<IHealthCheckService, HealthCheckService>();
            services.AddTransient<ITransactionService, TransactionService>();
            services.AddTransient<ILegacyTransactionService, LegacyTransactionService>();
            services.AddTransient<ILegacyValidationService, LegacyValidationService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            app.UseExceptionHandler("/error").WithConventions(x => {
                ConfigureExceptionHandler(x, env.IsDevelopment());
            });

            app.Map("/error", x => x.Run(y => throw new Exception()));

            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseMiddleware<AttachUserToLoggingMiddleware>();

            app.UseMvc()
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
