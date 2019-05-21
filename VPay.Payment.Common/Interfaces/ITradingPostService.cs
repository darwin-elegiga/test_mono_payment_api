using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions.TradingPostWs;

namespace VPay.Payment.Common
{
    public interface ITradingPostService
    {
        Task<TradingPostData.LoadResult> LoadCard(TradingPostData.AuthenticationValuesAndIp auth,
            TradingPostData.LoadRequest request);

        Task<TradingPostData.RetrieveResult> RetrieveCard(TradingPostData.AuthenticationValuesAndIp auth,
            TradingPostData.RetrieveRequest request);

        Task<TradingPostData.NotificationResult> CardNotificationRelease(TradingPostData.AuthenticationValuesAndIp auth,
            TradingPostData.ReleaseNotification request);
    }
}
