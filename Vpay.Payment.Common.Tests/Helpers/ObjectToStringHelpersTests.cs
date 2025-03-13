//using System.Collections.Generic;
//using System.Text;
//using VPay.Data.Db2.Abstractions.TradingPostWs;
//using VPay.Data.Db2.Abstractions.TransactionWs;
//using VPay.Payment.Common;
//using Xunit;
//namespace VPay.Payment.Common.Tests
//{
//    public class ObjectToStringHelpersTests
//    {
//        [Fact]
//        public void ToDisplayString_StandardRequest_ShouldReturnExpectedString()
//        {
//            // Arrange
//            var request = new StandardRequest
//            {
//                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
//                Source = 'P'
//            };

//            // Act
//            var result = request.ToDisplayString();

//            // Assert
//            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nClaim [ CurrencyType=840 ]\r\nSource=P";
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void ToDisplayString_StandardResponse_ShouldReturnExpectedString()
//        {
//            // Arrange
//            var response = new StandardResponse
//            {
//                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" }
//            };

//            // Act
//            var result = response.ToDisplayString();

//            // Assert
//            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nClaim [ CurrencyType=840 ]\r\n";
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void ToDisplayString_ReasonCodeRequest_ShouldReturnExpectedString()
//        {
//            // Arrange
//            var request = new ReasonCodeRequest
//            {
//                TransNumber = "123",
//                User = "testUser",
//                Token = "token"
//            };

//            // Act
//            var result = request.ToDisplayString();

//            // Assert
//            var expected = "ReasonCodeRequest [ TransNumber=123, User=testUser, Token=**** ]\r\n";
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void ToDisplayString_ReasonCodeResponse_ShouldReturnExpectedString()
//        {
//            // Arrange
//            var response = new ReasonCodeResponse
//            {
//                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
//                ReasonCodeList = new List<ReasonCodeType>
//            {
//                new ReasonCodeType { ReasonCode = "001", ReasonDesc = "Description", ReasonAdsc = "Action" }
//            }
//            };

//            // Act
//            var result = response.ToDisplayString();

//            // Assert
//            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nReasonCodes [ \r\nReasonCode=001, ReasonDesc=Description, ActionDesc=Action\n] ";
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void ToDisplayString_TransactionDetailResponse_ShouldReturnExpectedString()
//        {
//            // Arrange
//            var response = new TransactionDetailResponse
//            {
//                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
//                HeaderData = new HeaderData { TransNumber = 123 },
//                PayTypeDetail = new PayTypeDetail { CardNumber = "1234567890123456" },
//                DetailList = new List<Detail> { new Detail { TranId = 1, LoadTran = 52 } },
//                CorrespondenceList = new List<CorespDtl> { new CorespDtl { Direction = "Test", Type = "Test1" } }
//            };

//            // Act
//            var result = response.ToDisplayString();

//            // Assert
//            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nHeaderData [ TransNumber=123 ]\r\nPayTypeDetail [ CardNumber=1234*********56 ]\r\nDetailList [ \r\nDetail [ TranId=1, LoadTran=52 ]\r\n] \r\nCorrespondenceList [ \r\nCorrespondence [ Direction=Test, Type=Test1, RequestDate=0, StatusDate=0, DmRecId=0 ]\r\n] \r\n";
//            Assert.Equal(expected, result);
//        }

//        [Fact]
//        public void ToDisplayString_TradingPostData_CardInformation_ShouldReturnExpectedString()
//        {
//            // Arrange
//            var response = new TradingPostData.CardInformation
//            {
//                CardholderName = "Test",
//                CardExpiration = "12/2020",
//                CardholderPostalCode = "Test",
//                CardType = "Test",
//                CardholderAddress = "Test",
//                CardNumber = "134567",
//                CardSecurityValue = "Test",
//                LoadAmount = 1,
//                LoadFee = 1,
//                LoadTransactionId = 1,
//                PayeeName = "Test"
//            };

//            // Act
//            var result = response.ToDisplayString();

//            // Assert
//            var expected = "CardInformation [ CardType=Test, CardNumber=****, CardCvv2=***, CardExpiration=12/2020, LoadTransactionId=1, LoadAmount=$1.00, LoadFee=$1.00, PayeeName=Test, CardholderName=Test, CardholderAddress=Test, CardholderPostalCode=Test ]\r\n";
//            Assert.Equal(expected, result);
//        }

