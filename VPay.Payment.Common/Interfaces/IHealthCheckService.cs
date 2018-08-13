using System.Collections.Generic;
using System.Threading.Tasks;
using VPay.Payment.Common.Models;

namespace VPay.Payment.Common
{
    public interface IHealthCheckService
    {
        /// <summary>
        /// Will go through all services and verify if they are healthy
        /// </summary>
        /// <returns>Returns a list of components and their status, 'FAILURE' if it is not healthy and 'OK' if it is healthy</returns>
        Task<IEnumerable<ServiceComponentStatus>> CheckHealth();
    }
}
