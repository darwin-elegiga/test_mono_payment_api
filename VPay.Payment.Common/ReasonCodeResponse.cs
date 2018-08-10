using System;
using System.Collections.Generic;
using System.Text;

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
        public List<ReasonCode> RcList { get; set; }
        public IEnumerator<ReasonCode> RcPtr { get; set; }

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

    public class CommonData
    {
        // TODO:  Figure out real class to use instead
    }
}
