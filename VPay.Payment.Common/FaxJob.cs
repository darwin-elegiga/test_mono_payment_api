using System;
using System.Collections.Generic;

namespace VPay.Payment.Common
{
    public class FaxJob
    {
        public FaxJob()
        {
            FaxJobId = 0;
            FaxQueue = " ";
            FaxStatus = " ";
            Priority = 0;
            RetryCount = 0;
            FaxNumber = " ";
            ReserveName = " ";

            HoldAble = "FALSE";
            ReleaseAble = "FALSE";
            CancelAble = "FALSE";
            EditAble = "FALSE";
            DropToMailAble = "FALSE";
        }

        public int FaxJobId { get; set; }
        public string FaxQueue { get; set; }
        public string FaxStatus { get; set; }
        public int Priority { get; set; }
        public DateTime CreateTS { get; set; }
        public DateTime LastStatusTS { get; set; }
        public int RetryCount { get; set; }
        public string FaxNumber { get; set; }
        public string ReserveName { get; set; }

        public string HoldAble { get; set; }
        public string ReleaseAble { get; set; }
        public string CancelAble { get; set; }
        public string EditAble { get; set; }
        public string DropToMailAble { get; set; }
    }
}
