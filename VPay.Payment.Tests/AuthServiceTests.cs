using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Payment.Common.Db2;
using VPay.Payment.Common.Models;
using VPay.Payment.Common.MySql;
using Xunit;

namespace VPay.Payment.Tests
{
    public class AuthServiceTests
    {

        [Fact]
        public void HashPassword()
        {
            string password = "yraheem197";
            string username = "YAMMONRAHE";

            string expectedResult = "B666BB00CEB92C007481180E4134DD46EFF7F5D8";

            var dbOps = new Mock<IDbPaymentOps>();
            var mySqlOps = new Mock<IMySqlPaymentOps>();

            var authService = new AuthService(dbOps.Object, mySqlOps.Object, new NullLogger<AuthService>(), new PaymentConfig());

            var actual = authService.HashPassword(username, password);

            actual.Should().Be(expectedResult);
        }

    }
}
