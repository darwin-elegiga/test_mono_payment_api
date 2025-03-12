using System.Collections.Generic;
using System.Text;
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
            var expected = "CommonData [ TransNumber=123, User=testUser, Token=**** ]\r\nReasonCodes [ \r\nReasonCode=001, ReasonDesc=Description, ActionDesc=Action\r\n] ";
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

        // Additional tests for other methods can be added similarly
    }
}
