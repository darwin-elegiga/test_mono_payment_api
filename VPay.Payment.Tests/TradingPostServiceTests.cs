using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Moq;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Tests
{
    public class TradingPostServiceTests
    {
        private readonly Mock<IDb2Context> _db2ContextMock;
        private readonly Mock<ITradingPostWs> _tradingPostWsMock;
        private readonly Mock<ILogger<TradingPostService>> _loggerMock;
        private readonly TradingPostService _tradingPostService;
        private readonly CancellationToken _cancellationToken;
        //CancellationToken cancellationToken = default(CancellationToken)

        public TradingPostServiceTests()
        {
            _db2ContextMock = new Mock<IDb2Context>();
            _tradingPostWsMock = new Mock<ITradingPostWs>();
            _loggerMock = new Mock<ILogger<TradingPostService>>();
            _db2ContextMock.Setup(x => x.GetRepository<ITradingPostWs>()).Returns(_tradingPostWsMock.Object);
            _tradingPostService = new TradingPostService(_db2ContextMock.Object, _loggerMock.Object);
            _cancellationToken = new CancellationToken();
        }

        [Fact]
        public async Task LoadCard_ShouldReturnExpectedResponse()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp();
            var request = new TradingPostData.LoadRequest();
            request.CardHolder = new TradingPostData.Cardholder();
            request.AchDirections = new TradingPostData.AchDirections();
            request.CheckInformation = new TradingPostData.CheckInformation();
            request.Merchant = new TradingPostData.Merchant
                {
                    ContactPerson = "John Doe",
                    Fax = "12345",
                    PayeeCode = "123456",
                    Telephone = "985566778",
                    PayeeName = "Test",
                    PostalCode = "12345676"
                 };
            
            var expectedResponse = new TradingPostData.LoadResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation
                {
                    ResponseCode = "00000"
                }
            };

            _tradingPostWsMock.Setup(x => x.LoadCard(auth, request,_cancellationToken)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _tradingPostService.LoadCard(auth, request);

            // Assert
            Assert.Equal("00000", result.TransactionInformation.ResponseCode);
        }

        [Fact]
        public async Task RetrieveCard_ShouldReturnExpectedResponse()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp();
            var request = new TradingPostData.RetrieveRequest();
            var expectedResponse = new TradingPostData.RetrieveResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation
                {
                    ResponseCode = "00000"
                }
            };
            _tradingPostWsMock.Setup(x => x.RetrieveCard(auth, request, _cancellationToken)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _tradingPostService.RetrieveCard(auth, request);

            // Assert
            Assert.Equal("00000", result.TransactionInformation.ResponseCode);
        }

        [Fact]
        public async Task CardNotificationRelease_ShouldReturnExpectedResponse()
        {
            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp();
            var request = new TradingPostData.ReleaseNotification();
            var expectedResponse = new TradingPostData.NotificationResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation
                {
                    ResponseCode = "00000"
                }
            };
            _tradingPostWsMock.Setup(x => x.CardNotificationRelease(auth, request, _cancellationToken)).ReturnsAsync(expectedResponse);

            // Act
            var result = await _tradingPostService.CardNotificationRelease(auth, request);

            // Assert
            Assert.Equal("00000", result.TransactionInformation.ResponseCode);
        }

        [Fact]
        public void ValidateAuth_ShouldReturnExpectedValidationMessage()
        {
            var methodInfo = typeof(TradingPostService).GetMethod("ValidateAuth", BindingFlags.NonPublic | BindingFlags.Instance);

            // Arrange
            var auth = new TradingPostData.AuthenticationValuesAndIp();
           
            var parameters = new object[] { auth };

            // Act
            var result = methodInfo.Invoke(_tradingPostService, parameters);
            Type type = result.GetType();

            PropertyInfo propertyInfo = type.GetProperty("Code");

            var codeVal = propertyInfo.GetValue(result, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("305", codeVal);
        }

        [Fact]
        public void ValidateLoadCard_ShouldReturnExpectedValidationMessage()
        {
            var methodInfo = typeof(TradingPostService).GetMethod("ValidateLoadCard", BindingFlags.NonPublic | BindingFlags.Instance);

            // Arrange
            var req = new TradingPostData.LoadRequest();

            var parameters = new object[] { req };

            // Act
            var result = methodInfo.Invoke(_tradingPostService, parameters);
            Type type = result.GetType();

            PropertyInfo propertyInfo = type.GetProperty("Code");

            var codeVal = propertyInfo.GetValue(result, null);
            

            // Assert
            Assert.NotNull(result);
            Assert.Equal("302", codeVal);
        }

        [Fact]
        public void ValidateRetrieveCard_ShouldReturnExpectedValidationMessage()
        {
            var methodInfo = typeof(TradingPostService).GetMethod("ValidateRetrieveCard", BindingFlags.NonPublic | BindingFlags.Instance);

            // Arrange
            var req = new TradingPostData.RetrieveRequest();

            var parameters = new object[] { req };

            // Act
            var result = methodInfo.Invoke(_tradingPostService, parameters);
            Type type = result.GetType();

            PropertyInfo propertyInfo = type.GetProperty("Code");

            var codeVal = propertyInfo.GetValue(result, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("401", codeVal);
        }

        [Fact]
        public void ValidateNotifyCard_ShouldReturnExpectedValidationMessage()
        {
            var methodInfo = typeof(TradingPostService).GetMethod("ValidateNotifyCard", BindingFlags.NonPublic | BindingFlags.Instance);

            // Arrange
            var req = new TradingPostData.ReleaseNotification();

            var parameters = new object[] { req };

            // Act
            var result = methodInfo.Invoke(_tradingPostService, parameters);
            Type type = result.GetType();

            PropertyInfo propertyInfo = type.GetProperty("Code");

            var codeVal = propertyInfo.GetValue(result, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("302", codeVal);
        }

        [Fact]
        public void ValidateDecimalAmount_ShouldReturnExpectedValidationMessage()
        {
            var methodInfo = typeof(TradingPostService).GetMethod("ValidateDecimalAmount", BindingFlags.NonPublic | BindingFlags.Instance);

            // Arrange
            var amount = "-100";
            var fieldName = "claim amount";

            var parameters = new object[] { amount,fieldName };

            // Act
            var result = methodInfo.Invoke(_tradingPostService, parameters);
            Type type = result.GetType();

            PropertyInfo propertyInfo = type.GetProperty("Code");

            var codeVal = propertyInfo.GetValue(result, null);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("305", codeVal);
        }
    }
}
