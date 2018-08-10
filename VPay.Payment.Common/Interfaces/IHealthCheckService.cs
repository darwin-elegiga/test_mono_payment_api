using System.Collections.Generic;
using System.Threading.Tasks;
using VPay.Payment.Common.Models;

namespace VPay.Payment.Common
{
    public interface IHealthCheckService
    {
        Task<IEnumerable<ServiceComponentStatus>> CheckHealth();
    }
}
