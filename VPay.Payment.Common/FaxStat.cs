namespace VPay.Payment.Common
{
    public class FaxStat
    {
        public FaxStat()
        {
            JobId = 0;
            Status = " ";
            StatCode = "000       ";
            StatText = " ";
            LastStatRank = 10;
            LastStatText = "";
        }

        public int JobId { get; set; }
        public string Status { get; set; }
        public string StatCode { get; set; }
        public string StatText { get; set; }

        public int LastStatRank { get; set; }
        public string LastStatText { get; set; }

    }
}
