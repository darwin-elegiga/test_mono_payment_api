using System.Threading.Tasks;

namespace VPay.Payment.Common
{
    public interface IHealthCheck
    {
        string Component { get; }

        Task<bool> IsHealthy();
    }
}
