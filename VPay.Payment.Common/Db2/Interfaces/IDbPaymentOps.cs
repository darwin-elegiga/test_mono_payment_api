using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Common.Db2
{
    public interface IDbPaymentOps : IDisposable
    {
        Task<List<ReasonCodeType>> ReasonCodesData(string token, string user, string txid);
        Task<List<Detail>> TransactionDetailsData(string token, string client, string billc, string txid);
        Task<List<HeaderData>> TransactionHeadersData(string token, string user, string password, string txid, AuthenticationValues authValues, string ipAddress);
        Task<List<CorespDtl>> TransactionCorrespondenceData(string token, string user, string txid);
    }
}
