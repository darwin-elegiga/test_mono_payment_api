using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Controllers;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Api.Tests.Controllers
{
    public class LegacyControllerTests
    {
        private readonly Mock<IWebHostEnvironment> _webHostEnvironmentMock;
        private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock;
        private readonly Mock<ILegacyTransactionService> _transactionServiceMock;
        private readonly Mock<ILogger<LegacyController>> _loggerMock;
        private readonly LegacyController _controller;
        private readonly LegacyController _sut;
        private readonly NullLogger<LegacyController> _logger;
        private readonly Mock<ILegacyTransactionService> _transactionService;
        private readonly Mock<IFileProvider> _fileProvider;

        public LegacyControllerTests()
        {
            _webHostEnvironmentMock = new Mock<IWebHostEnvironment>();
            _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
            _transactionServiceMock = new Mock<ILegacyTransactionService>();
            _loggerMock = new Mock<ILogger<LegacyController>>();

            _webHostEnvironmentMock.Setup(x => x.WebRootFileProvider).Returns(new PhysicalFileProvider(Directory.GetCurrentDirectory()));

            _controller = new LegacyController(_webHostEnvironmentMock.Object, _httpContextAccessorMock.Object, _transactionServiceMock.Object, _loggerMock.Object);

            _logger = new NullLogger<LegacyController>();
            _transactionService = new Mock<ILegacyTransactionService>();
            _fileProvider = new Mock<IFileProvider>();
            var hostingEnv = new Mock<IWebHostEnvironment>();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var claims = new[] { new Claim(ClaimTypes.Name, "UserName"), new Claim(ClaimTypes.NameIdentifier, "Token"), new Claim(ClaimTypes.System, "S") };
            var identity = new ClaimsIdentity(claims, "Basic");
            context.User = new ClaimsPrincipal(identity);

            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            hostingEnv.Setup(_ => _.WebRootFileProvider).Returns(_fileProvider.Object);

            _sut = new LegacyController(hostingEnv.Object, mockHttpContextAccessor.Object, _transactionService.Object, _logger);

            var request = context.Request;

            // Set up the request properties as needed
            request.Scheme = "https";
            request.Host = new HostString("localhost", 5001);
            request.Path = "/api/myaction";
            request.QueryString = new QueryString("?param1=value1&param2=value2");

            _sut.ControllerContext.HttpContext = context;
        }

        [Fact]
        public async Task GetTransactionDetails_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        GetTransactionDetails = new LegacyTransactionDetailRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new TransactionDetailRequestDto()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.GetTransactionDetails(It.IsAny<TransactionDetailRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.GetTransactionDetails(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with GetTransactionDetails");
        }

        [Fact]
        public async Task GetPanNumber_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        GetPanNumber = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.GetPanNumber(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.GetPanNumber(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with GetPanNumber");
        }

        [Fact]
        public async Task OpenPreAuth_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        OpenPreAuth = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.OpenPreAuth(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.OpenPreAuth(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with OpenPreAuth");
        }

        [Fact]
        public async Task LoadPan_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        LoadPan = new LegacyLoadPanRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest(),
                            CustomData = new LegacyCustomData()
                            {
                                ClientData = ""
                            }
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.LoadPan(It.IsAny<StandardRequest>(), "", CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.LoadPan(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with LoadPan");
        }

        [Fact]
        public async Task BalanceRequest_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        BalanceRequest = new LegacyStandardRequest
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.GetBalanceRequest(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.BalanceRequest(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with BalanceRequest");
        }

        [Fact]
        public async Task UnloadPan_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        UnloadPan = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.UnloadPan(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.UnloadPan(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with UnloadPan");
        }

        [Fact]
        public async Task StopPay_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        StopPay = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.StopPay(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.StopPay(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with StopPay");
        }

        [Fact]
        public async Task ResendFax_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        ResendFax = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.ResendFax(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.ResendFax(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with ResendFax");
        }

        [Fact]
        public void GetVer_ShouldReturnVersionInfo()
        {
            // Act
            var result = _sut.GetVer();

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task PostEcho_ShouldReturnStandardResponse()
        {
            // Arrange
            var echoRequest = new EchoRequest { Es = "Test" };

            // Act
            var result = await _sut.PostEcho(echoRequest);

            // Assert
            result.Should().NotBeNull();
            result.CommonData.ResponseDesc.Should().Contain("VPayWs");
            result.CommonData.SuccessCode.Should().Be("0");
            result.CommonData.ReasonCode.Should().Be("0");
        }

        [Fact]
        public async Task GetReasonCodes_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        GetReasonCodes = new LegacyReasonCodeRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new ReasonCodeRequestDto()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.GetReasonCodes(It.IsAny<ReasonCodeRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.GetReasonCodes(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with GetReasonCodes");
        }

        //New Code

        [Fact]
        public void GetWsdl_ShouldReturnWsdlFile_WhenUrlIsNotProvided()
        {
            // Arrange
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("http");
            request.Setup(x => x.Host).Returns(new HostString("localhost"));
            request.Setup(x => x.Path).Returns("/api/legacy/wsdl");

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(x => x.Request).Returns(request.Object);

            _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext.Object);

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(x => x.PhysicalPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "VPayWSService.xml"));

            var fileProviderMock = new Mock<IFileProvider>();
            fileProviderMock.Setup(x => x.GetFileInfo("VPayWSService.xml")).Returns(fileInfoMock.Object);

            _webHostEnvironmentMock.Setup(x => x.WebRootFileProvider).Returns(fileProviderMock.Object);

            // Act
            var result = _controller.GetWsdl(null) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/xml", result.ContentType);
            var content = Encoding.UTF8.GetString(result.FileContents);
            Assert.Contains("<definitions", content);
        }

        [Fact]
        public void GetWsdl_ShouldReturnWsdlFile_WhenUrlIsProvided()
        {
            // Arrange
            var url = "http://localhost/api/legacy/wsdl";

            var fileInfoMock = new Mock<IFileInfo>();
            fileInfoMock.Setup(x => x.PhysicalPath).Returns(Path.Combine(Directory.GetCurrentDirectory(), "VPayWSService.xml"));

            var fileProviderMock = new Mock<IFileProvider>();
            fileProviderMock.Setup(x => x.GetFileInfo("VPayWSService.xml")).Returns(fileInfoMock.Object);

            _webHostEnvironmentMock.Setup(x => x.WebRootFileProvider).Returns(fileProviderMock.Object);

            // Act
            var result = _controller.GetWsdl(url) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("text/xml", result.ContentType);
            var content = Encoding.UTF8.GetString(result.FileContents);
            Assert.Contains("<definitions", content);
        }


    }
}
