using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Api.Dtos
{
    public class LoadPanStandardRequest : StandardRequest
    {

        public string ClientData { get; set; } = "";

    }
}
