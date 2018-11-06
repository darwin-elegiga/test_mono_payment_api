using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Controllers;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;
using Xunit;

namespace VPay.Payment.Api.Tests
{
    public class LegacyControllerTests
    {
        private readonly LegacyController _sut;
        private readonly NullLogger<LegacyController> _logger;
        private readonly Mock<ILegacyTransactionService> _transactionService;
        private readonly Mock<IFileProvider> _fileProvider;

        public LegacyControllerTests()
        {
            _logger = new NullLogger<LegacyController>();
            _transactionService = new Mock<ILegacyTransactionService>();
            _fileProvider = new Mock<IFileProvider>();

            var hostingEnv = new Mock<IHostingEnvironment>();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            var context = new DefaultHttpContext();
            var claims = new[] { new Claim(ClaimTypes.Name, "UserName"), new Claim(ClaimTypes.NameIdentifier, "Token"), new Claim(ClaimTypes.System, "S") };
            var identity = new ClaimsIdentity(claims, "Basic");
            context.User = new ClaimsPrincipal(identity);

            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);

            hostingEnv.Setup(_ => _.WebRootFileProvider).Returns(_fileProvider.Object);

            _sut = new LegacyController(hostingEnv.Object, mockHttpContextAccessor.Object, _transactionService.Object, _logger);
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
        public async Task CancelFax_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        CancelFax = new LegacyLoadPanRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.CancelFax(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.CancelFax(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with CancelFax");
        }

        [Fact]
        public async Task HoldFax_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        HoldFax = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.HoldFax(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.HoldFax(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with HoldFax");
        }

        [Fact]
        public async Task ReleaseFax_WhenServiceThrowsException_ThenShouldReturnObjectWithErrorCode9997()
        {
            var setupObj = new LegacyRequest()
            {
                Envelope = new LegacyEnvelope()
                {
                    Body = new LegacyBody()
                    {
                        ReleaseFax = new LegacyStandardRequest()
                        {
                            AuthenticationValues = new AuthenticationValues(),
                            Request = new StandardRequest()
                        }
                    }
                }
            };

            _transactionService
                .Setup(_ => _.ReleaseFax(It.IsAny<StandardRequest>(), CancellationToken.None))
                .ThrowsAsync(new Exception("General Exception"));

            var result = await _sut.ReleaseFax(setupObj);

            result.CommonData.SuccessCode.Should().Be("9997");
            result.CommonData.SuccessDesc.Should().Be("Unexpected Error with ReleaseFax");
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

    }
}
