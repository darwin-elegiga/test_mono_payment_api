using System.Data.Odbc;
using IBM.Data.Db2;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Odbc;

namespace VPay.Payment.Data.Db2.Connection;

public static class ServiceProviderExtensions
{
    /// <summary>
    /// Registers a <see cref="DB2UnifiedConnection"/> with the service collection
    /// </summary>
    /// <param name="serviceCollection"></param>
    public static IServiceCollection AddDb2UnifiedConnection(this IServiceCollection serviceCollection)
    {
        serviceCollection.TryAddScoped<DB2UnifiedConnection>(x =>
        {
            var factory = x.GetService<ILoggerFactory>();
            var config = x.GetRequiredService<DB2UnifiedConnectionSettings>();

            var connectionString = GetConnectionString(config);
            serviceCollection.AddDb2OdbcConnection(connectionString);

            return new DB2UnifiedConnection(connectionString, config.IsLocal,
                factory.CreateLogger<DB2UnifiedConnection>());
        });
        return serviceCollection;
    }

    private static string GetConnectionString(DB2UnifiedConnectionSettings config)
    {
        if (config.IsLocal)
        {
            return new DB2ConnectionStringBuilder
            {
                Database = "testdb",
                UserID = config.UserName,
                Password = config.Password,
                Server = config.Hostname
            }.ToString();
        }

        return new OdbcConnectionStringBuilder
        {
            Dsn = config.Dsn,
            ["DefaultLibraries"] = config.DefaultLibraries,
            ["UID"] = config.UserName,
            ["PWD"] = config.Password,
            ["System"] = config.Hostname
        }.ToString();
    }
}
