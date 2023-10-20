using System.Collections.Generic;
using FaxManagement.Client.v1.Models;

namespace VPay.Payment.Common
{
    public class CorespDtl
    {
        public CorespDtl()
        {
            Direction = " ";
            Type = " ";
            Status = " ";
            SentBehalfName = " ";
            FromName = " ";
            FromAddress1 = " ";
            FromAddress2 = " ";
            FromCity = " ";
            FromState = " ";
            FromPostalCode = " ";
            FromFax = " ";
            ToName = " ";
            ToAddress1 = " ";
            ToAddress2 = " ";
            ToCity = " ";
            ToState = " ";
            ToPostalCode = " ";
            ToFax = " ";
            ToPhone = " ";
            StatusText = " ";
            DmRecId = 0;
        }

        public string Direction { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int RequestDate { get; set; }
        public int StatusDate { get; set; }
        public string SentBehalfName { get; set; }
        public string FromName { get; set; }
        public string FromAddress1 { get; set; }
        public string FromAddress2 { get; set; }
        public string FromCity { get; set; }
        public string FromState { get; set; }
        public string FromPostalCode { get; set; }
        public string FromFax { get; set; }
        public string ToName { get; set; }
        public string ToAddress1 { get; set; }
        public string ToAddress2 { get; set; }
        public string ToCity { get; set; }
        public string ToState { get; set; }
        public string ToPostalCode { get; set; }
        public string ToFax { get; set; }
        public string ToPhone { get; set; }
        public string StatusText { get; set; }
        public List<FaxJobDto> FaxJobList { get; set; }
        public long DmRecId { get; set; }

    }
}
