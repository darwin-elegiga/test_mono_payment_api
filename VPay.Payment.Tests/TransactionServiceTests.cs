using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using FaxManagement.Client.v1;
using FluentAssertions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using VPay.Payment.Tests.Models;
using Xunit;
using Xunit.Abstractions;

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
        public async Task LoadPan_WithStandardObjectWithEmptyValues_ShouldSetDefaultValues()
        {
            var request = new StandardRequest();
            request.Claim.UserField1 = "This is a Test";
            request.Claim.CurrencyType = "";

            _db2Context.TransactionWsMock
                .Setup(x => x.LoadPan(It.IsAny<TransactionWsRequest>(), "", default(CancellationToken)))
                .ReturnsAsync(new StandardResponse());

            var result = await _sut.LoadPan(request, "");

            request.Claim.Amount.Should().Be("0.00");
            request.Claim.ClaimOdometer.Should().Be("000000");
            request.Claim.ClaimDeductible.Should().Be("0.00");
            request.Claim.ClaimDate.Should().Be("10000101");
            request.Claim.CurrencyType.Should().Be("USD");
            request.Claim.UserField1.Should().Be("THIS IS A TEST");

            request.CoveredItem.Year.Should().Be("1000");
            request.CoveredItem.Deductible.Should().Be("0.00");
            request.CoveredItem.BeginOdometer.Should().Be("000000");
            request.CoveredItem.ExpireDate.Should().Be("10000101");
            request.CoveredItem.BeginDate.Should().Be("10000101");
        }
        [Fact]
        public async Task LoadPan_WithStandardObjectWithValues_ShouldSetValues()
        {
            var request = new StandardRequest();
            request.Claim.UserField1 = "This is a Test";
            request.Claim.CurrencyType = "USD";
            _db2Context.TransactionWsMock
                .Setup(x => x.LoadPan(It.IsAny<TransactionWsRequest>(), "", default(CancellationToken)))
                .ReturnsAsync(new StandardResponse());
            var result = await _sut.LoadPan(request, "");
            request.Claim.Amount.Should().Be("0.00");
            request.Claim.ClaimOdometer.Should().Be("000000");
            request.Claim.ClaimDeductible.Should().Be("0.00");
            request.Claim.ClaimDate.Should().Be("10000101");
            request.Claim.CurrencyType.Should().Be("USD");
            request.Claim.UserField1.Should().Be("THIS IS A TEST");
            request.CoveredItem.Year.Should().Be("1000");
            request.CoveredItem.Deductible.Should().Be("0.00");
            request.CoveredItem.BeginOdometer.Should().Be("000000");
            request.CoveredItem.ExpireDate.Should().Be("10000101");
            request.CoveredItem.BeginDate.Should().Be("10000101");
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
        //public async Task GetTransactionDetails_ShouldReturnSuccessfulResponse_WhenValidationPasses()
        //{
        //    // Arrange
        //    var request = new TransactionDetailRequest { TransNumber = "123", User = "testUser", Token = "token", Source = 'P' };
        //    var standardRequest = new StandardRequest
        //    {
        //        CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
        //        Source = 'P'
        //    };

        //    //SetupValidation(standardRequest, "0000", "Success");
        //    //SetupPanNumber(standardRequest, "0000", "Success");
        //    //SetupHeaderData(request, "client", "billCode");
        //    //SetupBalanceRequest(standardRequest, "1000", "2000");
        //    //SetupTransactionDetails(request, "client", "billCode");
        //    //SetupCorrespondenceList(request.TransNumber);

        //    // Act
        //    var response = await _sut.GetTransactionDetails(request);

        //    // Assert
        //    Assert.Equal("0000", response.CommonData.SuccessCode);
        //    Assert.Equal("Successful Query", response.CommonData.SuccessDesc);
        //}

        //[Fact]
        //public async Task GetTransactionDetails_ShouldReturnValidationError_WhenValidationFails()
        //{
        //    // Arrange
        //    var request = new TransactionDetailRequest { TransNumber = "123", User = "testUser", Token = "token", Source = 'P' };
        //    //var standardRequest = new StandardRequest
        //    //{
        //    //    CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
        //    //    Source = 'P'
        //    //};

        //    //SetupValidation(standardRequest, "0001", "Validation Error");

        //    // Act
        //    var response = await _sut.GetTransactionDetails(request);

        //    // Assert
        //    Assert.Equal("0001", response.CommonData.SuccessCode);
        //    Assert.Equal("Validation Error", response.CommonData.SuccessDesc);
        //}

        // Additional helper methods for setting up mocks
        //public void SetupValidation(StandardRequest request, string code, string message)
        //{
        //    _transactionService.Setup(x => x.CheckTransactionDetailForValidations(request))
        //        .Returns(new ResponseValidationMessage { code = code, message = message });
        //}

        //private void SetupPanNumber(StandardRequest request, string successCode, string successDesc)
        //{
        //    _sut.Setup(x => x.GetPanNumber(request))
        //        .ReturnsAsync(new PanNumResponse
        //        {
        //            CommonData = new CommonData { SuccessCode = successCode, SuccessDesc = successDesc },
        //            CardData = new CardData(),
        //            CheckData = new CheckData()
        //        });
        //}

        //private void SetupHeaderData(TransactionDetailRequest request, string client, string billCode)
        //{
        //    _db2ContextMock.Setup(x => x.GetRepository<ITransactionWs>().TransactionHeadersData(request.Token, request.User, request.TransNumber))
        //        .ReturnsAsync(new List<HeaderData> { new HeaderData { Client = client, BillCode = billCode } });
        //}

        //private void SetupBalanceRequest(StandardRequest request, string availableBal, string currentBal)
        //{
        //    _transactionService.Setup(x => x.GetBalanceRequest(request))
        //        .ReturnsAsync(new StandardResponse
        //        {
        //            SwitchTransaction = new SwitchTransaction { AvailableBal = availableBal, CurrentBal = currentBal }
        //        });
        //}

        //private void SetupTransactionDetails(TransactionDetailRequest request, string client, string billCode)
        //{
        //    _db2ContextMock.Setup(x => x.GetRepository<ITransactionWs>().TransactionDetailsData(request.Token, client, billCode, request.TransNumber))
        //        .ReturnsAsync(new List<DetailData>());
        //}

        //private void SetupCorrespondenceList(string transNumber)
        //{
        //    _transactionService.Setup(x => x.GetCorrespondenceList(long.Parse(transNumber)))
        //        .ReturnsAsync(new List<CorrespondenceData>());
        //}
        //[Fact]
        //public async Task GetPanNumberWithMockData()
        //{
        //    var request = new StandardRequest();
        //    request.Claim.UserField1 = "This is a Test";
        //    request.Claim.CurrencyType = "USD";
        //    _db2Context.TransactionWsMock
        //        .Setup(x => x.LoadPan(It.IsAny<TransactionWsRequest>(), "", default(CancellationToken)))
        //        .ReturnsAsync(new StandardResponse());
        //    var response = await _sut.GetPanNumber(request);
        //    Assert.ThrowsAny<System.NullReferenceException>(() => response);
        //    //response.Should().NotBeNull();
        //}
        //[Fact]
        //public async Task GetTransactionDetailsWithMockData()
        //{
        //    var request = new StandardRequest();
        //    request.Claim.UserField1 = "This is a Test";
        //    request.Claim.CurrencyType = "USD";
        //    _db2Context.TransactionWsMock
        //        .Setup(x => x.LoadPan(It.IsAny<TransactionWsRequest>(), "", default(CancellationToken)))
        //        .ReturnsAsync(new StandardResponse());
        //    var request1 = new TransactionDetailRequest()
        //    {
        //        TransNumber = "12345",
        //        Token = null,
        //        User = "Test"
        //    };
        //    _sut.Get
        //    var response = await _sut.GetTransactionDetails(request1);
        //    response.Should().NotBeNull();
        //}
    }
}
