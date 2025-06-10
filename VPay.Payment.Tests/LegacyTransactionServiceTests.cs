using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Tests
{
    public class LegacyTransactionServiceTests
    {
        private readonly LegacyTransactionService _sut;
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<ILegacyValidationService> _validationServiceMock;
        private readonly NullLogger<LegacyTransactionService> _logger;

        public LegacyTransactionServiceTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _validationServiceMock = new Mock<ILegacyValidationService>();
            _logger = new NullLogger<LegacyTransactionService>();
            _sut = new LegacyTransactionService(_transactionServiceMock.Object, _validationServiceMock.Object, _logger);
        }

        [Fact]
        public async Task GetReasonCodes_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new ReasonCodeRequest { TransNumber = "123", User = "testUser", Token = "token" };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new ReasonCodeResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.GetReasonCodes(It.IsAny<ReasonCodeRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.GetReasonCodes(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task GetTransactionDetails_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new TransactionDetailRequest { TransNumber = "123", User = "testUser", Token = "token", Source = 'P' };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new TransactionDetailResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.GetTransactionDetails(It.IsAny<TransactionDetailRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.GetTransactionDetails(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task GetPanNumber_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.GetPanNumber(It.IsAny<StandardRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.GetPanNumber(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task OpenPreAuth_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.OpenPreAuth(It.IsAny<StandardRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.OpenPreAuth(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task LoadPan_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var clientData = "testClientData";
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateLoadPanStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.LoadPan(It.IsAny<StandardRequest>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.LoadPan(request, clientData);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task GetBalanceRequest_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.GetBalanceRequest(It.IsAny<StandardRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.GetBalanceRequest(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task UnloadPan_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.UnloadPan(It.IsAny<StandardRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.UnloadPan(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task StopPay_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateStandardRequest(It.IsAny<StandardRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.StopPay(It.IsAny<StandardRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.StopPay(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task ResendFax_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CorrespondenceData = new CorrespondenceData { PhoneNumber = "1234567890" } };
            var validationResponse = new ValidationMessage { Code = "0000" };
            var expectedResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };

            _validationServiceMock
                .Setup(x => x.ValidateResendFaxNumberRequest(It.IsAny<StandardRequest>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(validationResponse);

            _transactionServiceMock
                .Setup(x => x.ResendFax(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _sut.ResendFax(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }
    }
}
