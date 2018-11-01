namespace VPay.Payment.Common
{
    /// <summary>
    /// Class to represent the status of various components used by a service
    /// </summary>
    public class ServiceComponentStatus
    {
        /// <summary>
        /// The component that is being tested
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// The status of the service
        /// </summary>
        public string Status { get; set; }
    }
}
