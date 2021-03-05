using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Api.Auth;
using VPay.Payment.Api.Dtos;
using VPay.Payment.Common;

namespace VPay.Payment.Api.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class LegacyController : ControllerBase
    {
        private readonly IHttpContextAccessor _accessor;
        private readonly ILegacyTransactionService _transactionService;
        private readonly IFileProvider _fileProvider;

        private readonly ILogger _logger;

        public LegacyController(IHostingEnvironment fileProvider, IHttpContextAccessor accessor, ILegacyTransactionService transactionService, ILogger<LegacyController> logger)
        {
            _fileProvider = fileProvider.WebRootFileProvider;
            _accessor = accessor;
            _transactionService = transactionService;
            _logger = logger;
        }

        [HttpGet("wsdl")]
        [Produces("text/xml")]
        [AllowAnonymous]
        public IActionResult GetWsdl([FromQuery] string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                var request = _accessor.HttpContext.Request;
                var location = new Uri($"{request.Scheme}://{request.Host}{request.Path}");

                url = location.AbsoluteUri;
            }
            else
            {
                var uri = new Uri(url);
                url = uri.GetLeftPart(UriPartial.Path);
            }
            var doc = XDocument.Load(_fileProvider.GetFileInfo("VPayWSService.xml").PhysicalPath);
            XNamespace nsSoap = "http://schemas.xmlsoap.org/wsdl/soap/";
            XNamespace nsXsd = "http://www.w3.org/2001/XMLSchema";

            var addressElement = doc.Descendants(nsSoap + "address").FirstOrDefault();
            if (addressElement != null)
            {
                var location = addressElement.Attribute("location");
                if (location != null)
                {
                    location.Value = url;
                }
            }

            var importElement = doc.Descendants(nsXsd + "import").FirstOrDefault();
            if (importElement != null)
            {
                var location = importElement.Attribute("schemaLocation");
                if (location != null)
                {
                    location.Value = $"{url}?xsd=1";
                }
            }

            var settings = new XmlWriterSettings { OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
            using (var memoryStream = new MemoryStream())
            using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
                return File(memoryStream.ToArray(), "text/xml");
            }
        }

        [HttpGet("xsd")]
        [Produces("text/xml")]
        [AllowAnonymous]
        public IActionResult GetSchemaXsd([FromQuery] string url)
        {
            var doc = XDocument.Load(_fileProvider.GetFileInfo("VPayWSServiceXSD.xml").PhysicalPath);
            var settings = new XmlWriterSettings { OmitXmlDeclaration = false, Encoding = Encoding.UTF8 };
            using (var memoryStream = new MemoryStream())
            using (var xmlWriter = XmlWriter.Create(memoryStream, settings))
            {
                doc.WriteTo(xmlWriter);
                xmlWriter.Flush();
                return File(memoryStream.ToArray(), "text/xml");
            }
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public string GetVer()
        {
            var aboutInfo = AboutInfo.GetBuildAboutInfo();

            return $"{aboutInfo.VersionInfo} - {aboutInfo.BuildTime}";
        }

        [HttpPost("echo")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public Task<StandardResponse> PostEcho(EchoRequest entity)
        {
            var aboutInfo = AboutInfo.GetBuildAboutInfo();

            return Task.FromResult(new StandardResponse()
            {
                CommonData = new CommonData()
                {
                    ResponseDesc = $"VPayWs: {aboutInfo.BuildTime} {aboutInfo.VersionInfo} {entity.Es}",
                    SuccessCode = "0",
                    ReasonCode = "0"
                }
            });
        }

        /// <summary>
        /// This service returns a list of reasons for transaction
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetReasonCodes")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(ReasonCodeResponse), 200)]
        public async Task<ReasonCodeResponse> GetReasonCodes(LegacyRequest request)
        {
            try
            {
                var response =
                    await _transactionService.GetReasonCodes(
                        StandardReasonCodeRequest(request.Envelope.Body.GetReasonCodes.Request));

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with GetReasonCodes");

                ReasonCodeResponse errorResponse = new ReasonCodeResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with GetReasonCodes"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This service will return related information about a transaction
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetTransactionDetails")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(TransactionDetailResponse), 200)]
        public async Task<TransactionDetailResponse> GetTransactionDetails(LegacyRequest request)
        {
            try
            {
                var response = await _transactionService.GetTransactionDetails(
                    StandardTransactionDetailRequest(request.Envelope.Body.GetTransactionDetails.Request));

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with GetTransactionDetails");

                TransactionDetailResponse errorResponse = new TransactionDetailResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with GetTransactionDetails"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This service retrieves the payment identification information for the transaction.
        /// The payment numbers returned are based on the transaction type.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("GetPanNumber")]
        [ServicePermissionAuthorize(ServicePermission.GetPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> GetPanNumber(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.GetPanNumber?.Request);

                var result = await _transactionService.GetPanNumber(sr);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with GetPanNumber");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with GetPanNumber"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This service tests the card payment specified by the transaction number for a Pre-Authorization without a corresponding Reversal or Settlement.
        /// When only a transaction number is input the web service tests all Pre-Authorization transactions for the card.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("OpenPreAuth")]
        [ServicePermissionAuthorize(ServicePermission.OpenPreAuth)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> OpenPreAuth(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.OpenPreAuth?.Request);

                var result = await _transactionService.OpenPreAuth(sr);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with OpenPreAuth");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with OpenPreAuth"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This service is used to specify payments and load funds for all payment types.
        /// For card payments, VPay® manages the card numbers and assigns a card number to the payment.
        /// For check payments outside of the VPay® Positive Pay system, an input check number is required.
        /// Checks in the VPay® Positive Pay system can be configured to use a client input check number or have the Positive Pay system generate the check number for the payment.
        /// For EFT payments, the bank routing number and account number are required to identify the account for the deposit.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("LoadPan")]
        [ServicePermissionAuthorize(ServicePermission.LoadPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> LoadPan(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.LoadPan?.Request);

                var result = await _transactionService.LoadPan(sr, request.Envelope.Body.LoadPan?.CustomData?.ClientData);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with LoadPan");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with LoadPan"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This retrieves the VPay® available and current balance information for the specified transaction.
        /// For payments processed through a switch, the available and current balances from the switch are returned.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("BalanceRequest")]
        [ServicePermissionAuthorize(ServicePermission.BalanceRequest)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> BalanceRequest(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.BalanceRequest?.Request);

                var result = await _transactionService.GetBalanceRequest(sr);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with BalanceRequest");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with BalanceRequest"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This service accepts a transaction number and an unload reason code as input.
        /// This service verifies the card has no pending pre-Authorizations, then unloads the funds from the card.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("UnloadPan")]
        [ServicePermissionAuthorize(ServicePermission.Unload)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> UnloadPan(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.UnloadPan?.Request);

                var result = await _transactionService.UnloadPan(sr);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with UnloadPan");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with UnloadPan"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This service accepts a transaction number and an unload reason code as input.
        /// A stop payment transaction is initiated for the check identified by the transaction number.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("StopPay")]
        [ServicePermissionAuthorize(ServicePermission.StopPay)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> StopPay(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.StopPay?.Request);

                var result = await _transactionService.StopPay(sr);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with StopPay");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with StopPay"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This will cancel the Fax job specified by the faxCode.  Only Fax jobs in HOLD status can be cancelled.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("CancelFax")]
        [ServicePermissionAuthorize(ServicePermission.CancelFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> CancelFax(LegacyRequest request)
        {
            try
            {
                var result = await _transactionService.CancelFax(request.Envelope.Body.CancelFax.Request);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with CancelFax");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with CancelFax"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This will update the fax number for the Fax job specified by the faxCode.
        /// Only Fax jobs in HOLD status can have the fax number updated.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ChangeFaxNumber")]
        [ServicePermissionAuthorize(ServicePermission.ChangeFaxNumber)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> ChangeFaxNumber(LegacyRequest request)
        {
            try
            {
                var result = await _transactionService.ChangeFaxNumber(request.Envelope.Body.ChangeFaxNumber.Request);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with ChangeFaxNumber");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with ChangeFaxNumber"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This will place the specified Fax job in HOLD status. You cannot put a CANCELLED Fax job on Hold.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("HoldFax")]
        [ServicePermissionAuthorize(ServicePermission.HoldFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> HoldFax(LegacyRequest request)
        {
            try
            {
                var result = await _transactionService.HoldFax(request.Envelope.Body.HoldFax.Request);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with HoldFax");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with HoldFax"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This will releases the Fax job from HOLD status to DUPENUMBER status.
        /// Letting the fax able able to be processed and sent.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ReleaseFax")]
        [ServicePermissionAuthorize(ServicePermission.ReleaseFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> ReleaseFax(LegacyRequest request)
        {
            try
            {
                var result = await _transactionService.ReleaseFax(request.Envelope.Body.ReleaseFax.Request);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with ReleaseFax");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with ReleaseFax"
                    }
                };

                return errorResponse;
            }
        }

        /// <summary>
        /// This creates a new Fax job to resend the specified Fax job.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("ResendFax")]
        [ServicePermissionAuthorize(ServicePermission.ResendFax)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> ResendFax(LegacyRequest request)
        {
            try
            {
                var result = await _transactionService.ResendFax(request.Envelope.Body.ResendFax.Request);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Unexpected Error with ResendFax");

                StandardResponse errorResponse = new StandardResponse()
                {
                    CommonData = new CommonData()
                    {
                        SuccessCode = "9997",
                        SuccessDesc = "Unexpected Error with ResendFax"
                    }
                };

                return errorResponse;
            }
        }

        private StandardRequest DefaultStandardRequest(StandardRequest request)
        {
            request.CommonData.User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
            request.CommonData.Token =
                _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            request.Source = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.System).Value[0];

            return request;
        }

        private ReasonCodeRequest StandardReasonCodeRequest(ReasonCodeRequestDto request)
        {
            var reasonCodeRequest = new ReasonCodeRequest()
            {
                TransNumber = request.TransNumber,
                User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Source = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.System).Value[0]
            };

            return reasonCodeRequest;
        }

        private TransactionDetailRequest StandardTransactionDetailRequest(TransactionDetailRequestDto request)
        {
            var transactionDetailRequest = new TransactionDetailRequest()
            {
                TransNumber = request.TransNumber,
                User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value,
                Source = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.System).Value[0]
            };

            return transactionDetailRequest;
        }
    }
}
