using System;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Common.Tests
{
    public class FaxJobTests
    {
        [Fact]
        public void FaxJob_DefaultConstructor_ShouldInitializeProperties()
        {
            // Act
            var faxJob = new FaxJob();

            // Assert
            Assert.Equal(0, faxJob.FaxJobId);
            Assert.Equal(" ", faxJob.FaxQueue);
            Assert.Equal(" ", faxJob.FaxStatus);
            Assert.Equal(0, faxJob.Priority);
            Assert.Equal(0, faxJob.RetryCount);
            Assert.Equal(" ", faxJob.FaxNumber);
            Assert.Equal(" ", faxJob.ReserveName);
            Assert.Equal("FALSE", faxJob.HoldAble);
            Assert.Equal("FALSE", faxJob.ReleaseAble);
            Assert.Equal("FALSE", faxJob.CancelAble);
            Assert.Equal("FALSE", faxJob.EditAble);
            Assert.Equal("FALSE", faxJob.DropToMailAble);
        }

        [Fact]
        public void FaxJob_SetAndGetProperties_ShouldWorkCorrectly()
        {
            // Arrange
            var faxJob = new FaxJob();
            var createTS = DateTime.Now;
            var lastStatusTS = DateTime.Now.AddMinutes(5);

            // Act
            faxJob.FaxJobId = 1;
            faxJob.FaxQueue = "Queue1";
            faxJob.FaxStatus = "Status1";
            faxJob.Priority = 1;
            faxJob.CreateTS = createTS;
            faxJob.LastStatusTS = lastStatusTS;
            faxJob.RetryCount = 2;
            faxJob.FaxNumber = "1234567890";
            faxJob.ReserveName = "Reserve1";
            faxJob.HoldAble = "TRUE";
            faxJob.ReleaseAble = "TRUE";
            faxJob.CancelAble = "TRUE";
            faxJob.EditAble = "TRUE";
            faxJob.DropToMailAble = "TRUE";

            // Assert
            Assert.Equal(1, faxJob.FaxJobId);
            Assert.Equal("Queue1", faxJob.FaxQueue);
            Assert.Equal("Status1", faxJob.FaxStatus);
            Assert.Equal(1, faxJob.Priority);
            Assert.Equal(createTS, faxJob.CreateTS);
            Assert.Equal(lastStatusTS, faxJob.LastStatusTS);
            Assert.Equal(2, faxJob.RetryCount);
            Assert.Equal("1234567890", faxJob.FaxNumber);
            Assert.Equal("Reserve1", faxJob.ReserveName);
            Assert.Equal("TRUE", faxJob.HoldAble);
            Assert.Equal("TRUE", faxJob.ReleaseAble);
            Assert.Equal("TRUE", faxJob.CancelAble);
            Assert.Equal("TRUE", faxJob.EditAble);
            Assert.Equal("TRUE", faxJob.DropToMailAble);
        }
    }
}
