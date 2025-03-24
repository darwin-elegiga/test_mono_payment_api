using System;
using Xunit;
using VPay.Payment.Common;

namespace VPay.Payment.Common.Tests.Helpers
{
    public class ServicePermissionHelpersTests
    {
        [Theory]
        [InlineData(ServicePermission.GetPan, "GETPAN", "EDIT")]
        [InlineData(ServicePermission.OpenPreAuth, "OPENPREAUTH", "READ")]
        [InlineData(ServicePermission.LoadPan, "LOADPAN", "EDIT")]
        [InlineData(ServicePermission.BalanceRequest, "BALREQUEST", "READ")]
        [InlineData(ServicePermission.Unload, "UNLOAD", "DELETE")]
        [InlineData(ServicePermission.StopPay, "STOPPAY", "DELETE")]
        [InlineData(ServicePermission.CancelFax, "CANCELFAX", "DELETE")]
        [InlineData(ServicePermission.ChangeFaxNumber, "CHANGEFAXNUMBER", "DELETE")]
        [InlineData(ServicePermission.HoldFax, "HOLDFAX", "DELETE")]
        [InlineData(ServicePermission.ReleaseFax, "RELEASEFAX", "DELETE")]
        [InlineData(ServicePermission.ResendFax, "RESENDFAX", "DELETE")]
        public void GetPermissionInfo_ShouldReturnCorrectInfo(ServicePermission permission, string expectedPermissionName, string expectedAction)
        {
            // Act
            var result = permission.GetPermissionInfo();

            // Assert
            Assert.Equal(expectedPermissionName, result.PermissionName);
            Assert.Equal(expectedAction, result.Action);
        }

        [Fact]
        public void GetPermissionInfo_InvalidPermission_ShouldThrowArgumentOutOfRangeException()
        {
            // Arrange
            var invalidPermission = (ServicePermission)999;

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => invalidPermission.GetPermissionInfo());
        }
    }
}
