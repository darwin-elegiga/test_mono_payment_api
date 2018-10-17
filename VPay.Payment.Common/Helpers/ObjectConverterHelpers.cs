using System;
using System.Collections.Generic;
using System.Linq;
using VPay.Data.Db2.Abstractions.CorrespondenceRepo;

namespace VPay.Payment.Common
{
    public static class ObjectConverterHelpers
    {

        public static CorespDtl ToCorespDtl(this Correspondence entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new CorespDtl()
            {
                DmRecId = entity.Id,
                Type = entity.Type,
                Direction = entity.Direction,
                SentBehalfName = entity.SenderName,
                FromName = entity.SenderName,
                FromAddress1 = entity.SenderAddress1,
                FromAddress2 = entity.SenderAddress2,
                FromCity = entity.SenderCity,
                FromState = entity.SenderState,
                FromPostalCode = entity.SenderZip,
                FromFax = entity.SenderFax,
                ToName = entity.ReceiverName,
                ToAddress1 = entity.ReceiverAddress1,
                ToAddress2 = entity.ReceiverAddress2,
                ToCity = entity.ReceiverCity,
                ToState = entity.ReceiverState,
                ToPostalCode = entity.ReceiverZip,
                ToPhone = entity.ReceiverPhone,
                ToFax = entity.ReceiverFax,
                Status = entity.Status,
                RequestDate = Convert.ToInt32(entity.RequestDatePart),
                StatusDate = Convert.ToInt32(entity.StatusDatePart),
                FaxJobList = entity.FaxJobList.ToFaxJobs()?.ToList(),
            };
        }

        public static IEnumerable<CorespDtl> ToCorespDtl(this IEnumerable<Correspondence> entities)
        {
            return entities?.Select(x => x.ToCorespDtl());
        }

        public static FaxJob ToFaxJob(this VPay.Data.Db2.Abstractions.Fax.FaxJob entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new FaxJob
            {
                FaxJobId = entity.Id,
                FaxQueue = entity.Queue,
                FaxStatus = entity.FaxStatus,
                Priority = entity.Priority,
                CreateTS = entity.CreatedTimeStamp,
                LastStatusTS = entity.LastStatusTimeStamp,
                RetryCount = entity.RetryCount,
                FaxNumber = entity.FaxNumber,
                ReserveName = entity.ReserveName,
                ReleaseAble = entity.CanRelease ? "TRUE" : "FALSE",
                HoldAble = entity.CanHold ? "TRUE": "FALSE",
                CancelAble = entity.CanCancel ? "TRUE": "FALSE",
                DropToMailAble = entity.CanDropToMail ? "TRUE": "FALSE",
                EditAble = entity.CanEdit ? "TRUE": "FALSE"
            };
        }

        public static IEnumerable<FaxJob> ToFaxJobs(this IEnumerable<VPay.Data.Db2.Abstractions.Fax.FaxJob> entities)
        {
            return entities?.Select(x => x.ToFaxJob());
        }

    }
}
