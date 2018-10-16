using System.Collections.Generic;
using VPay.Data.Db2.Abstractions.CorrespondenceRepo;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public class TransactionDetailResponse
    {
        public CommonData CommonData { get; set; } = new CommonData();

        public HeaderData HeaderData { get; set; } = new HeaderData();

        public PayTypeDetail PayTypeDetail { get; set; } = new PayTypeDetail();

        public List<Detail> DetailList { get; set; } = new List<Detail>();

        public List<Correspondence> CorrespondenceList { get; set; } = new List<Correspondence>();


    }
}
