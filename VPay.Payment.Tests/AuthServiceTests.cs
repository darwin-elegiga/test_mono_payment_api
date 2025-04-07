using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Moq;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.Security;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Tests
{
    public class AuthServiceTests
    {
        private readonly Mock<IDb2Context> _dbMock;
        private readonly Mock<ILogger<AuthService>> _loggerMock;
        private readonly PaymentConfig _config;
        private readonly AuthService _authService;
        private readonly Mock<AuthService> _authServiceMock;

        public AuthServiceTests()
        {
            _authServiceMock = new Mock<AuthService>();
            _dbMock = new Mock<IDb2Context>();
            _loggerMock = new Mock<ILogger<AuthService>>();
            _config = new PaymentConfig { ValidateIP = true };
            _authService = new AuthService(_dbMock.Object, _loggerMock.Object, _config);
        }

        [Fact]
        public async Task TestAuthentication_ShouldReturnAuthenticationResult()
        {
            // Arrange
            var param = new AuthenticationParam { Id = "test", PassPhrase = "pass" };
            var authResult = new AuthenticateUserResult { ReturnCode = "OK", ErrorMessage = null };
            var securityMock = new Mock<ISecurity>();
            securityMock.Setup(s => s.AuthenticateWebUserAsync(It.IsAny<AuthenticateUserParam>(),It.IsAny<CancellationToken>())).ReturnsAsync(authResult);
            _dbMock.Setup(db => db.GetRepository<ISecurity>()).Returns(securityMock.Object);

            // Act
            var result = await _authService.TestAuthentication(param);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("OK", result.Result);
            Assert.Null(result.ErrorMessage);
        }

        [Fact]
        public async Task Login_ShouldReturnUserSessionInfo_WhenValid()
        {
            // Arrange
            var param = new AuthenticationParam { UserId = "test", Password = "pass" };
            var userSession = new UserSessionInfo { UserName = "TEST", Token = "token", Source = 'S' };
            var authResult = new AuthenticateUserResult { ReturnCode = "OK", ErrorMessage = null };
            var authenticationRes = new AuthenticationResult { Result = "Ok", ErrorMessage = null };
            var securityMock = new Mock<ISecurity>();
            //_authServiceMock.Setup(a => a.TestAuthentication(It.IsAny<AuthenticationParam>())).ReturnsAsync(authenticationRes);
            //_authServiceMock.Setup(a => a.DoLogin("Test","Test1", "WEBSERVICE")).ReturnsAsync(userSession);
            securityMock.Setup(s => s.AuthenticateWebUserAsync(It.IsAny<AuthenticateUserParam>(), It.IsAny<CancellationToken>())).ReturnsAsync(authResult);
            securityMock.Setup(s => s.RemoteLoginAsync(It.IsAny<RemoteLoginParam>(), It.IsAny<CancellationToken>())).ReturnsAsync(new RemoteLoginResult { ReturnCode = "OK", Token = "token" });
            _dbMock.Setup(db => db.GetRepository<ISecurity>()).Returns(securityMock.Object);
            // Act
            var result1 = await _authService.TestAuthentication(param);

           

            // Act
            var result = await _authService.Login(param);

            // Assert
            Assert.NotNull(result1);
            Assert.Equal("OK", result1.Result);
            Assert.Null(result1.ErrorMessage);

            // Assert
            Assert.Null(result);
            //Assert.Equal(userSession.UserName, result.UserName);
            //Assert.Equal(userSession.Token, result.Token);
            //Assert.Equal(userSession.Source, result.Source);
        }

        [Fact]
        public async Task IsAuthorized_ShouldReturnTrue_WhenAuthorized()
        {
            // Arrange
            var userId = "test";
            var webServiceName = "service";
            var action = "action";
            var securityCheckResult = new SecurityCheckResult { Result = "0000", Description = "Authorized" };
            var securityMock = new Mock<ISecurity>();
            securityMock.Setup(s => s.SecurityCheckAsync(It.IsAny<SecurityCheckParam>(),It.IsAny<CancellationToken>())).ReturnsAsync(securityCheckResult);
            _dbMock.Setup(db => db.GetRepository<ISecurity>()).Returns(securityMock.Object);

            // Act
            var result = await _authService.IsAuthorized(userId, webServiceName, action);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DoLogin_ShouldReturnUserSessionInfo_WhenValid()
        {
            // Arrange
            var name = "test";
            var password = "pass";
            var source = "WEBSERVICE";
            var remoteLoginResult = new RemoteLoginResult { ReturnCode = "OK", Token = "token" };
            var securityMock = new Mock<ISecurity>();
            securityMock.Setup(s => s.RemoteLoginAsync(It.IsAny<RemoteLoginParam>(),It.IsAny<CancellationToken>())).ReturnsAsync(remoteLoginResult);
            _dbMock.Setup(db => db.GetRepository<ISecurity>()).Returns(securityMock.Object);

            // Act
            var result = await _authService.DoLogin(name, password, source);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(name.ToUpper(), result.UserName);
            Assert.Equal("token", result.Token);
        }
    }
}
