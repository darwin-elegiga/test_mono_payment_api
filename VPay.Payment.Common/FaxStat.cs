using System;
using System.Collections.Generic;
using System.Text;

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

        public FaxStat(int jobIdIn, string statusIn, string statCodeIn, string statTextIn)
        {
            JobId = jobIdIn;
            Status = statusIn;
            StatCode = statCodeIn;
            StatText = statTextIn;
            LastStatRank = 10;
            LastStatText = "";
        }


        public int JobId { get; set; }
        public string Status { get; set; }
        public string StatCode { get; set; }
        public string StatText { get; set; } // M001

        public int LastStatRank { get; set; }
        public string LastStatText { get; set; }

        public void ParseResult()
        {
            // TODO:  Convert SQL-related object to properties
        }
    }
}
