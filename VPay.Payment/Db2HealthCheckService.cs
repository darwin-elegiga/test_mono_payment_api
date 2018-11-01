using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class Db2HealthCheckService : IHealthCheck
    {
        private readonly IDb2Context _db2Context;

        public Db2HealthCheckService(IDb2Context context)
        {
            _db2Context = context;

        }

        public string Component { get; } = "Db2";

        public async Task<bool> IsHealthy()
        {
            return await _db2Context.CanConnectAsync();
        }

    }
}
