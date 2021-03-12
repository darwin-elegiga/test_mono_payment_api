using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VPay.Extensions.Logging.GrayLog;

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
                .ConfigureLogging(loggingBuilder =>
                    loggingBuilder
                        .ClearProviders()
                        .AddConsole()
                        .AddDebug()
                        .AddVPayGrayLog()
                )
                .UseStartup<Startup>();
    }
}
