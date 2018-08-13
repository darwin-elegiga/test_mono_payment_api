using System.Threading.Tasks;

namespace VPay.Payment.Common
{
    /// <summary>
    /// This interface is used by the <see cref="IHealthCheckService" /> to run health checks on each of the
    /// components that inherit it.
    /// </summary>
    public interface IHealthCheck
    {
        /// <summary>
        /// This is the name of the component that the <see cref="IHealthCheckService" /> returns
        /// </summary>
        string Component { get; }

        /// <summary>
        /// Returns if the current component is healthy or not.
        /// </summary>
        /// <returns>Should return <value>True</value> is returned if the component is healthy; otherwise return <value>false</value></returns>
        Task<bool> IsHealthy();
    }
}
