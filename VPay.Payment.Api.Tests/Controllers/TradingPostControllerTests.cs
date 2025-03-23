using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Moq;
using VPay.Data.Db2.Abstractions.TradingPostWs;
using VPay.Payment.Api.Controllers;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Tests
{
    public class TradingPostControllerTests
    {
        private readonly TradingPostController _controller;
        private readonly Mock<ITradingPostService> _tradingPostServiceMock;
        private readonly Mock<IHttpContextAccessor> _accessorMock;
        private readonly Mock<IFileProvider> _fileProviderMock;
        private readonly Mock<ILogger<LegacyController>> _loggerMock;
        private readonly PaymentConfig _config;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;

        public TradingPostControllerTests()
        {
            _tradingPostServiceMock = new Mock<ITradingPostService>();
            _accessorMock = new Mock<IHttpContextAccessor>();
            _fileProviderMock = new Mock<IFileProvider>();
            _loggerMock = new Mock<ILogger<LegacyController>>();
            _config = new PaymentConfig { TradingPostIp = "127.0.0.1" };
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(f => f.PhysicalPath).Returns("SEcureCardServices.xml");
            _fileProviderMock.Setup(f => f.GetFileInfo(It.IsAny<string>())).Returns(fileInfoMock.Object);

            _controller = new TradingPostController(
                _tradingPostServiceMock.Object,
                Mock.Of<IWebHostEnvironment>(env => env.WebRootFileProvider == _fileProviderMock.Object),
                _accessorMock.Object,
                _config,
                _loggerMock.Object);
        }

        //[Fact]
        //public void GetWsdl_ShouldReturnWsdlContent()
        //{
        //    // Arrange
        //    var httpReq = new Mock<HttpRequest>();
        //    httpReq.Setup(r => r.Scheme).Returns("http");
        //    httpReq.Setup(r => r.Host).Returns(new HostString("test"));
        //    httpReq.Setup(r => r.Path).Returns("/test");
        //    var context = new Mock<HttpContext>();
        //    context.Setup(c => c.Request).Returns(httpReq.Object);
        //    _accessorMock.Setup(a => a.HttpContext).Returns(context.Object);
        //    var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

        //    mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context.Object);
        //    // Act
        //    var result = _controller.GetWsdl(null);

        //    // Assert
        //    result.Should().BeOfType<FileContentResult>();
        //    var fileResult = result as FileContentResult;
        //    fileResult.ContentType.Should().Be("text/xml");
        //}

        [Fact]
        public void GetWsdl_ReturnsXmlFile()
        {
            // Arrange
            var xmlContent = @"
                <definitions xmlns:soap='http://schemas.xmlsoap.org/wsdl/soap/' xmlns:soap12='http://schemas.xmlsoap.org/wsdl/soap12/'>
                    <service>
                        <port>
                            <soap:address location='http://oldurl'/>
                            <soap12:address location='http://oldurl'/>
                        </port>
                    </service>
                </definitions>";
            File.WriteAllText("SEcureCardServices.xml", xmlContent);

            var httpReq = new Mock<HttpRequest>();
            httpReq.Setup(r => r.Scheme).Returns("http");
            httpReq.Setup(r => r.Host).Returns(new HostString("test"));
            httpReq.Setup(r => r.Path).Returns("/test");
            var context = new Mock<HttpContext>();
            context.Setup(c => c.Request).Returns(httpReq.Object);
            _accessorMock.Setup(a => a.HttpContext).Returns(context.Object);
            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context.Object);

            // Act
            var result = _controller.GetWsdl(null) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/xml", result.ContentType);

            //var resultXml = Encoding.UTF8.GetString(result.FileContents);
            //var doc = XDocument.Parse(resultXml);
            //var nsSoap = "http://schemas.xmlsoap.org/wsdl/soap/";
            //var nsSoap12 = "http://schemas.xmlsoap.org/wsdl/soap12/";

            //var soapAddress = doc.Descendants(XName.Get("address", nsSoap)).FirstOrDefault();
            //var soap12Address = doc.Descendants(XName.Get("address", nsSoap12)).FirstOrDefault();

            //Assert.NotNull(soapAddress);
            //Assert.NotNull(soap12Address);
            //Assert.Equal("http://localhost/api/tradingpost/wsdl", soapAddress.Attribute("location").Value);
            //Assert.Equal("http://localhost/api/tradingpost/wsdl", soap12Address.Attribute("location").Value);
        }

        [Fact]
        public async Task LoadCard_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new TradingPostRequest
            {
                Envelope = new TradingPostEnvelope
                {
                    Body = new TradingPostBody
                    {
                        LoadCard = new TradingPostLoadRequest
                        {
                            AuthenticationValues = new AuthenticationValues { Id = "user", PassPhrase = "pass" },
                            LoadRequest = new TradingPostData.LoadRequest()
                        }
                    }
                }
            };

            var expectedResponse = new TradingPostData.LoadResult();
            _tradingPostServiceMock.Setup(s => s.LoadCard(It.IsAny<TradingPostData.AuthenticationValuesAndIp>(), It.IsAny<TradingPostData.LoadRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.LoadCard(request);

            // Assert
            result.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task RetrieveCard_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new TradingPostRequest
            {
                Envelope = new TradingPostEnvelope
                {
                    Body = new TradingPostBody
                    {
                        RetrieveCard = new TradingPostRetrieveRequest
                        {
                            AuthenticationValues = new AuthenticationValues { Id = "user", PassPhrase = "pass" },
                            RetrieveRequest = new TradingPostData.RetrieveRequest()
                        }
                    }
                }
            };

            var expectedResponse = new TradingPostData.RetrieveResult();
            _tradingPostServiceMock.Setup(s => s.RetrieveCard(It.IsAny<TradingPostData.AuthenticationValuesAndIp>(), It.IsAny<TradingPostData.RetrieveRequest>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.RetrieveCard(request);

            // Assert
            result.Should().Be(expectedResponse);
        }

        [Fact]
        public async Task CardNotificationRelease_ShouldReturnExpectedResponse()
        {
            // Arrange
            var request = new TradingPostRequest
            {
                Envelope = new TradingPostEnvelope
                {
                    Body = new TradingPostBody
                    {
                        CardNotificationRelease = new TradingPostNotificationRequest
                        {
                            AuthenticationValues = new AuthenticationValues { Id = "user", PassPhrase = "pass" },
                            NotificationRequest = new TradingPostData.ReleaseNotification()
                        }
                    }
                }
            };

            var expectedResponse = new TradingPostData.NotificationResult();
            _tradingPostServiceMock.Setup(s => s.CardNotificationRelease(It.IsAny<TradingPostData.AuthenticationValuesAndIp>(), It.IsAny<TradingPostData.ReleaseNotification>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CardNotificationRelease(request);

            // Assert
            result.Should().Be(expectedResponse);
        }
    }
}
