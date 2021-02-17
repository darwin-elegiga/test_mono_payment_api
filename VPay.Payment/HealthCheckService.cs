using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class HealthCheckService : IHealthCheckService
    {
        private readonly IEnumerable<IHealthCheck> _healthChecks;

        public HealthCheckService(IEnumerable<IHealthCheck> healthChecks)
        {
            _healthChecks = healthChecks;
        }

        public async Task<IEnumerable<ServiceComponentStatus>> CheckHealth()
        {
            var healthCheckTasks = _healthChecks.Select(GetHealth).ToList();

            // Will run the IsHealthy on each of the health check components in parallel.
            var results = await Task.WhenAll(healthCheckTasks);

            return results;
        }

        private async Task<ServiceComponentStatus> GetHealth(IHealthCheck healthCheck)
        {
            var health = await healthCheck.IsHealthy();

            return new ServiceComponentStatus
            {
                Component = healthCheck.Component,
                Status = health ? "OK" : "FAILURE"
            };
        }
    }
}
