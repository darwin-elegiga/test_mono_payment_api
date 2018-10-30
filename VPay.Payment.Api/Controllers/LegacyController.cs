using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        // Add version tag
        private readonly string _version;

        private readonly IHttpContextAccessor _accessor;
        private readonly ILegacyTransactionService _transactionService;

        private readonly ILogger _logger;

        public LegacyController(IHttpContextAccessor accessor, ILegacyTransactionService transactionService, ILogger<LegacyController> logger)
        {
            _accessor = accessor;
            _transactionService = transactionService;
            _logger = logger;

            // TODO:  Initialize private variables
            _version = "2018-08-10";
        }

        [HttpGet("version")]
        [AllowAnonymous]
        public string GetVer()
        {
            return _version;
        }

        [HttpPost("echo")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public Task<StandardResponse> PostEcho(EchoRequest entity)
        {
            return Task.FromResult(new StandardResponse()
            {
                CommonData = new CommonData()
                {
                    ResponseDesc = entity.Es,
                    SuccessCode = "0",
                    ReasonCode = "0"
                }
            });
        }

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

        [HttpPost("LoadPan")]
        [ServicePermissionAuthorize(ServicePermission.LoadPan)]
        [ProducesResponseType(typeof(StandardResponse), 200)]
        public async Task<StandardResponse> LoadPan(LegacyRequest request)
        {
            try
            {
                var sr = DefaultStandardRequest(request.Envelope.Body.LoadPan?.Request);

                var result = await _transactionService.LoadPan(sr);

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

            return request;
        }


        private ReasonCodeRequest StandardReasonCodeRequest(ReasonCodeRequestDto request)
        {
            var reasonCodeRequest = new ReasonCodeRequest()
            {
                TransNumber = request.TransNumber,
                User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value
            };

            return reasonCodeRequest;
        }

        private TransactionDetailRequest StandardTransactionDetailRequest(TransactionDetailRequestDto request)
        {
            var transactionDetailRequest = new TransactionDetailRequest()
            {
                TransNumber = request.TransNumber,
                User = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.Name).Value,
                Token = _accessor.HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value
            };

            return transactionDetailRequest;
        }

    }
}
