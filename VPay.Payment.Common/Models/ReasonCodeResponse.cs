using System.Collections.Generic;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public class ReasonCodeResponse
    {
        public ReasonCodeResponse() : this(new CommonData())
        {
        }

        public ReasonCodeResponse(CommonData commonData)
        {
            CommonData = commonData ?? new CommonData();
        }

        public CommonData CommonData { get; set; }
        public List<ReasonCodeType> ReasonCodeList { get; set; }

    }
}
