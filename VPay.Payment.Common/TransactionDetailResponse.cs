using System;
using System.Collections.Generic;
using System.Text;
using VPay.Payment.Common.DataWebService;

namespace VPay.Payment.Common
{
    public class TransactionDetailResponse
    {
        public CommonData CommonData { get; set; }
        public HeaderData HeaderData { get; set; }
        public PayTypeDetail PayTypeDetail { get; set; }
        public List<Detail> DetailList { get; set; }
        public IEnumerator<Detail> DetailPtr { get; set; }
        public List<CorespDtl> CorespList { get; set; }
        public IEnumerator<CorespDtl> CorespPtr { get; set; }
        public CorespDtl Cr { get; set; }
    }
}
