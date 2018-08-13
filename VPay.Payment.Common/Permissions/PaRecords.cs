using System;
using System.Collections.Generic;
using System.Text;

namespace VPay.Payment.Common.Permissions
{
    public class PaRecords // done
    {
        public PaRecords()
        {

        }

        public PaRecords(int id, DateTime recordDate)
        {
            Id = id;
            RecordDate = recordDate;
        }

        public PaRecords(int id, string userName, string objectCode, int access, int permission, DateTime recordDate)
        {
            Id = id;
            UserName = userName;
            ObjectCode = objectCode;
            Access = access;
            Permission = permission;
            RecordDate = recordDate;
        }


        public int Id { get; set; }
        public string UserName { get; set; }
        public string ObjectCode { get; set; }
        public int Access { get; set; }
        public int Permission { get; set; }
        public DateTime RecordDate { get; set; }

    }
}
