using System;
using System.Collections.Generic;
using System.Linq;
using VPay.Data.Db2.Abstractions.CorrespondenceRepo;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Data.Db2.Abstractions.Fax;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Common.Tests
{
    public class ObjectConverterHelpersTests
    {
        [Fact]
        public void ToCorespDtl_ShouldConvertCorrectly()
        {
            // Arrange
            var correspondence = new Correspondence
            {
                Id = 1,
                Type = "Type1",
                Direction = "In",
                SenderName = "Sender",
                SenderAddress1 = "Address1",
                SenderAddress2 = "Address2",
                SenderCity = "City",
                SenderState = "State",
                SenderZip = "Zip",
                SenderFax = "Fax",
                ReceiverName = "Receiver",
                ReceiverAddress1 = "RAddress1",
                ReceiverAddress2 = "RAddress2",
                ReceiverCity = "RCity",
                ReceiverState = "RState",
                ReceiverZip = "RZip",
                ReceiverPhone = "RPhone",
                ReceiverFax = "RFax",
                Status = "Status1",
                RequestDatePart = 20220101M,
                StatusDatePart = 20220102M
            };

            // Act
            var result = correspondence.ToCorespDtl();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(correspondence.Id, result.DmRecId);
            Assert.Equal(correspondence.Type, result.Type);
            Assert.Equal(correspondence.Direction, result.Direction);
            Assert.Equal(correspondence.SenderName, result.SentBehalfName);
            Assert.Equal(correspondence.SenderName, result.FromName);
            Assert.Equal(correspondence.SenderAddress1, result.FromAddress1);
            Assert.Equal(correspondence.SenderAddress2, result.FromAddress2);
            Assert.Equal(correspondence.SenderCity, result.FromCity);
            Assert.Equal(correspondence.SenderState, result.FromState);
            Assert.Equal(correspondence.SenderZip, result.FromPostalCode);
            Assert.Equal(correspondence.SenderFax, result.FromFax);
            Assert.Equal(correspondence.ReceiverName, result.ToName);
            Assert.Equal(correspondence.ReceiverAddress1, result.ToAddress1);
            Assert.Equal(correspondence.ReceiverAddress2, result.ToAddress2);
            Assert.Equal(correspondence.ReceiverCity, result.ToCity);
            Assert.Equal(correspondence.ReceiverState, result.ToState);
            Assert.Equal(correspondence.ReceiverZip, result.ToPostalCode);
            Assert.Equal(correspondence.ReceiverPhone, result.ToPhone);
            Assert.Equal(correspondence.ReceiverFax, result.ToFax);
            Assert.Equal(correspondence.Status, result.Status);
            Assert.Equal(Convert.ToInt32(correspondence.RequestDatePart), result.RequestDate);
            Assert.Equal(Convert.ToInt32(correspondence.StatusDatePart), result.StatusDate);
        }

        [Fact]
        public void ToCorespDtl_NullEntity_ShouldReturnNull()
        {
            // Arrange
            Correspondence correspondence = null;

            // Act
            var result = correspondence.ToCorespDtl();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToCorespDtl_Enumerable_ShouldConvertCorrectly()
        {
            // Arrange
            var correspondences = new List<Correspondence>
            {
                new Correspondence { Id = 1, Type = "Type1" },
                new Correspondence { Id = 2, Type = "Type2" }
            };

            // Act
            var result = correspondences.ToCorespDtl().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].DmRecId);
            Assert.Equal("Type1", result[0].Type);
            Assert.Equal(2, result[1].DmRecId);
            Assert.Equal("Type2", result[1].Type);
        }

        [Fact]
        public void ToFaxJob_ShouldConvertCorrectly()
        {
            // Arrange
            var faxJob = new VPay.Data.Db2.Abstractions.Fax.FaxJob
            {
                Id = 1,
                Queue = "Queue1",
                FaxStatus = "Status1",
                Priority = 1,
                CreatedTimeStamp = DateTime.Now,
                LastStatusTimeStamp = DateTime.Now.AddMinutes(5),
                RetryCount = 2,
                FaxNumber = "1234567890",
                ReserveName = "Reserve1"
            };

            // Act
            var result = faxJob.ToFaxJob();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(faxJob.Id, result.FaxJobId);
            Assert.Equal(faxJob.Queue, result.FaxQueue);
            Assert.Equal(faxJob.FaxStatus, result.FaxStatus);
            Assert.Equal(faxJob.Priority, result.Priority);
            Assert.Equal(faxJob.CreatedTimeStamp, result.CreateTS);
            Assert.Equal(faxJob.LastStatusTimeStamp, result.LastStatusTS);
            Assert.Equal(faxJob.RetryCount, result.RetryCount);
            Assert.Equal(faxJob.FaxNumber, result.FaxNumber);
            Assert.Equal(faxJob.ReserveName, result.ReserveName);
            Assert.Equal("FALSE", result.ReleaseAble);
            Assert.Equal("FALSE", result.HoldAble);
            Assert.Equal("FALSE", result.CancelAble);
            Assert.Equal("FALSE", result.DropToMailAble);
            Assert.Equal("FALSE", result.EditAble);
        }

        [Fact]
        public void ToFaxJob_NullEntity_ShouldReturnNull()
        {
            // Arrange
            VPay.Data.Db2.Abstractions.Fax.FaxJob faxJob = null;

            // Act
            var result = faxJob.ToFaxJob();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToFaxJobs_Enumerable_ShouldConvertCorrectly()
        {
            // Arrange
            var faxJobs = new List<VPay.Data.Db2.Abstractions.Fax.FaxJob>
            {
                new VPay.Data.Db2.Abstractions.Fax.FaxJob { Id = 1, Queue = "Queue1" },
                new VPay.Data.Db2.Abstractions.Fax.FaxJob { Id = 2, Queue = "Queue2" }
            };

            // Act
            var result = faxJobs.ToFaxJobs().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].FaxJobId);
            Assert.Equal("Queue1", result[0].FaxQueue);
            Assert.Equal(2, result[1].FaxJobId);
            Assert.Equal("Queue2", result[1].FaxQueue);
        }

        [Fact]
        public void ToHeaderData_ShouldConvertCorrectly()
        {
            // Arrange
            var transactionHeader = new TransactionHeader
            {
                TransactionId = 1,
                ClientCode = "Client1",
                BillingEntity = "Bill1",
                PaymentType = "Type1",
                CurrentBalance = 100.50m,
                PayeeCode = "Payee1",
                PayeeName = "PayeeName1",
                ProviderName = "Provider1",
                RequesterId = "Requester1",
                RequesterName = "RequesterName1",
                TaxId = "Tax1",
                AvailableBalance = 50.25m,
                UserField1 = "User1",
                UserField2 = "User2",
                UserField3 = "User3"
            };

            // Act
            var result = transactionHeader.ToHeaderData();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionHeader.TransactionId, result.TransNumber);
            Assert.Equal(transactionHeader.ClientCode, result.Client);
            Assert.Equal(transactionHeader.BillingEntity, result.BillCode);
            Assert.Equal(transactionHeader.PaymentType, result.BillType);
            Assert.Equal(transactionHeader.CurrentBalance.ToString("F2"), result.CurrentBalance);
            Assert.Equal(transactionHeader.PayeeCode, result.PayeeCode);
            Assert.Equal(transactionHeader.PayeeName, result.PayeeName);
            Assert.Equal(transactionHeader.ProviderName, result.ProviderName);
            Assert.Equal(transactionHeader.RequesterId, result.RequesterId);
            Assert.Equal(transactionHeader.RequesterName, result.RequesterName);
            Assert.Equal(transactionHeader.TaxId, result.TaxId);
            Assert.Equal(transactionHeader.AvailableBalance.ToString("F2"), result.AvailBalance);
            Assert.Equal(transactionHeader.UserField1, result.UserField1);
            Assert.Equal(transactionHeader.UserField2, result.UserField2);
            Assert.Equal(transactionHeader.UserField3, result.UserField3);
        }

        [Fact]
        public void ToHeaderData_NullEntity_ShouldReturnNull()
        {
            // Arrange
            TransactionHeader transactionHeader = null;

            // Act
            var result = transactionHeader.ToHeaderData();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToDetail_ShouldConvertCorrectly()
        {
            // Arrange
            var transactionDetail = new TransactionDetail
            {
                TransactionId = 1,
                Status = "Status1",
                Amount = 100.50m,
                AuthCode = "Auth1",
                TranTimeStamp = DateTime.Now,
                Expiration = 20250102m,
                MerchantCode = "Merchant1",
                MerchantName = "MerchantName1",
                ReasonCode = "Reason1",
                ReasonDescription = "ReasonDesc1",
                ActionCode = "Action1",
                ActionDescription = "ActionDesc1",
                FinancialType = "Type1",
                RequesterName = "Requester1",
                BatchNumber = 12345,
                StatusDescription = "StatusDesc1"
            };

            // Act
            var result = transactionDetail.ToDetail();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(transactionDetail.TransactionId, result.TranId);
            Assert.Equal(transactionDetail.TransactionId, result.LoadTran);
            Assert.Equal(transactionDetail.Status, result.Status);
            Assert.Equal(transactionDetail.Amount.ToString("F2"), result.Amount);
            Assert.Equal(transactionDetail.AuthCode, result.AuthCode);
            Assert.Equal(transactionDetail.TranTimeStamp.ToString("yyyy-MM-dd HH:mm:ss.ffffff"), result.TranTimeStamp);
            Assert.Equal(transactionDetail.Expiration.ToString("F0"), result.Expiration);
            Assert.Equal(transactionDetail.MerchantCode, result.MerchantCode);
            Assert.Equal(transactionDetail.MerchantName, result.MerchantName);
            Assert.Equal(transactionDetail.ReasonCode, result.ReasonCode);
            Assert.Equal(transactionDetail.ReasonDescription, result.ReasonDesc);
            Assert.Equal(transactionDetail.ActionCode, result.ActionCode);
            Assert.Equal(transactionDetail.ActionDescription, result.ActionDesc);
            Assert.Equal(transactionDetail.FinancialType, result.FinancialType);
            Assert.Equal(transactionDetail.RequesterName, result.RequesterName);
            Assert.Equal(transactionDetail.BatchNumber.ToString("F0"), result.BatchNumber);
            Assert.Equal(transactionDetail.StatusDescription, result.StatusDesc);
        }

        [Fact]
        public void ToDetail_NullEntity_ShouldReturnNull()
        {
            // Arrange
            TransactionDetail transactionDetail = null;

            // Act
            var result = transactionDetail.ToDetail();

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void ToDetail_Enumerable_ShouldConvertCorrectly()
        {
            // Arrange
            var transactionDetails = new List<TransactionDetail>
            {
                new TransactionDetail { TransactionId = 1, Status = "Status1" },
                new TransactionDetail { TransactionId = 2, Status = "Status2" }
            };

            // Act
            var result = transactionDetails.ToDetail().ToList();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(1, result[0].TranId);
            Assert.Equal("Status1", result[0].Status);
            Assert.Equal(2, result[1].TranId);
            Assert.Equal("Status2", result[1].Status);
        }
    }
}
