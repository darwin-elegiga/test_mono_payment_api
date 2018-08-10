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
            FromAddr1 = " ";
            FromAddr2 = " ";
            FromCity = " ";
            FromState = " ";
            FromZip = " ";
            FromFax = " ";
            ToName = " ";
            ToAddr1 = " ";
            ToAddr2 = " ";
            ToCity = " ";
            ToState = " ";
            ToZip = " ";
            ToFax = " ";
            ToPhone = " ";
            StatText = " ";
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
            RqstDate = rqstDateIn;
            StatDate = statDateIn;
            SentBehalfName = sentBehalfNameIn;
            FromName = fromNameIn;
            FromAddr1 = fromAddr1In;
            FromAddr2 = fromAddr2In;
            FromCity = fromCityIn;
            FromState = fromStateIn;
            FromZip = fromZipIn;
            FromFax = fromFaxIn;
            ToName = toNameIn;
            ToAddr1 = toAddr1In;
            ToAddr2 = toAddr2In;
            ToCity = toCityIn;
            ToState = toStateIn;
            ToZip = toZipIn;
            ToFax = toFaxIn;
            ToPhone = toPhoneIn;
            StatText = statTextIn;
            FaxJobList = faxJobListIn;
            DmRecId = dmRecIdIn;
            LastStatRank = 10;
            LastStatText = "";
        }

        public string Direction { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public int RqstDate { get; set; }
        public int StatDate { get; set; }
        public string SentBehalfName { get; set; }
        public string FromName { get; set; }
        public string FromAddr1 { get; set; }
        public string FromAddr2 { get; set; }
        public string FromCity { get; set; }
        public string FromState { get; set; }
        public string FromZip { get; set; }
        public string FromFax { get; set; }
        public string ToName { get; set; }
        public string ToAddr1 { get; set; }
        public string ToAddr2 { get; set; }
        public string ToCity { get; set; }
        public string ToState { get; set; }
        public string ToZip { get; set; }
        public string ToFax { get; set; }
        public string ToPhone { get; set; }
        public string StatText { get; set; }
        public List<FaxJob> FaxJobList { get; set; }
        public IEnumerator<FaxJob> FaxJobPtr { get; set; }
        //public SignatureBatch SignatureBatch { get; set; }

        public int DmRecId { get; set; }
        public int LastStatRank { get; set; }
        public string LastStatText { get; set; }

        public void ParseResult()
        {
            // TODO:  Convert SQL-related object to properties
        }
    }
}
