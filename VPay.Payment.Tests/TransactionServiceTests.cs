using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FaxManagement.Client.v1;
using FaxManagement.Client.v1.Models;
using FluentAssertions;
using IBM.Data.Db2;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.Fax;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using VPay.Payment.Tests.Models;
using Xunit;

namespace VPay.Payment.Tests
{
    public class TransactionServiceTests
    {
        private readonly TransactionService _sut;
        private readonly Db2ContextMock _db2Context;
        private readonly Mock<IUserInfo> _user;
        private readonly Mock<IFaxQueueV1Client> _fax;
        private readonly NullLogger<TransactionService> _logger;

        public TransactionServiceTests()
        {
            _db2Context = new Db2ContextMock();
            _user = new Mock<IUserInfo>();
            _fax = new Mock<IFaxQueueV1Client>();
            _logger = new NullLogger<TransactionService>();
            _sut = new TransactionService(_db2Context, _user.Object, _fax.Object, _logger);
        }

        [Fact]
        public async Task GetReasonCodes_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new ReasonCodeRequest { TransNumber = "123", User = "testUser", Token = "token" };
            var panRequest = new ReasonCodeResponse { CommonData = new CommonData { SuccessCode = "0002" } };

            _db2Context.TransactionWsMock
                .Setup(x => x.GetPan(It.IsAny<TransactionWsRequest>(), default(CancellationToken)))
                .ReturnsAsync(new StandardResponse());

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
            var panResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0002" } };
            _db2Context.TransactionWsMock
                .Setup(x => x.GetPan(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(panResponse);

            // Act
            var result = await _sut.GetTransactionDetails(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0002");
        }

        [Fact]
        public async Task GetPanNumber_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var panResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };
            _db2Context.TransactionWsMock
                .Setup(x => x.GetPan(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(panResponse);

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
            var preAuthResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };
            _db2Context.TransactionWsMock
                .Setup(x => x.OpenPreAuth(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(preAuthResponse);

            // Act
            var result = await _sut.OpenPreAuth(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        //[Fact]
        //public async Task LoadPan_ShouldReturnExpectedResponse()
        //{
        //    // Arrange
        //    var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
        //    var loadPanResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };
        //    string clientData = "test";
        //    //_db2Context.GetRepository<ITransactionWs>().LoadPan(request, clientData);
        //    _db2Context.TransactionWsMock
        //        .Setup(x => x.LoadPan(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(loadPanResponse);

        //    // Act
        //    var result = await _sut.LoadPan(request, "clientData");

        //    // Assert
        //    result.CommonData.SuccessCode.Should().Be("0000");
        //}

        [Fact]
        public async Task GetBalanceRequest_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new StandardRequest { CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" } };
            var balanceResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };
            _db2Context.TransactionWsMock.Setup(x => x.BalanceRequest(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(balanceResponse);

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
            var unloadPanResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };
            _db2Context.TransactionWsMock.Setup(x => x.UnloadPan(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(unloadPanResponse);

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
            var stopPayResponse = new StandardResponse { CommonData = new CommonData { SuccessCode = "0000" } };
            _db2Context.TransactionWsMock.Setup(x => x.StopPay(It.IsAny<TransactionWsRequest>(), default(CancellationToken))).ReturnsAsync(stopPayResponse);

            // Act
            var result = await _sut.StopPay(request);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public async Task ResendFax_ShouldReturnExpectedResponse()
        {
            // Arrange
            var faxCode = 123;
            var faxNumber = "1234567890";
            var faxJob = new FaxJobDto { TransactionIds = new long[] { 1, 3 }, CorrespondenceId = 1 };
            _fax.Setup(x => x.GetFaxJob(It.IsAny<FaxJobRequestDto>())).ReturnsAsync(faxJob);
            var dbResult = new ResendFaxResult { SuccessCode = "0000", SuccessDescription = "Success", FaxNumber = faxNumber };
            _db2Context.FaxMock
                .Setup(x => x.ResendFaxAsync(It.IsAny<string>(), It.IsAny<long>(), It.IsAny<long>(), It.IsAny<string>(), default(CancellationToken))).ReturnsAsync(dbResult);

            // Act
            var result = await _sut.ResendFax(faxCode, faxNumber);

            // Assert
            result.CommonData.SuccessCode.Should().Be("0000");
        }

        [Fact]
        public void CheckTransactionDetailForValidations_ValidTransNumber_ReturnsSuccessfulValidation()
        {
            var methodInfo = typeof(TransactionService).GetMethod("CheckTransactionDetailForValidations", BindingFlags.NonPublic | BindingFlags.Instance);
            // Arrange
            var request = new StandardRequest
            {
                CommonData = new CommonData { TransNumber = "1234567890" }
            };

            var parameters = new object[] { request };

            // Act
            var result = methodInfo.Invoke(_sut, parameters);

            // Assert
            Assert.Contains("0000", result.ToString());
            Assert.Contains("Successful Validation", result.ToString());
        }

        [Fact]
        public void CheckTransactionDetailForValidations_InvalidTransNumber_ReturnsInvalidTransNumber()
        {
            var methodInfo = typeof(TransactionService).GetMethod("CheckTransactionDetailForValidations", BindingFlags.NonPublic | BindingFlags.Instance);
            // Arrange
            var request = new StandardRequest
            {
                CommonData = new CommonData { TransNumber = "InvalidNumber" }
            };
            var parameters = new object[] { request };

            // Act
            var result = methodInfo.Invoke(_sut, parameters);

            // Assert
            Assert.Contains("0997", result.ToString());
            Assert.Contains("Invalid Trans Number", result.ToString());
        }

        //[Fact]
        //public async Task GetCorrespondenceList_ShouldReturnExpectedResponse()
        //{
        //    // Arrange
        //    var transactionId = 123L;
        //    var correspondenceList = new List<CorespDtl> { new CorespDtl { DmRecId = 1 } };
        //    _db2Context.CorrespondenceMock
        //        .Setup(x => x.GetByTransactionId(transactionId, default(CancellationToken))).ReturnsAsync(correspondenceList);


        //    // Act
        //    var result = await _sut.GetCorrespondenceList(transactionId);

        //    // Assert
        //    result.Should().BeEquivalentTo(correspondenceList);
        //}

        [Fact]
        public void SetupDefaultValuesForLoadPan_ShouldSetDefaultValues()
        {
            var methodInfo = typeof(TransactionService).GetMethod("SetupDefaultValuesForLoadPan", BindingFlags.NonPublic | BindingFlags.Instance);
            // Arrange
            var request = new StandardRequest
            {
                Claim = new ClaimData(),
                CoveredItem = new CoveredItemData()
            };

            var parameters = new object[] { request };

            // Act
            methodInfo.Invoke(_sut, parameters);

            // Assert
            request.Claim.Amount.Should().Be("0.00");
            request.Claim.ClaimOdometer.Should().Be("000000");
            request.Claim.ClaimDeductible.Should().Be("0.00");
            request.Claim.ClaimDate.Should().Be("10000101");
            request.Claim.CurrencyType.Should().Be("840");

            request.CoveredItem.Year.Should().Be("1000");
            request.CoveredItem.Deductible.Should().Be("0.00");
            request.CoveredItem.BeginOdometer.Should().Be("000000");
            request.CoveredItem.ExpireDate.Should().Be("10000101");
            request.CoveredItem.BeginDate.Should().Be("10000101");
        }

        [Fact]
        public void CheckLoadPanForDeclineErrorMessages_ShouldReturnExpectedResponse()
        {
            var methodInfo = typeof(TransactionService).GetMethod("CheckLoadPanForDeclineErrorMessages", BindingFlags.NonPublic | BindingFlags.Instance);
            // Arrange
            var request = new StandardRequest
            {
                Claim = new ClaimData { Amount = "100.00" },
                CoveredItem = new CoveredItemData { Year = "2022", Deductible = "50.00", BeginOdometer = "1000" },
                Payment = new PaymentData { Type = "CLNPF" }
            };

            var parameters = new object[] { request };

            // Act
            var result = methodInfo.Invoke(_sut, parameters);

            // Assert
            result.Should().BeEquivalentTo(("0909", "Payee Code cannot be Blank"));
        }

        [Fact]
        public void ClearCommonData_ShouldClearCommonData()
        {
            var methodInfo = typeof(TransactionService).GetMethod("ClearCommonData", BindingFlags.NonPublic | BindingFlags.Instance);
            // Arrange
            var response = new StandardResponse
            {
                CommonData = new CommonData { ReasonCode = "123", ReasonDesc = "Test", Token = "token" }
            };

            var parameters = new object[] { response };

            // Act
            methodInfo.Invoke(_sut, parameters);

            // Assert
            response.CommonData.ReasonCode.Should().BeEmpty();
            response.CommonData.ReasonDesc.Should().BeEmpty();
            response.CommonData.Token.Should().BeEmpty();
        }

        [Fact]
        public void PackAndUnpackResponse_ShouldReturnExpectedResponse()
        {
            var methodInfo = typeof(TransactionService).GetMethod("PackAndUnpackResponse", BindingFlags.NonPublic | BindingFlags.Instance);
            // Act
            var result = methodInfo.Invoke(_sut, null);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
