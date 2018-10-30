using System;
using System.Threading.Tasks;
using FluentAssertions;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using VPay.Payment.Common.Models;
using Xunit;

namespace VPay.Payment.Tests
{
    public class LegacyValidationServiceTests
    {
        private readonly LegacyValidationService _sut;
        

        public LegacyValidationServiceTests()
        {
            _sut = new LegacyValidationService();
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAValidStandardRequest_ShouldReturnAMessageWithCode0000AndMessageSuccessful()
        {
            var standardRequest = GetValidStandardRequest();

            var expected = new ValidationMessage()
            {
                Code = "0000",
                Message = "Successful Validation"
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateChangeFaxNumberRequest_WithAValidStandardRequest_ShouldReturnAMessageWithCode0000AndMessageSuccessful()
        {
            var standardRequest = GetValidStandardRequest();

            var originalFax = "1-888-963-9623";

            var expected = new ValidationMessage()
            {
                Code = "0000",
                Message = "Successful Validation"
            };

            var actual = await _sut.ValidateChangeFaxNumberRequest(standardRequest, originalFax);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public async Task
            ValidateChangeFaxNumberRequest_WithAMissingFaxNumber_ShouldReturnAMessageWithCode0055AndMessageError(string phoneNumber)
        {
            var standardRequest = GetValidStandardRequest();
            standardRequest.CorrespondenceData.PhoneNumber = phoneNumber;

            var originalFax = "";

            var expected = new ValidationMessage()
            {
                Code = "0055",
                Message = "No Fax Number Provided"
            };

            var actual = await _sut.ValidateChangeFaxNumberRequest(standardRequest, originalFax);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("1-236-962-312")]
        [InlineData("123")]
        [InlineData("1(236)-962-312")]
        [InlineData("a(012)-345-6789")]
        [InlineData("+(012)-345-6789")]
        public async Task
            ValidateChangeFaxNumberRequest_WithInvalidPhoneNumber_ShouldReturnAMessageWithCode0005AndMessageError(string phoneNumber)
        {
            var standardRequest = GetValidStandardRequest();
            standardRequest.CorrespondenceData.PhoneNumber = phoneNumber.CleanFaxNumber();

            var originalFax = phoneNumber;

            var expected = new ValidationMessage()
            {
                Code = "0005",
                Message = $"New Fax Number Invalid: {originalFax}"
            };

            var actual = await _sut.ValidateChangeFaxNumberRequest(standardRequest, originalFax);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateResendFaxNumberRequest_WithAValidStandardRequest_ShouldReturnAMessageWithCode0000AndMessageSuccessful()
        {
            var standardRequest = GetValidStandardRequest();

            var originalFax = "1-888-963-9623";

            var expected = new ValidationMessage()
            {
                Code = "0000",
                Message = "Successful Validation"
            };

            var actual = await _sut.ValidateResendFaxNumberRequest(standardRequest, originalFax);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("    ")]
        public async Task
            ValidateResendFaxNumberRequest_WithAMissingFaxNumber_ShouldReturnAMessageWithCode0000AndMessageSuccessful(string phoneNumber)
        {
            var standardRequest = GetValidStandardRequest();
            standardRequest.CorrespondenceData.PhoneNumber = phoneNumber;

            var originalFax = "";

            var expected = new ValidationMessage()
            {
                Code = "0000",
                Message = "Successful Validation"
            };

            var actual = await _sut.ValidateResendFaxNumberRequest(standardRequest, originalFax);

            actual.Should().BeEquivalentTo(expected);
        }

        [Theory]
        [InlineData("1-236-962-312")]
        [InlineData("123")]
        [InlineData("1(236)-962-312")]
        [InlineData("a(012)-345-6789")]
        [InlineData("+(012)-345-6789")]
        public async Task
            ValidateResendFaxNumberRequest_WithInvalidPhoneNumber_ShouldReturnAMessageWithCode0005AndMessageError(string phoneNumber)
        {
            var standardRequest = GetValidStandardRequest();
            standardRequest.CorrespondenceData.PhoneNumber = phoneNumber.CleanFaxNumber();

            var originalFax = phoneNumber;

            var expected = new ValidationMessage()
            {
                Code = "0005",
                Message = $"New Fax Number Invalid: {originalFax}"
            };

            var actual = await _sut.ValidateChangeFaxNumberRequest(standardRequest, originalFax);

            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAInvalidCardDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.CardData.CardType = new String('A', 51);
            standardRequest.CardData.CardNumber = new String('A', 17);

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: cardType, cardNumber, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidCardDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.CardData = GetInvalidCardDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: cardType, cardNumber, cardCvv2, cardExpiration, loadTransId, loadAmount, loadFee, payeeName, cardholderName, cardholderAddress, cardPostalCode, unloadCode, unloadDesc, vcRef, displayCVV2, maskPan, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidCheckDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.CheckData = GetInvalidCheckDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: checkNumber, posPayNumber, switchNumber, chkNum1, chkNum2, checkDate, address1, address2, address3, city, stateOrProvince, zip, county, region, country, memo, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidClaimLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.Claim = GetInvalidClaimDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: userKey, userField1, userField2, userField3, currencyType, amount, claimDeductible, claimDate, claimOdometer, claimDescription, requesterId, requesterName, repairOrderId, claimNotes, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidCommonDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.CommonData = GetInvalidCommonDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: transNumber, seClaimID, tpaClaimID, programID, reasonCode, reasonDesc, responseCode, responseDesc, successCode, successDesc, user, passWord, timeStamp, token, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidCorrespondenceDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.CorrespondenceData = GetInvalidCorrespondenceDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: attachmentLocation, documentID, email, faxCode, faxStat, phoneNumber, type, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidCoverItemDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.CoveredItem = GetInvalidCoverItemDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: itemType, itemId, year, manufacturer, model, bookStateOrProvince, postalCode, planCode, planDescription, deductible, newUsed, beginDate, expireDate, odometerType, beginOdometer, expireOdometer, ownerLastName, ownerFirstName, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }


        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidMerchantDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.Merchant = GetInvalidMerchantDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: payeeCode, payeeName, contactPerson, postalCode, telephone, fax, emailAddress, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidPaymentDataLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.Payment = GetInvalidPaymentDataLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: accountingCode, accountingDesc, accountNumber, action, availableBalance, billCode, client, currencyCode, currentBalance, free, futureUse, id, loadAmount, panNumber, requestedAmount, routingNumber, type, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        [Fact]
        public async Task
            ValidateStandardRequest_WithAllInvalidSwitchTransactionLengths_ShouldReturnAMessageWithCode0990AndErrorMessageWithListOfFieldsHavingError()
        {
            var standardRequest = GetValidStandardRequest();

            standardRequest.SwitchTransaction = GetInvalidSwitchTransactionLengths();

            var expected = new ValidationMessage()
            {
                Code = "0990",
                Message = "Fields too long: acquireID, authCode, availableBal, captureTS, currentBal, merchantID, olsLogID, stan, switch, terminalID, transactionTS, "
            };

            var actual = await _sut.ValidateStandardRequest(standardRequest);


            actual.Should().BeEquivalentTo(expected);
        }

        public CardData GetInvalidCardDataLengths()
        {
            return new CardData()
            {
                CardType = new String('A', 1000),
                CardNumber = new String('A', 1000),
                CardCvv2 = new String('A', 1000),
                CardExpiration = new String('A', 1000),
                LoadTransId = new String('A', 1000),
                LoadAmount = new String('A', 1000),
                LoadFee = new String('A', 1000),
                PayeeName = new String('A', 1000),
                CardholderName = new String('A', 1000),
                CardholderAddress = new String('A', 1000),
                CardPostalCode = new String('A', 1000),
                UnloadCode = new String('A', 1000),
                UnloadDesc = new String('A', 1000),
                VcRef = new String('A', 1000),
                DisplayCVV2 = new String('A', 1000),
                MaskPan = new String('A', 1000)
            };
        }

        public CheckData GetInvalidCheckDataLengths()
        {
            return new CheckData()
            {
                CheckNumber = new String('A', 1000),
                PosPayNumber = new String('A', 1000),
                SwitchNumber = new String('A', 1000),
                ChkNum1 = new String('A', 1000),
                ChkNum2 = new String('A', 1000),
                CheckDate = new String('A', 1000),
                Address1 = new String('A', 1000),
                Address2 = new String('A', 1000),
                Address3 = new String('A', 1000),
                City = new String('A', 1000),
                StateOrProvince = new String('A', 1000),
                Zip = new String('A', 1000),
                County = new String('A', 1000),
                Region = new String('A', 1000),
                Country = new String('A', 1000),
                Memo = new String('A', 1000)
            };
        }

        public ClaimData GetInvalidClaimDataLengths()
        {
            return new ClaimData()
            {
                UserKey = new String('A', 1000),
                UserField1 = new String('A', 1000),
                UserField2 = new String('A', 1000),
                UserField3 = new String('A', 1000),
                CurrencyType = new String('A', 1000),
                Amount = new String('A', 1000),
                ClaimDeductible = new String('A', 1000),
                ClaimDate = new String('A', 1000),
                ClaimOdometer = new String('A', 1000),
                ClaimDescription = new String('A', 1000),
                RequesterId = new String('A', 1000),
                RequesterName = new String('A', 1000),
                RepairOrderId = new String('A', 1000),
                ClaimNotes = new String('A', 1000)
            };
        }

        public CommonData GetInvalidCommonDataLengths()
        {
            return new CommonData()
            {
                TransNumber = new String('A', 1000),
                SeClaimID = new String('A', 1000),
                TpaClaimID = new String('A', 1000),
                ProgramID = new String('A', 1000),
                ReasonCode = new String('A', 1000),
                ReasonDesc = new String('A', 1000),
                ResponseCode = new String('A', 1000),
                ResponseDesc = new String('A', 1000),
                SuccessCode = new String('A', 1000),
                SuccessDesc = new String('A', 1000),
                User = new String('A', 1000),
                PassWord = new String('A', 1000),
                TimeStamp = new String('A', 1000),
                Token = new String('A', 1000)
            };
        }

        public CorrespondenceData GetInvalidCorrespondenceDataLengths()
        {
            return new CorrespondenceData()
            {
                AttachmentLocation = new String('A', 1000),
                DocumentID = new String('A', 1000),
                Email = new String('A', 1000),
                FaxCode = new String('A', 1000),
                FaxStat = new String('A', 1000),
                PhoneNumber = new String('A', 1000),
                Type = new String('A', 1000)
            };
        }

        public CoveredItemData GetInvalidCoverItemDataLengths()
        {
            return new CoveredItemData()
            {
                ItemType = new String('A', 1000),
                ItemId = new String('A', 1000),
                Year = new String('A', 1000),
                Manufacturer = new String('A', 1000),
                Model = new String('A', 1000),
                BookStateOrProvince = new String('A', 1000),
                PostalCode = new String('A', 1000),
                PlanCode = new String('A', 1000),
                PlanDescription = new String('A', 1000),
                Deductible = new String('A', 1000),
                NewUsed = new String('A', 1000),
                BeginDate = new String('A', 1000),
                ExpireDate = new String('A', 1000),
                OdometerType = new String('A', 1000),
                BeginOdometer = new String('A', 1000),
                ExpireOdometer = new String('A', 1000),
                OwnerLastName = new String('A', 1000),
                OwnerFirstName = new String('A', 1000)
            };
        }

        public MerchantData GetInvalidMerchantDataLengths()
        {
            return new MerchantData()
            {
                PayeeCode = new String('A', 1000),
                PayeeName = new String('A', 1000),
                ContactPerson = new String('A', 1000),
                PostalCode = new String('A', 1000),
                Telephone = new String('A', 1000),
                Fax = new String('A', 1000),
                EmailAddress = new String('A', 1000)
            };
        }

        public PaymentData GetInvalidPaymentDataLengths()
        {
            return new PaymentData()
            {
                AccountingCode = new String('A', 1000),
                AccountingDesc = new String('A', 1000),
                AccountNumber = new String('A', 1000),
                Action = new String('A', 1000),
                AvailableBalance = new String('A', 1000),
                BillCode = new String('A', 1000),
                Client = new String('A', 1000),
                CurrencyCode = new String('A', 1000),
                CurrentBalance = new String('A', 1000),
                Free = new String('A', 1000),
                FutureUse = new String('A', 1000),
                Id = new String('A', 1000),
                LoadAmount = new String('A', 1000),
                PanNumber = new String('A', 1000),
                RequestedAmount = new String('A', 1000),
                RoutingNumber = new String('A', 1000),
                Type = new String('A', 1000)
            };
        }

        public SwitchTransactionData GetInvalidSwitchTransactionLengths()
        {
            return new SwitchTransactionData()
            {
                AcquireID = new String('A', 1000),
                AuthCode = new String('A', 1000),
                AvailableBal = new String('A', 1000),
                CaptureTS = new String('A', 1000),
                CurrentBal = new String('A', 1000),
                MerchantID = new String('A', 1000),
                OlsLogID = new String('A', 1000),
                Stan = new String('A', 1000),
                Switch = new String('A', 1000),
                TerminalID = new String('A', 1000),
                TransactionTS = new String('A', 1000)
            };
        }

        private StandardRequest GetValidStandardRequest()
        {
            return new StandardRequest()
            {
                CommonData = new CommonData()
                {
                    TransNumber = "TransNumber",
                    SeClaimID = "SeClaimID",
                    TpaClaimID = "TpaClaimID",
                    ProgramID = "ProgramID",
                    ReasonCode = "0000",
                    ReasonDesc = "ReasonDesc",
                    ResponseCode = "0001",
                    ResponseDesc = "ResponseDesc",
                    SuccessCode = "0002",
                    SuccessDesc = "SuccessDesc",
                    User = "User",
                    PassWord = "PassWord",
                    TimeStamp = "TimeStamp",
                    Token = "Token"
                },
                CardData = new CardData()
                {
                    CardType = "CardType",
                    CardNumber = "CardNumber",
                    CardCvv2 = "9638",
                    CardExpiration = "9639",
                    LoadTransId = "LoadTransId",
                    LoadAmount = "LoadAmount",
                    LoadFee = "LoadFe",
                    PayeeName = "PayeeName",
                    CardholderName = "CardholderName",
                    CardholderAddress = "CardholderAddress",
                    CardPostalCode = "CardPostalCode",
                    UnloadCode = "UnloadCode",
                    UnloadDesc = "UnloadDesc",
                    VcRef = "VcRef",
                    DisplayCVV2 = "1",
                    MaskPan = "M",
                },
                CheckData = new CheckData()
                {
                    CheckNumber = "CheckNumber",
                    PosPayNumber = "PosPayNumber",
                    SwitchNumber = "SwitchNumber",
                    ChkNum1 = "ChkNum1",
                    ChkNum2 = "ChkNum2",
                    CheckDate = "CheckDat",
                    Address1 = "Address1",
                    Address2 = "Address2",
                    Address3 = "Address3",
                    City = "City",
                    StateOrProvince = "StateOrProvince",
                    Zip = "Zip",
                    County = "County",
                    Region = "Region",
                    Country = "Country",
                    Memo = "Memo",
                },
                Claim = new ClaimData()
                {
                    UserKey = "UserKey",
                    UserField1 = "UserField1",
                    UserField2 = "UserField2",
                    UserField3 = "UserField3",
                    CurrencyType = "840",
                    Amount = "Amount",
                    ClaimDeductible = "ClaimDed",
                    ClaimDate = "ClaimDat",
                    ClaimOdometer = "ClaimO",
                    ClaimDescription = "ClaimDescription",
                    RequesterId = "RequesterId",
                    RequesterName = "RequesterName",
                    RepairOrderId = "RepairOrderId",
                    ClaimNotes = "ClaimNotes"
                },
                CorrespondenceData = new CorrespondenceData()
                {
                    AttachmentLocation = "AttachmentLocation",
                    DocumentID = "DocumentID",
                    Email = "Email",
                    FaxCode = "FaxCode",
                    FaxStat = "FaxStat",
                    PhoneNumber = "8889639623",
                    Type = "Type",
                },
                CoveredItem = new CoveredItemData()
                {
                    ItemType = "ItemType",
                    ItemId = "ItemId",
                    Year = "2018",
                    Manufacturer = "Manufacturer",
                    Model = "Model",
                    BookStateOrProvince = "TXT",
                    PostalCode = "PostalCode",
                    PlanCode = "PlanCode",
                    PlanDescription = "PlanDescription",
                    Deductible = "Deductib",
                    NewUsed = "N",
                    BeginDate = "BeginDat",
                    ExpireDate = "ExpireDa",
                    OdometerType = "Odom",
                    BeginOdometer = "BeginOd",
                    ExpireOdometer = "ExpireO",
                    OwnerLastName = "OwnerLastName",
                    OwnerFirstName = "OwnerFirstName",
                },
                Merchant = new MerchantData()
                {
                    PayeeCode = "PayeeCode",
                    PayeeName = "PayeeName",
                    ContactPerson = "ContactPerson",
                    PostalCode = "PostalCode",
                    Telephone = "Telephone",
                    Fax = "Fax",
                    EmailAddress = "EmailAddress",
                },
                Payment = new PaymentData()
                {
                    AccountingCode = "A",
                    AccountingDesc = "Account",
                    AccountNumber = "AccountNumber",
                    Action = "Action",
                    AvailableBalance = "AvailableBalance",
                    BillCode = "BillCode",
                    Client = "Client",
                    CurrencyCode = "CurrencyCo",
                    CurrentBalance = "CurrentBalance",
                    Free = "Free",
                    FutureUse = "Ft",
                    Id = "Id",
                    LoadAmount = "LoadAmount",
                    PanNumber = "PanNumber",
                    RequestedAmount = "RequestedAmount",
                    RoutingNumber = "RoutingNumber",
                    Type = "Type",
                },
                SwitchTransaction = new SwitchTransactionData()
                {
                    AcquireID = "AcquireID",
                    AuthCode = "AuthCode",
                    AvailableBal = "AvailableBal",
                    CaptureTS = "CaptureTS",
                    CurrentBal = "CurrentBal",
                    MerchantID = "MerchantID",
                    OlsLogID = "OlsLogID",
                    Stan = "Stan",
                    Switch = "Switch",
                    TerminalID = "TerminalID",
                    TransactionTS = "TransactionTS",
                }
            };
        }

    }
}
