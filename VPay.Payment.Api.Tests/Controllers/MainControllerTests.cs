using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Auth;
using VPay.Payment.Api.Controllers;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Api.Tests.Controllers
{
    public class MainControllerTests
    {
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<ITransactionService> _mockTransactionService;
        private readonly Mock<IUserInfo> _mockUserInfo;
        private readonly MainController _controller;
        public MainControllerTests()
        {
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockTransactionService = new Mock<ITransactionService>();
            _mockUserInfo = new Mock<IUserInfo>();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, "testUser"),
                new Claim(ClaimTypes.NameIdentifier, "token"),
                new Claim(ClaimTypes.System, "P")
            };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.User).Returns(claimsPrincipal);
            _mockHttpContextAccessor.Setup(a => a.HttpContext).Returns(mockHttpContext.Object);

            _controller = new MainController(_mockHttpContextAccessor.Object, _mockTransactionService.Object, _mockUserInfo.Object);
        }

        [Fact]
        public async Task GetReasonCodes_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new ReasonCodeResponse();
            _mockTransactionService.Setup(s => s.GetReasonCodes(It.IsAny<ReasonCodeRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetReasonCodes(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetTransactionDetails_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new TransactionDetailResponse();
            _mockTransactionService.Setup(s => s.GetTransactionDetails(It.IsAny<TransactionDetailRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetTransactionDetails(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task GetPanNumber_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new StandardResponse();
            _mockUserInfo.Setup(s => s.Source).Returns("P");
            _mockTransactionService.Setup(s => s.GetPanNumber(It.IsAny<StandardRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.GetPanNumber(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task OpenPreAuth_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new StandardResponse();
            _mockUserInfo.Setup(s => s.Source).Returns("P");
            _mockTransactionService.Setup(s => s.OpenPreAuth(It.IsAny<StandardRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.OpenPreAuth(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task LoadPan_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new StandardResponse();
            _mockUserInfo.Setup(s => s.Source).Returns("P");
            _mockTransactionService.Setup(s => s.LoadPan(It.IsAny<StandardRequest>(), It.IsAny<string>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.LoadPan(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task BalanceRequest_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new StandardResponse();
            _mockUserInfo.Setup(s => s.Source).Returns("P");
            _mockTransactionService.Setup(s => s.GetBalanceRequest(It.IsAny<StandardRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.BalanceRequest(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task UnloadPan_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new StandardResponse();
            _mockUserInfo.Setup(s => s.Source).Returns("P");
            _mockTransactionService.Setup(s => s.UnloadPan(It.IsAny<StandardRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.UnloadPan(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public async Task StopPay_ShouldReturnExpectedResponse()
        {
            // Arrange
            var transNumber = "123";
            var expectedResponse = new StandardResponse();
            _mockUserInfo.Setup(s => s.Source).Returns("P");
            _mockTransactionService.Setup(s => s.StopPay(It.IsAny<StandardRequest>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.StopPay(transNumber);

            // Assert
            Assert.Equal(expectedResponse, result);
        }

        [Fact]
        public void CancelFax_ShouldReturnDeprecatedResponse()
        {
            // Arrange
            var request = new FaxRequest();
            var expectedResponse = new StandardResponse
            {
                CommonData = new CommonData
                {
                    SuccessCode = "9997",
                    SuccessDesc = "This API endpoint has been deprecated."
                }
            };

            // Act
            var result = _controller.CancelFax(request);

            // Assert
            Assert.Equal(expectedResponse.CommonData.SuccessCode, result.CommonData.SuccessCode);
            Assert.Equal(expectedResponse.CommonData.SuccessDesc, result.CommonData.SuccessDesc);
        }

        [Fact]
        public void ChangeFaxNumber_ShouldReturnDeprecatedResponse()
        {
            // Arrange
            var request = new ChangeFaxNumberRequest();
            var expectedResponse = new StandardResponse
            {
                CommonData = new CommonData
                {
                    SuccessCode = "9997",
                    SuccessDesc = "This API endpoint has been deprecated."
                }
            };

            // Act
            var result = _controller.ChangeFaxNumber(request);

            // Assert
            Assert.Equal(expectedResponse.CommonData.SuccessCode, result.CommonData.SuccessCode);
            Assert.Equal(expectedResponse.CommonData.SuccessDesc, result.CommonData.SuccessDesc);
        }

        [Fact]
        public void HoldFax_ShouldReturnDeprecatedResponse()
        {
            // Arrange
            var request = new FaxRequest();
            var expectedResponse = new StandardResponse
            {
                CommonData = new CommonData
                {
                    SuccessCode = "9997",
                    SuccessDesc = "This API endpoint has been deprecated."
                }
            };

            // Act
            var result = _controller.HoldFax(request);

            // Assert
            Assert.Equal(expectedResponse.CommonData.SuccessCode, result.CommonData.SuccessCode);
            Assert.Equal(expectedResponse.CommonData.SuccessDesc, result.CommonData.SuccessDesc);
        }

        [Fact]
        public void ReleaseFax_ShouldReturnDeprecatedResponse()
        {
            // Arrange
            var request = new FaxRequest();
            var expectedResponse = new StandardResponse
            {
                CommonData = new CommonData
                {
                    SuccessCode = "9997",
                    SuccessDesc = "This API endpoint has been deprecated."
                }
            };

            // Act
            var result = _controller.ReleaseFax(request);

            // Assert
            Assert.Equal(expectedResponse.CommonData.SuccessCode, result.CommonData.SuccessCode);
            Assert.Equal(expectedResponse.CommonData.SuccessDesc, result.CommonData.SuccessDesc);
        }

        [Fact]
        public async Task ResendFax_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new ResendFaxRequest { FaxCode = 123 };
            var expectedResponse = new StandardResponse();
            _mockTransactionService.Setup(s => s.ResendFax(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.ResendFax(request);

            // Assert
            Assert.Equal(expectedResponse, result);
        }
    }
}
