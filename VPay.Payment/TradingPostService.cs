using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class TradingPostService : ITradingPostService
    {
        private readonly IDb2Context _db2Context;
        private readonly IUserInfo _user;
        private readonly ILogger _logger;
        private readonly ITradingPostWs _tradingPostWs;

        public TradingPostService(IDb2Context db2Context, IUserInfo user, ILogger<TradingPostService> logger, ITradingPostWs tradingPostWs)
        {
            _db2Context = db2Context;
            _user = user;
            _logger = logger;
            _tradingPostWs = tradingPostWs;
        }

        public async Task<TradingPostData.LoadResult> LoadCard(TradingPostData.AuthenticationValuesAndIp auth, TradingPostData.LoadRequest request)
        {
            var response = await _tradingPostWs.LoadCard(auth, request);

            return response;
        }

        public async Task<TradingPostData.RetrieveResult> RetrieveCard(TradingPostData.AuthenticationValuesAndIp auth, TradingPostData.RetrieveRequest request)
        {
            var response = await _tradingPostWs.RetrieveCard(auth, request);

            return response;
        }

        public async Task<TradingPostData.NotificationResult> CardNotificationRelease(TradingPostData.AuthenticationValuesAndIp auth, TradingPostData.ReleaseNotification request)
        {
            var response = await _tradingPostWs.CardNotificationRelease(auth, request);

            return response;
        }
    }
}
