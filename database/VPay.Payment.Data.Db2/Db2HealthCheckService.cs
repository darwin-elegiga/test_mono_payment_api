using Microsoft.Extensions.Diagnostics.HealthChecks;
using VPay.Payment.Data.Db2.Connection;
// ReSharper disable ArrangeConstructorOrDestructorBody

// ReSharper disable once CheckNamespace
namespace VPay.Payment.Data.Health;

public class Db2HealthCheckService : IHealthCheck
{
    private readonly DB2UnifiedConnection _db2Connection;

    public Db2HealthCheckService(DB2UnifiedConnection connection)
    {
        _db2Connection = connection;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var result = await _db2Connection.CanConnectAsync(cancellationToken);

        if (result)
        {
            return HealthCheckResult.Healthy("Db2 Successful");
        }

        return HealthCheckResult.Unhealthy("Db2 Failed");
    }
}
