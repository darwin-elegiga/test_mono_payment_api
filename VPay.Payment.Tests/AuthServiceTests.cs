using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Payment.Common.Db2;
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

            var authService = new AuthService(dbOps.Object, new NullLogger<AuthService>());

            var actual = authService.HashPassword(username, password);

            actual.Should().Be(expectedResult);
        }

    }
}
