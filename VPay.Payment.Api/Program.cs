using System;
using Gelf.Extensions.Logging;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace VPay.Payment.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((builderContext, configurationBuilder) =>
                {
                    IWebHostEnvironment hostingEnvironment = builderContext.HostingEnvironment;

                    configurationBuilder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                        .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.Local.json", optional: true, reloadOnChange: true)
                        .AddJsonFile($"appsettings.secure.json", optional: true, reloadOnChange: true);
                })
                .ConfigureLogging((builderContext, loggingBuilder) =>
                {
                    IConfigurationSection graylogSection = builderContext.Configuration.GetSection("Graylog");
                    loggingBuilder.Services.Configure<GelfLoggerOptions>(graylogSection);

                    loggingBuilder.Services.PostConfigure<GelfLoggerOptions>(gelfLoggerOptions =>
                        gelfLoggerOptions.AdditionalFields["machine_name"] = Environment.MachineName);

                    IConfigurationSection loggingSection = builderContext.Configuration.GetSection("Logging");
                    loggingBuilder.AddConfiguration(loggingSection)
                        .AddConsole()
                        .AddDebug()
                        .AddGelf();
                })
                .UseStartup<Startup>();
    }
}
