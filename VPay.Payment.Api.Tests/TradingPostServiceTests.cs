using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;
using Xunit;



namespace VPay.Payment.Tests
{
    public class TradingPostServiceTests
    {
        private readonly NullLogger<ILogger> _logger;
        private readonly ITradingPostService _tradingPostService;
        private readonly Mock<IDb2Context> _db2Context;
        private readonly Mock<ITradingPostWs> _tradingPostWs;

        public TradingPostServiceTests()
        {
            _logger = new NullLogger<ILogger>();
            _db2Context = new Mock<IDb2Context>();
            _tradingPostWs = new Mock<ITradingPostWs>();

            _db2Context.Setup(_ => _.TradingPostWs).Returns(_tradingPostWs.Object);

            _tradingPostWs.Setup(_ => _.RetrieveCard(It.IsAny<TradingPostData.AuthenticationValuesAndIp>(), It.IsAny<TradingPostData.RetrieveRequest>(), CancellationToken.None))
            .Returns(Task.FromResult(new TradingPostData.RetrieveResult()));

            _tradingPostService = new TradingPostService(_db2Context.Object, new NullLogger<TradingPostService>());
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task ValidateBillingCode_ShouldReturnAMessageWithCode0907(string billingCode)
        {
            var setupObj = new TradingPostRequest()
            {
                Envelope = new TradingPostEnvelope()
                {
                    Body = new TradingPostBody()
                    {
                        RetrieveCard = new TradingPostRetrieveRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            RetrieveRequest = new TradingPostData.RetrieveRequest()
                            {
                                AchDirections = new TradingPostData.AchDirections()
                                {
                                    PayeeAccountNumber = "string"
                                    , PayeeRoutingNumber = "string"
                                },
                                CardHolder = new TradingPostData.Cardholder()
                                {
                                    BillingCode = billingCode
                                    ,BillingType = "string"
                                    ,Client = "string"
                                },
                                CardInformation = new TradingPostData.CardInformation()
                                {
                                     CardholderAddress = "string"
                                    ,CardExpiration = "string"
                                    ,CardholderName = "string"
                                    ,CardholderPostalCode = "string" 
                                    ,CardNumber = "string"
                                    ,CardSecurityValue = "string"
                                    ,CardType = "string"
                                    ,LoadAmount = 0
                                    ,LoadFee = 0
                                    ,LoadTransactionId = 0
                                    ,PayeeName = "string"
                                },
                                CheckInformation = new TradingPostData.CheckInformation()
                                {
                                     CheckDate= "string"
                                    ,CheckNumber= "string"
                                },
                                Claim = new TradingPostData.Claim()
                                {
                                     Amount = "0"
                                    ,ClaimDate = "string"
                                    ,ClaimDeductible = "0"
                                    ,ClaimDescription = "string"
                                    ,ClaimNotes = "string"
                                    ,ClaimOdometer = "0"
                                    ,CurrencyType = "string"
                                    ,RepairOrderId = "string"
                                    ,RequesterId ="string"
                                    ,RequesterName = "string"
                                    ,UserField1 = "string"
                                    ,UserField2 = "string"
                                    ,UserField3 = "string"
                                    ,UserKey = "string"
                                },
                                Correspondence = new TradingPostData.Correspondence()
                                {
                                    AttachmentLocation = "string"
                                    ,SendFaxCode = "string"
                                },
                                CoveredItem = new TradingPostData.CoveredItem()
                                {
                                     BeginDate = "string"
                                    ,BeginOdometer = "0"
                                    ,BookStateOrProvince = "string"
                                    ,Deductible = "0"
                                    , ExpireDate = "string"
                                    ,ExpireOdometer = "0"
                                    ,ItemId = "string"
                                    ,ItemType = "string"
                                    ,Manufacturer = "string"
                                    ,Model = "string"
                                    ,NewUsed = "string"
                                    ,OdometerType = "string"
                                    ,OwnerFirstName = "string"
                                    ,OwnerLastName = "string"
                                    ,PlanCode = "string"
                                    ,PlanDescription = "string"
                                    ,PostalCode = "string"
                                    ,Year = "string"
                                },
                                Merchant = new TradingPostData.Merchant()
                                {
                                     ContactPerson = "string"
                                    ,Fax = "string"
                                    ,PayeeCode = "string"
                                    ,PayeeName = "string"
                                    ,PostalCode = "string"
                                    ,Telephone = "string"
                                }
                            }
                        }
                    }
                }
            };

            var auth = new TradingPostData.AuthenticationValuesAndIp();

            var actual = await _tradingPostService.RetrieveCard(auth, setupObj.Envelope.Body.RetrieveCard.RetrieveRequest);

            auth.InitialErrorCode.Should().Be("907");
            auth.InitialErrorMessage.Should().Be("billing code is not valid");
        }
    }
}
