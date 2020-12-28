using System;
using System.IO;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using VPay.Data.Db2.Odbc;
using VPay.Payment.Api.Auth;
using VPay.Payment.Common;

namespace VPay.Payment.Api
{
    public static class ServiceCollectionExtensions
    {
        public static IMvcBuilder AddFluentValidationSettings(this IMvcBuilder mvcBuilder)
        {
            return mvcBuilder.AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<Startup>());
        }

        public static IServiceCollection SetupDb2(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<OdbcConnectionConfig>(configuration.GetSection("Db2"));
            services.AddDb2OdbcConnection();

            services.AddScoped<IHealthCheck, Db2HealthCheckService>();

            return services;
        }

        public static IServiceCollection AddSwaggerGenService(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //Names used here are used in URL for SwaggerUI
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Payment API", Version = "v1.0" });

                c.AddSecurityDefinition("basic", new OpenApiSecurityScheme()
                {
                    Description = "Basic HTTP Auth",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });

                c.AddFluentValidationRules();
                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetEntryAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });

            return services;
        }

        public static IServiceCollection SetupAuth(this IServiceCollection services)
        {
            services
                .AddAuthorization()
                .AddAuthentication(VPayAuthenticationDefaults.AuthenticationScheme)
                .AddVPay<AuthService>();

            services.AddSingleton<IAuthorizationPolicyProvider, ServicePermissionPolicyProvider>();
            services.AddScoped<IAuthorizationHandler, ServiceMethodHandler>();

            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IUserInfo, UserInfo>();

            return services;
        }
    }
}
