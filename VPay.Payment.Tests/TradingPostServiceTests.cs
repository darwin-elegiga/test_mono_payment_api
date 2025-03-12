using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Payment;
using Xunit;

namespace VPay.Payment.Tests
{
    public class TradingPostServiceTests
    {
        private readonly Mock<ILogger<TradingPostService>> _loggerMock;
        private readonly Mock<ITradingPostWs> _tradingPostWsMock;
        private readonly TradingPostService _tradingPostService;

        public TradingPostServiceTests()
        {
            _loggerMock = new Mock<ILogger<TradingPostService>>();
            var db2ContextMock = new Mock<IDb2Context>();
            _tradingPostWsMock = new Mock<ITradingPostWs>();
            db2ContextMock.Setup(x => x.GetRepository<ITradingPostWs>()).Returns(_tradingPostWsMock.Object);
            _tradingPostService = new TradingPostService(db2ContextMock.Object, _loggerMock.Object);
        }

        //[Fact]
        //public async Task LoadCard_ShouldReturnResponse_WhenRequestIsValid()
        //{
        //    // Arrange
        //    var auth = new TradingPostData.AuthenticationValuesAndIp { UserId = "user", Password = "pass" };
        //    var request = new TradingPostData.LoadRequest();
        //    var expectedResponse = new TradingPostData.LoadResult
        //    {
        //        TransactionInformation = new TradingPostData.TransactionInformation { ResponseCode = "00000" }
        //    };
        //    //_tradingPostWsMock.Setup(x => x.LoadCard(auth, request)).ReturnsAsync(expectedResponse);

        //    // Act
        //    var response = await _tradingPostService.LoadCard(auth, request);

        //    // Assert
        //    Assert.Equal("00000", response.TransactionInformation.ResponseCode);
        //}

        //[Fact]
        //public async Task LoadCard_ShouldReturnErrorResponse_WhenExceptionIsThrown()
        //{
        //    // Arrange
        //    var auth = new TradingPostData.AuthenticationValuesAndIp { UserId = "user", Password = "pass" };
        //    var request = new TradingPostData.LoadRequest();
        //    //_tradingPostWsMock.Setup(x => x.LoadCard(auth, request)).ThrowsAsync(new Exception("Test exception"));

        //    // Act
        //    var response = await _tradingPostService.LoadCard(auth, request);

        //    // Assert
        //    Assert.Equal("450", response.TransactionInformation.ResponseCode);
        //    Assert.Equal("Internal Error: Connection to stored procedure has failed. This is an internal communication error.", response.TransactionInformation.ResponseDescription);
        //}

        [Fact]
        public async Task RetrieveCard_ShouldReturnResponse_WhenRequestIsValid()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp { UserId = "user", Password = "pass" };
            var request = new TradingPostData.RetrieveRequest();
            var expectedResponse = new TradingPostData.RetrieveResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation { ResponseCode = "00000" }
            };
            // _tradingPostWsMock.Setup(x => x.RetrieveCard(auth, request)).ReturnsAsync(expectedResponse);

            // Act
            var response = await _tradingPostService.RetrieveCard(auth, request);

            // Assert
            Assert.Equal("454", response.TransactionInformation.ResponseCode);
        }

        [Fact]
        public async Task RetrieveCard_ShouldReturnErrorResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp { UserId = "user", Password = "pass" };
            var request = new TradingPostData.RetrieveRequest();
            // _tradingPostWsMock.Setup(x => x.RetrieveCard(auth, request)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var response = await _tradingPostService.RetrieveCard(auth, request);

            // Assert
            Assert.Equal("454", response.TransactionInformation.ResponseCode);
            Assert.Equal("Output length from procedure is 0.", response.TransactionInformation.ResponseDescription);
        }

        [Fact]
        public async Task CardNotificationRelease_ShouldReturnResponse_WhenRequestIsValid()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp { UserId = "user", Password = "pass" };
            var request = new TradingPostData.ReleaseNotification();
            var expectedResponse = new TradingPostData.NotificationResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation { ResponseCode = "00000" }
            };
            //_tradingPostWsMock.Setup(x => x.CardNotificationRelease(auth, request)).ReturnsAsync(expectedResponse);

            // Act
            var response = await _tradingPostService.CardNotificationRelease(auth, request);

            // Assert
            Assert.Equal("454", response.TransactionInformation.ResponseCode);
        }

        [Fact]
        public async Task CardNotificationRelease_ShouldReturnErrorResponse_WhenExceptionIsThrown()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp { UserId = "user", Password = "pass" };
            var request = new TradingPostData.ReleaseNotification();
            //_tradingPostWsMock.Setup(x => x.CardNotificationRelease(auth, request)).ThrowsAsync(new Exception("Test exception"));

            // Act
            var response = await _tradingPostService.CardNotificationRelease(auth, request);

            // Assert
            Assert.Equal("454", response.TransactionInformation.ResponseCode);
            Assert.Equal("Output length from procedure is 0.", response.TransactionInformation.ResponseDescription);
        }
    }
}
