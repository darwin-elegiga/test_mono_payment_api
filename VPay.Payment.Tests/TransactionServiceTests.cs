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
    }
}
