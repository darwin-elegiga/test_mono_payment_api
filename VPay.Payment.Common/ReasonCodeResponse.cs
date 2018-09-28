using System.Collections.Generic;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public class ReasonCodeResponse
    {
        public ReasonCodeResponse()
        {
            CommonData = new CommonData();
        }

        public ReasonCodeResponse(CommonData commonData)
        {
            CommonData = commonData;
            if (CommonData == null)
            {
                CommonData = new CommonData();
            }
        }

        public CommonData CommonData { get; set; }
        public List<ReasonCodeType> ReasonCodeList { get; set; }


        public override string ToString()
        {
            // TODO:  Implement real logic

            return "";
        }

        public bool IsValid()
        {
            return true;
        }

    }
}