//New Code
using System.Collections.Generic;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Common.Tests
{
    public class ObjectToStringHelpersTests
    {
        [Fact]
        public void ToDisplayString_StandardRequest_ShouldReturnExpectedString()
        {
            // Arrange
            var request = new StandardRequest
            {
                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
                Source = 'P'
            };

            // Act
            var result = request.ToDisplayString();

            // Assert
            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nClaim [ CurrencyType=840 ]\r\nSource=P";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_StandardResponse_ShouldReturnExpectedString()
        {
            // Arrange
            var response = new StandardResponse
            {
                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" }
            };

            // Act
            var result = response.ToDisplayString();

            // Assert
            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nClaim [ CurrencyType=840 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_ReasonCodeRequest_ShouldReturnExpectedString()
        {
            // Arrange
            var request = new ReasonCodeRequest
            {
                TransNumber = "123",
                User = "testUser",
                Token = "token"
            };

            // Act
            var result = request.ToDisplayString();

            // Assert
            var expected = "ReasonCodeRequest [ TransNumber=123, User=testUser, Token=**** ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_ReasonCodeResponse_ShouldReturnExpectedString()
        {
            // Arrange
            var response = new ReasonCodeResponse
            {
                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
                ReasonCodeList = new List<ReasonCodeType>
                {
                    new ReasonCodeType { ReasonCode = "001", ReasonDesc = "Description", ReasonAdsc = "Action" }
                }
            };

            // Act
            var result = response.ToDisplayString();

            // Assert
            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nReasonCodes [ \r\nReasonCode=001, ReasonDesc=Description, ActionDesc=Action\n] ";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TransactionDetailResponse_ShouldReturnExpectedString()
        {
            // Arrange
            var response = new TransactionDetailResponse
            {
                CommonData = new CommonData { TransNumber = "123", User = "testUser", Token = "token" },
                HeaderData = new HeaderData { TransNumber = 123 },
                PayTypeDetail = new PayTypeDetail { CardNumber = "1234567890123456" },
                DetailList = new List<Detail> { new Detail { TranId = 1, LoadTran = 52 } },
                CorrespondenceList = new List<CorespDtl> { new CorespDtl { Direction = "Test", Type = "Test1" } }
            };

            // Act
            var result = response.ToDisplayString();

            // Assert
            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nHeaderData [ TransNumber=123 ]\r\nPayTypeDetail [ CardNumber=1234*********56 ]\r\nDetailList [ \r\nDetail [ TranId=1, LoadTran=52 ]\r\n] \r\nCorrespondenceList [ \r\nCorrespondence [ Direction=Test, Type=Test1, RequestDate=0, StatusDate=0, DmRecId=0 ]\r\n] \r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_CommonData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new CommonData
            {
                TransNumber = "123",
                User = "testUser",
                Token = "token"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_CardData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new CardData
            {
                CardType = "Visa",
                CardNumber = "1234567890123456",
                CardCvv2 = "123",
                CardExpiration = "12/2023"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "CardData [ CardType=Visa, CardNumber=1234*********56, CardCvv2=1***, CardExpiration=12/2023 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_CheckData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new CheckData
            {
                CheckDate = "01/01/2023",
                CheckNumber = "123456"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "CheckData [ CheckDate=01/01/2023, CheckNumber=123456 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_ClaimData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new ClaimData
            {
                UserKey = "userKey",
                UserField1 = "field1"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Claim [ UserKey=userKey, UserField1=field1, CurrencyType=840 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_CorrespondenceData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new CorrespondenceData
            {
                AttachmentLocation = "location",
                DocumentID = "docId"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "CorrespondenceData [ AttachmentLocation=location, DocumentID=docId ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_CoveredItemData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new CoveredItemData
            {
                ItemType = "type",
                ItemId = "id"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Covered Item [ ItemType=type, ItemId=id ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_MerchantData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new MerchantData
            {
                PayeeCode = "code",
                PayeeName = "name"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Merchant [ PayeeCode=code, PayeeName=name ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_PaymentData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new PaymentData
            {
                AccountingCode = "code",
                AccountingDesc = "desc"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Payment [ AccountingCode=code, AccountingDesc=desc ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_SwitchTransactionData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new SwitchTransactionData
            {
                AcquireID = "id",
                AuthCode = "auth"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "SwitchTransaction [ AcquireID=id, AuthCode=auth ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_ReasonCodeType_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new ReasonCodeType
            {
                ReasonCode = "001",
                ReasonDesc = "desc",
                ReasonAdsc = "action"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "ReasonCode=001, ReasonDesc=desc, ActionDesc=action";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_HeaderData_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new HeaderData
            {
                TransNumber = 123,
                Client = "client"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "HeaderData [ TransNumber=123, Client=client ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_PayTypeDetail_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new PayTypeDetail
            {
                Association = "assoc",
                Bank = "bank"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "PayTypeDetail [ Association=assoc, Bank=bank ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_Detail_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new Detail
            {
                TranId = 1,
                LoadTran = 52
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Detail [ TranId=1, LoadTran=52 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_CorespDtl_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new CorespDtl
            {
                Direction = "dir",
                Type = "type"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Correspondence [ Direction=dir, Type=type, RequestDate=0, StatusDate=0, DmRecId=0 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_LoadRequest_ShouldReturnExpectedString()
        {
            // Arrange
            var request = new TradingPostData.LoadRequest
            {
                CardHolder = new TradingPostData.Cardholder { Client = "client" },
                Merchant = new TradingPostData.Merchant { PayeeCode = "code" }
            };

            // Act
            var result = request.ToDisplayString();

            // Assert
            var expected = "Cardholder [ Client=client ]\r\nMerchant [ PayeeCode=code ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_LoadResult_ShouldReturnExpectedString()
        {
            // Arrange
            var result = new TradingPostData.LoadResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation { ResponseCode = "code" },
                CardInformation = new TradingPostData.CardInformation { CardType = "type" }
            };

            // Act
            var displayString = result.ToDisplayString();

            // Assert
            var expected = "TransactionInformation [ ResponseCode=code ]\r\nCardInformation [ CardType=type ]\r\n";
            Assert.Equal(expected, displayString);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_RetrieveRequest_ShouldReturnExpectedString()
        {
            // Arrange
            var request = new TradingPostData.RetrieveRequest
            {
                CardInformation = new TradingPostData.CardInformation { CardType = "type" },
                CardHolder = new TradingPostData.Cardholder { Client = "client" }
            };

            // Act
            var result = request.ToDisplayString();

            // Assert
            var expected = "CardInformation [ CardType=type ]\r\nCardholder [ Client=client ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_RetrieveResult_ShouldReturnExpectedString()
        {
            // Arrange
            var result = new TradingPostData.RetrieveResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation { ResponseCode = "code" },
                CardInformation = new TradingPostData.CardInformation { CardType = "type" }
            };

            // Act
            var displayString = result.ToDisplayString();

            // Assert
            var expected = "TransactionInformation [ ResponseCode=code ]\r\nCardInformation [ CardType=type ]\r\n";
            Assert.Equal(expected, displayString);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_ReleaseNotification_ShouldReturnExpectedString()
        {
            // Arrange
            var request = new TradingPostData.ReleaseNotification
            {
                CardInformation = new TradingPostData.CardInformation { CardType = "type" },
                CardHolder = new TradingPostData.Cardholder { Client = "client" }
            };

            // Act
            var result = request.ToDisplayString();

            // Assert
            var expected = "CardInformation [ CardType=type ]\r\nCardholder [ Client=client ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_NotificationResult_ShouldReturnExpectedString()
        {
            // Arrange
            var result = new TradingPostData.NotificationResult
            {
                TransactionInformation = new TradingPostData.TransactionInformation { ResponseCode = "code" },
                CardInformation = new TradingPostData.CardInformation { CardType = "type" }
            };

            // Act
            var displayString = result.ToDisplayString();

            // Assert
            var expected = "TransactionInformation [ ResponseCode=code ]\r\nCardInformation [ CardType=type ]\r\n";
            Assert.Equal(expected, displayString);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_Cardholder_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.Cardholder
            {
                Client = "client",
                BillingCode = "code"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Cardholder [ Client=client, BillingCode=code ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_Merchant_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.Merchant
            {
                PayeeCode = "code",
                PayeeName = "name"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Merchant [ PayeeCode=code, PayeeName=name ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_CoveredItem_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.CoveredItem
            {
                ItemType = "type",
                ItemId = "id"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Covered Item [ ItemType=type, ItemId=id ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_Claim_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.Claim
            {
                UserKey = "userKey",
                UserField1 = "field1"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Claim [ UserKey=userKey, UserField1=field1 ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_Correspondence_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.Correspondence
            {
                SendFaxCode = "faxCode",
                AttachmentLocation = "location"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "Correspondence [ SendFaxCode=faxCode, AttachmentLocation=location ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_TransactionInformation_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.TransactionInformation
            {
                ResponseCode = "code",
                ResponseDescription = "desc"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "TransactionInformation [ ResponseCode=code, ResponseDescription=desc ]\r\n";
            Assert.Equal(expected, result);
        }

        [Fact]
        public void ToDisplayString_TradingPostData_CardInformation_ShouldReturnExpectedString()
        {
            // Arrange
            var entity = new TradingPostData.CardInformation
            {
                CardType = "Visa",
                CardNumber = "1234567890123456",
                CardSecurityValue = "123",
                CardExpiration = "12/2023"
            };

            // Act
            var result = entity.ToDisplayString();

            // Assert
            var expected = "CardInformation [ CardType=Visa, CardNumber=1234*********56, CardCvv2=***, CardExpiration=12/2023 ]\r\n";
            Assert.Equal(expected, result);
        }
    }
}

// Additional tests for other methods can be added similarly
//}
//}
