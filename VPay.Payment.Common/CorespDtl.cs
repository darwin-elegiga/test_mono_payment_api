using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common
{
    public class CorespDtl
    {
        public CorespDtl()
        {
            Direction = " ";
            Type = " ";
            Status = " ";
            //RqstDate = " ";
            //StatDate = " ";
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
            //FaxJobList = " ";
            DmRecId = 0;
            LastStatRank = 10;
            LastStatText = "";
        }

        public CorespDtl(
            string directionIn, string typeIn, string statusIn,
            int rqstDateIn, int statDateIn, string sentBehalfNameIn,
            string fromNameIn, string fromAddr1In, string fromAddr2In,
            string fromCityIn, string fromStateIn, string fromZipIn,
            string fromFaxIn, string toNameIn, string toAddr1In,
            string toAddr2In, string toCityIn, string toStateIn,
            string toZipIn, string toFaxIn, string toPhoneIn,
            string statTextIn, List<FaxJob> faxJobListIn, int dmRecIdIn)
        {
            Direction = directionIn;
            Type = typeIn;
            Status = statusIn;
            RequestDate = rqstDateIn;
            StatusDate = statDateIn;
            SentBehalfName = sentBehalfNameIn;
            FromName = fromNameIn;
            FromAddress1 = fromAddr1In;
            FromAddress2 = fromAddr2In;
            FromCity = fromCityIn;
            FromState = fromStateIn;
            FromPostalCode = fromZipIn;
            FromFax = fromFaxIn;
            ToName = toNameIn;
            ToAddress1 = toAddr1In;
            ToAddress2 = toAddr2In;
            ToCity = toCityIn;
            ToState = toStateIn;
            ToPostalCode = toZipIn;
            ToFax = toFaxIn;
            ToPhone = toPhoneIn;
            StatusText = statTextIn;
            FaxJobList = faxJobListIn;
            DmRecId = dmRecIdIn;
            LastStatRank = 10;
            LastStatText = "";
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
        public List<FaxJob> FaxJobList { get; set; }

        public int DmRecId { get; set; }
        public int LastStatRank { get; set; }
        public string LastStatText { get; set; }

    }
}
