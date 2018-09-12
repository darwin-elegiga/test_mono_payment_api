using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions;
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

            var dbContext = new Mock<IDb2Context>();
            var mySqlOps = new Mock<IMySqlPaymentOps>();

            var authService = new AuthService(dbContext.Object, mySqlOps.Object, new NullLogger<AuthService>(), new PaymentConfig());

            var actual = authService.HashPassword(username, password);

            actual.Should().Be(expectedResult);
        }

    }
}
