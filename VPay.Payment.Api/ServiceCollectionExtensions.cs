using System;
using System.IO;
using System.Linq;
using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Swagger;
using VPay.Data.Db2.Odbc;
using VPay.Payment.Api.Auth;
using VPay.Payment.Common;

namespace VPay.Payment.Api
{
    public static class ServiceCollectionExtensions
    {

        public static IMvcBuilder AddJsonSettings(this IMvcBuilder mvcBuilder)
        {
            mvcBuilder
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    options.SerializerSettings.DateFormatHandling = DateFormatHandling.IsoDateFormat;
                });

            return mvcBuilder;
        }

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

        public static IServiceCollection AddApiVersioningService(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = new HeaderApiVersionReader();
            });

            return services;
        }

        public static IServiceCollection AddSwaggerGenService(this IServiceCollection services)
        {
            // Register the Swagger generator, defining one or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                //Names used here are used in URL for SwaggerUI
                c.SwaggerDoc("v1.0", new Info { Title = "Payment API", Version = "v1.0" });

                c.AddSecurityDefinition("VPay", new BasicAuthScheme()
                {
                    Description = "Basic HTTP Auth"
                });

                c.OperationFilter<BasicAuthFilter>(); ;

                //Determine which set of documentation an API should belong to
                c.DocInclusionPredicate((docName, apiDesc) =>
                {
                    var actionApiVersionModel = apiDesc.ActionDescriptor?.GetApiVersion();

                    // if no version is specified or API is marked version neutral add to all swagger documents
                    if (actionApiVersionModel == null || actionApiVersionModel.IsApiVersionNeutral)
                    {
                        return true;
                    }
                    if (actionApiVersionModel.DeclaredApiVersions.Any())
                    {
                        return actionApiVersionModel.DeclaredApiVersions.Any(v => $"v{v.ToString()}" == docName);
                    }
                    return actionApiVersionModel.ImplementedApiVersions.Any(v => $"v{v.ToString()}" == docName);
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
