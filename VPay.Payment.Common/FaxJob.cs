using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    public class FaxJob
    {
        public FaxJob()
        {
            JobId = 0;
            Queue = " ";
            FaxStatus = " ";
            Priority = 0;
            RetryCount = 0;
            FaxNumber = " ";
            ReserveName = " ";
            //createTS = "";
            //lastStatTS = "";

            HoldAble = "FALSE";
            ReleaseAble = "FALSE";
            CancelAble = "FALSE";
            EditAble = "FALSE";
            DropToMailAble = "FALSE";
        }

        public FaxJob(
            int jobIdIn, string queueIn, string faxStatusIn,
            int priorityIn, DateTime createTSIn, DateTime lastStatTSIn,
            int retryCountIn, string faxNumberIn, string reserveNameIn)
        {
            JobId = jobIdIn;
            Queue = queueIn;
            FaxStatus = faxStatusIn;
            Priority = priorityIn;
            CreateTS = createTSIn;
            LastStatTS = lastStatTSIn;
            RetryCount = retryCountIn;
            FaxNumber = faxNumberIn;
            ReserveName = reserveNameIn;

            HoldAble = "FALSE";
            ReleaseAble = "FALSE";
            CancelAble = "FALSE";
            EditAble = "FALSE";
            DropToMailAble = "FALSE";
        }


        public int JobId { get; set; }
        public string Queue { get; set; }
        public string FaxStatus { get; set; }
        public int Priority { get; set; }
        public DateTime CreateTS { get; set; }
        public DateTime LastStatTS { get; set; }
        public int RetryCount { get; set; }
        public string FaxNumber { get; set; }
        public string ReserveName { get; set; }
        public List<FaxStat> FaxStatList { get; set; }
        public IEnumerator<FaxStat> FaxStatPtr { get; set; }

        public string HoldAble { get; set; }
        public string ReleaseAble { get; set; }
        public string CancelAble { get; set; }
        public string EditAble { get; set; }
        public string DropToMailAble { get; set; }

        public void ParseResult()
        {
            // TODO:  Convert SQL-related object to properties
        }

    }
}
