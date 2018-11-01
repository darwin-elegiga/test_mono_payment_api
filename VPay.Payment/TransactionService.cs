using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Payment.Common;

namespace VPay.Payment
{
    public class TransactionService : ITransactionService
    {

        private readonly IDb2Context _db2Context;
        private readonly IUserInfo _user;
        private readonly ILogger _logger;


        public TransactionService(IDb2Context db2Context, IUserInfo user, ILogger<TransactionService> logger)
        {
            _db2Context = db2Context;
            _user = user;
            _logger = logger;
        }

        public async Task<ReasonCodeResponse> GetReasonCodes(ReasonCodeRequest request)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{request.ToDisplayString()}",
                nameof(GetReasonCodes), "Starting");

            var panRequestParam = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = new StandardRequest()
                {
                    CommonData = new CommonData()
                    {
                        TransNumber = request.TransNumber,
                        User = request.User,
                        Token = request.Token
                    }
                }
            };

            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{request.ToDisplayString()}",
                nameof(GetReasonCodes), "Testing Security");

            var panRequest = await _db2Context.TransactionWs.GetPan(panRequestParam);

            var reasonCodeResponse = new ReasonCodeResponse();

            if (panRequest.CommonData.SuccessCode == "0002")
            {
                reasonCodeResponse = new ReasonCodeResponse(panRequest.CommonData);
            }
            else
            {
                var reasonCodes = await _db2Context.TransactionWs.ReasonCodesData(request.Token, request.User, request.TransNumber);

                reasonCodeResponse.ReasonCodeList = reasonCodes;
                reasonCodeResponse.CommonData = new CommonData
                {
                    SuccessCode = "0000",
                    SuccessDesc = "Successful Completion",
                    User = request.User,
                    TransNumber = request.TransNumber
                };
            }

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = reasonCodeResponse.CommonData.ReasonCode,
                ["ReasonDesc"] = reasonCodeResponse.CommonData.ReasonDesc,
                ["ResponseCode"] = reasonCodeResponse.CommonData.ResponseCode,
                ["ResponseDesc"] = reasonCodeResponse.CommonData.ResponseDesc,
                ["SuccessCode"] = reasonCodeResponse.CommonData.SuccessCode,
                ["SuccessDesc"] = reasonCodeResponse.CommonData.SuccessDesc,
            }))
            {
                var level = reasonCodeResponse.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{reasonCodeResponse.ToDisplayString()}",
                    nameof(GetPanNumber), "Response");
            }

            reasonCodeResponse.CommonData.ReasonDesc = "";
            reasonCodeResponse.CommonData.ReasonCode = "";

            return reasonCodeResponse;
        }

        public async Task<TransactionDetailResponse> GetTransactionDetails(TransactionDetailRequest request)
        {
            var standardRequest = new StandardRequest
            {
                CommonData = new CommonData
                {
                    TransNumber = request.TransNumber,
                    User = request.User,
                    Token = request.Token
                }
            };

            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}",
                nameof(GetTransactionDetails), "Starting");


            TransactionDetailResponse response;

            var panNumResponse = await GetPanNumber(standardRequest);

            if (panNumResponse.CommonData.SuccessCode == "0002")
            {
                response = new TransactionDetailResponse()
                {
                    CommonData = panNumResponse.CommonData
                };
            }
            else
            {
                var headerData = (await _db2Context.TransactionWs.TransactionHeadersData(request.Token, request.User, request.TransNumber)).FirstOrDefault();
                if (headerData != null)
                {
                    var convHeaderData = headerData.ToHeaderData();

                    string client = convHeaderData.Client;
                    string billCode = convHeaderData.BillCode;

                    StandardResponse balRequestResponse = await GetBalanceRequest(standardRequest);
                    convHeaderData.SwitchAvailBal = balRequestResponse.SwitchTransaction.AvailableBal;
                    convHeaderData.SwitchCurrentBal = balRequestResponse.SwitchTransaction.CurrentBal;

                    PayTypeDetail payTypeDetail = new PayTypeDetail();
                    payTypeDetail.CardNumber = panNumResponse.CardData.CardNumber;
                    payTypeDetail.CardCvv2 = panNumResponse.CardData.CardCvv2;
                    payTypeDetail.CardExp = panNumResponse.CardData.CardExpiration;
                    payTypeDetail.Association = panNumResponse.CardData.CardType;
                    payTypeDetail.Bank = panNumResponse.CardData.CardholderName;
                    payTypeDetail.OutsideCheck = panNumResponse.CheckData.CheckNumber;
                    payTypeDetail.PosPayCheck = panNumResponse.CheckData.PosPayNumber;
                    payTypeDetail.SwitchNumber = panNumResponse.CheckData.SwitchNumber;
                    payTypeDetail.ClearCheck = panNumResponse.CheckData.ChkNum1;

                    var detailList = (await _db2Context.TransactionWs.TransactionDetailsData(request.Token, client, billCode, request.TransNumber)).ToDetail().ToList();
                    var correspList = await GetCorrespondenceList(long.Parse(request.TransNumber));

                    // when nothing goes wrong
                    string finalSuccessCode = "0000";
                    string finalSuccessDesc = "Successful Query";

                    if (detailList.Count == 0)
                    {
                        finalSuccessCode = "0101";
                        finalSuccessDesc = "Details Not Found for: " + request.TransNumber + "," + client + "," + billCode;
                    }
                    else if (correspList.Count == 0)
                    {
                        finalSuccessDesc = "No Correspondence Data For : " + request.TransNumber;
                    }

                    response = new TransactionDetailResponse()
                    {
                        DetailList = detailList,
                        HeaderData = convHeaderData,
                        CorrespondenceList = correspList,
                        PayTypeDetail = payTypeDetail,
                        CommonData = new CommonData()
                        {
                            User = request.User.ToUpper(),
                            TransNumber = request.TransNumber,
                            SuccessCode = finalSuccessCode,
                            SuccessDesc = finalSuccessDesc
                        }
                    };
                }
                else // if no header data exists
                {
                    response = new TransactionDetailResponse()
                    {
                        CommonData = new CommonData()
                        {
                            User = request.User.ToUpper(),
                            TransNumber = request.TransNumber,
                            SuccessCode = "0103",
                            SuccessDesc = "No Header for: " + request.TransNumber
                        }
                    };
                }
            }


            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = response.CommonData.ReasonCode,
                ["ReasonDesc"] = response.CommonData.ReasonDesc,
                ["ResponseCode"] = response.CommonData.ResponseCode,
                ["ResponseDesc"] = response.CommonData.ResponseDesc,
                ["SuccessCode"] = response.CommonData.SuccessCode,
                ["SuccessDesc"] = response.CommonData.SuccessDesc,
            }))
            {
                var level = response.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{response.ToDisplayString()}",
                    nameof(GetTransactionDetails), "Response");
            }

            response.CommonData.ReasonDesc = "";
            response.CommonData.ReasonCode = "";

            return response;
        }

        public async Task<StandardResponse> GetPanNumber(StandardRequest standardRequest)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}",
                nameof(GetPanNumber), "Starting");

            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.GetPan(request);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = result.CommonData.ReasonCode,
                ["ReasonDesc"] = result.CommonData.ReasonDesc,
                ["ResponseCode"] = result.CommonData.ResponseCode,
                ["ResponseDesc"] = result.CommonData.ResponseDesc,
                ["SuccessCode"] = result.CommonData.SuccessCode,
                ["SuccessDesc"] = result.CommonData.SuccessDesc,
            }))
            {
                var level = result.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{result.ToDisplayString()}",
                    nameof(GetPanNumber), "Response");
            }

            ClearCommonData(result);

            return result;
        }

        public async Task<StandardResponse> OpenPreAuth(StandardRequest standardRequest)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}", nameof(OpenPreAuth), "Starting");

            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.OpenPreAuth(request);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = result.CommonData.ReasonCode,
                ["ReasonDesc"] = result.CommonData.ReasonDesc,
                ["ResponseCode"] = result.CommonData.ResponseCode,
                ["ResponseDesc"] = result.CommonData.ResponseDesc,
                ["SuccessCode"] = result.CommonData.SuccessCode,
                ["SuccessDesc"] = result.CommonData.SuccessDesc,
            }))
            {
                var level = result.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{result.ToDisplayString()}", nameof(OpenPreAuth), "Response");
            }

            ClearCommonData(result);

            return result;
        }

        public async Task<StandardResponse> LoadPan(StandardRequest standardRequest)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}", nameof(LoadPan), "Starting");

            SetupDefaultValuesForLoadPan(standardRequest);

            var checkDeclineMessages = CheckLoadPanForDeclineErrorMessages(standardRequest);

            // Setting these response codes and description. If these are set to non-success code of 0000
            // then the LoadPan db2 call will record an declined message with these responses
            // WE MUST STILL CALL THE STORED PROC, even if there is an error in these responses.
            standardRequest.CommonData.ResponseCode = checkDeclineMessages.code;
            standardRequest.CommonData.ResponseDesc = checkDeclineMessages.message;

            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}", nameof(LoadPan), "After CheckMessage");

            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.LoadPan(request);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = result.CommonData.ReasonCode,
                ["ReasonDesc"] = result.CommonData.ReasonDesc,
                ["ResponseCode"] = result.CommonData.ResponseCode,
                ["ResponseDesc"] = result.CommonData.ResponseDesc,
                ["SuccessCode"] = result.CommonData.SuccessCode,
                ["SuccessDesc"] = result.CommonData.SuccessDesc,
            }))
            {
                var level = result.CommonData.SuccessCode == "0000" && checkDeclineMessages.code == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{result.ToDisplayString()}",
                    nameof(LoadPan), "Response");
            }

            // Set the Success Code and Description to the Declined Message if the Declined Message is an error code
            if (checkDeclineMessages.code != "0000")
            {
                result.CommonData.SuccessCode = checkDeclineMessages.code;
                result.CommonData.SuccessDesc = checkDeclineMessages.message;
            }

            ClearCommonData(result);

            return result;
        }

        public async Task<StandardResponse> GetBalanceRequest(StandardRequest standardRequest)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}", nameof(GetBalanceRequest), "Starting");

            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.BalanceRequest(request);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = result.CommonData.ReasonCode,
                ["ReasonDesc"] = result.CommonData.ReasonDesc,
                ["ResponseCode"] = result.CommonData.ResponseCode,
                ["ResponseDesc"] = result.CommonData.ResponseDesc,
                ["SuccessCode"] = result.CommonData.SuccessCode,
                ["SuccessDesc"] = result.CommonData.SuccessDesc,
            }))
            {
                var level = result.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{result.ToDisplayString()}",
                    nameof(GetBalanceRequest), "Response");
            }

            ClearCommonData(result);

            return result;
        }

        public async Task<StandardResponse> UnloadPan(StandardRequest standardRequest)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}", nameof(UnloadPan), "Starting");

            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.UnloadPan(request);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = result.CommonData.ReasonCode,
                ["ReasonDesc"] = result.CommonData.ReasonDesc,
                ["ResponseCode"] = result.CommonData.ResponseCode,
                ["ResponseDesc"] = result.CommonData.ResponseDesc,
                ["SuccessCode"] = result.CommonData.SuccessCode,
                ["SuccessDesc"] = result.CommonData.SuccessDesc,
            }))
            {
                var level = result.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{result.ToDisplayString()}",
                    nameof(UnloadPan), "Response");
            }

            ClearCommonData(result);

            return result;
        }

        public async Task<StandardResponse> StopPay(StandardRequest standardRequest)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \n{standardRequest.ToDisplayString()}", nameof(StopPay), "Starting");

            var request = new TransactionWsRequest()
            {
                UserId = "WSQATEST",
                Password = "QATEST01WS18",
                IpAddress = "10.120.202.129",
                Request = standardRequest
            };

            var result = await _db2Context.TransactionWs.StopPay(request);

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["ReasonCode"] = result.CommonData.ReasonCode,
                ["ReasonDesc"] = result.CommonData.ReasonDesc,
                ["ResponseCode"] = result.CommonData.ResponseCode,
                ["ResponseDesc"] = result.CommonData.ResponseDesc,
                ["SuccessCode"] = result.CommonData.SuccessCode,
                ["SuccessDesc"] = result.CommonData.SuccessDesc,
            }))
            {
                var level = result.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{result.ToDisplayString()}",
                    nameof(StopPay), "Response");
            }

            ClearCommonData(result);

            return result;
        }

        public async Task<StandardResponse> CancelFax(int faxCode)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \nFaxCode={{FaxCode}}", nameof(CancelFax), "Starting", faxCode);

            var dbResult = await _db2Context.Fax.CancelFaxAsync(_user.Token, faxCode);

            var sResp = PackAndUnpackResponse();
            sResp.CommonData.SuccessCode = dbResult.SuccessCode;
            sResp.CommonData.SuccessDesc = dbResult.SuccessDescription;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["SuccessCode"] = sResp.CommonData.SuccessCode,
                ["SuccessDesc"] = sResp.CommonData.SuccessDesc,
            }))
            {
                var level = sResp.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{sResp.ToDisplayString()}",
                    nameof(CancelFax), "Response", faxCode);
            }

            return sResp;
        }

        public async Task<StandardResponse> ChangeFaxNumber(int faxCode, string faxNumber)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \nFaxCode={{FaxCode}}, FaxNumber={faxNumber}", nameof(ChangeFaxNumber), "Starting", faxCode);

            var dbResult = await _db2Context.Fax.ChangeFaxNumberAsync(_user.Token, faxCode, faxNumber);

            var sResp = PackAndUnpackResponse();
            sResp.CommonData.SuccessCode = dbResult.SuccessCode;
            sResp.CommonData.SuccessDesc = dbResult.SuccessDescription;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["SuccessCode"] = sResp.CommonData.SuccessCode,
                ["SuccessDesc"] = sResp.CommonData.SuccessDesc,
            }))
            {
                var level = sResp.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{sResp.ToDisplayString()}",
                    nameof(ChangeFaxNumber), "Response", faxCode);
            }

            return sResp;
        }

        public async Task<StandardResponse> HoldFax(int faxCode)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \nFaxCode={{FaxCode}}", nameof(HoldFax), "Starting", faxCode);

            var dbResult = await _db2Context.Fax.HoldFaxAsync(_user.Token, faxCode);

            var sResp = PackAndUnpackResponse();
            sResp.CommonData.SuccessCode = dbResult.SuccessCode;
            sResp.CommonData.SuccessDesc = dbResult.SuccessDescription;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["SuccessCode"] = sResp.CommonData.SuccessCode,
                ["SuccessDesc"] = sResp.CommonData.SuccessDesc,
            }))
            {
                var level = sResp.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{sResp.ToDisplayString()}", nameof(HoldFax),
                    "Response", faxCode);
            }

            return sResp;
        }

        public async Task<StandardResponse> ReleaseFax(int faxCode)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \nFaxCode={{FaxCode}}", nameof(ReleaseFax), "Starting", faxCode);

            var dbResult = await _db2Context.Fax.ReleaseFaxAsync(_user.Token, faxCode);

            var sResp = PackAndUnpackResponse();
            sResp.CommonData.SuccessCode = dbResult.SuccessCode;
            sResp.CommonData.SuccessDesc = dbResult.SuccessDescription;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["SuccessCode"] = sResp.CommonData.SuccessCode,
                ["SuccessDesc"] = sResp.CommonData.SuccessDesc,
            }))
            {
                var level = sResp.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{sResp.ToDisplayString()}",
                    nameof(ReleaseFax), "Response", faxCode);
            }
            return sResp;
        }

        public async Task<StandardResponse> ResendFax(int faxCode, string faxNumber)
        {
            _logger.LogInformation($"{{ServiceName}} - {{Step}} with: \nFaxCode={{FaxCode}}, FaxNumber={faxNumber}", nameof(ResendFax), "Starting", faxCode);

            var dbResult = await _db2Context.Fax.ResendFaxAsync(_user.Token, faxCode, faxNumber ?? "");

            var sResp = PackAndUnpackResponse();
            sResp.CommonData.SuccessCode = dbResult.SuccessCode;
            sResp.CommonData.SuccessDesc = dbResult.SuccessDescription;
            sResp.CorrespondenceData.PhoneNumber = dbResult.FaxNumber;

            using (_logger.BeginScope(new Dictionary<string, object>
            {
                ["SuccessCode"] = sResp.CommonData.SuccessCode,
                ["SuccessDesc"] = sResp.CommonData.SuccessDesc,
            }))
            {
                var level = sResp.CommonData.SuccessCode == "0000" ? LogLevel.Information : LogLevel.Warning;
                _logger.Log(level, $"{{ServiceName}} - {{Step}} with: \n{sResp.ToDisplayString()}", nameof(ResendFax), "Response", faxCode);
            }

            return sResp;
        }
        

        private async Task<List<CorespDtl>> GetCorrespondenceList(long transactionId)
        {

            var result = (await _db2Context.Correspondence.GetByTransactionId(transactionId)).ToCorespDtl().ToList();
            if (result.Count > 0)
            {
                var faxes = (await _db2Context.Fax.GetFaxJobByTransactionId(transactionId))
                    .OrderBy(x => x.CreatedTimeStamp).ThenBy(x => x.LastStatusTimeStamp).ToList();

                if (faxes.Count > 0)
                {
                    var faxStatus = (await _db2Context.Fax.GetFaxJobStatusByTransactionId(transactionId))
                        .OrderBy(x => x.CreatedTimeStamp).ToList();

                    foreach (var corr in result)
                    {
                        corr.FaxJobList = faxes.Where(x => x.CorrespondenceId == corr.DmRecId).ToFaxJobs().ToList();

                        var lastStatRank = 10;
                        var lastStatText = "";

                        foreach (var faxJob in corr.FaxJobList)
                        {
                            var statuses = faxStatus.Where(x => x.FaxJobId == faxJob.FaxJobId).ToList();

                            foreach (var faxJobStatus in statuses)
                            {
                                switch (faxJobStatus.StatusCode?.Trim())
                                {
                                    case "000":
                                        if (8 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 8;
                                        }

                                        break;
                                    case "001":
                                        if (7 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 7;
                                        }

                                        break;
                                    case "004":
                                        if (6 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 6;
                                        }

                                        break;
                                    case "003":
                                        if (5 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 5;
                                        }

                                        break;
                                    case "002":
                                        if (4 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 4;
                                        }

                                        break;
                                    case "006":
                                        if (3 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 3;
                                        }

                                        break;
                                    case "005":
                                        if (2 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 2;
                                        }

                                        break;
                                    case "999":
                                        if (1 < lastStatRank)
                                        {
                                            corr.StatusText = faxJobStatus.StatusText;
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 1;
                                        }

                                        break;
                                    default:
                                        if (0 < lastStatRank)
                                        {
                                            corr.StatusText = "";
                                            lastStatText = faxJobStatus.StatusText;
                                            lastStatRank = 0;
                                        }

                                        break;
                                }
                            }
                        }
                    }

                }

            }

            return result;
        }

        private void SetupDefaultValuesForLoadPan(StandardRequest sr)
        {
            if (sr.Claim != null)
            {
                if (string.IsNullOrWhiteSpace(sr.Claim.Amount))
                {
                    sr.Claim.Amount = "0.00";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.ClaimOdometer))
                {
                    sr.Claim.ClaimOdometer = "000000";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.ClaimDeductible))
                {
                    sr.Claim.ClaimDeductible = "0.00";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.ClaimDate))
                {
                    sr.Claim.ClaimDate = "10000101";
                }
                if (string.IsNullOrWhiteSpace(sr.Claim.CurrencyType))
                {
                    sr.Claim.CurrencyType = "USD";
                }

                if (!string.IsNullOrWhiteSpace(sr.Claim.UserField1))
                {
                    sr.Claim.UserField1 = sr.Claim.UserField1.ToUpper();
                }
            }

            if (sr.CoveredItem != null)
            {
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.Year))
                {
                    sr.CoveredItem.Year = "1000";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.Deductible))
                {
                    sr.CoveredItem.Deductible = "0.00";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.BeginOdometer))
                {
                    sr.CoveredItem.BeginOdometer = "000000";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.ExpireDate))
                {
                    sr.CoveredItem.ExpireDate = "10000101";
                }
                if (string.IsNullOrWhiteSpace(sr.CoveredItem.BeginDate))
                {
                    sr.CoveredItem.BeginDate = "10000101";
                }
            }

        }

        /// <summary>
        /// This will Validate the data in the objects for the load pan and create response error codes.
        /// The order of this is setup based on the java code. 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        private (string code, string message) CheckLoadPanForDeclineErrorMessages(StandardRequest request)
        {
            var notEqualMsg = "Invalid Numeric Format:";
            var invalidDateMsg = "Date format not ISO ";
            var greaterThanAmtMsg = "Amount must be >= 0.00 ";
            var equalAmtMsg = "Amount must be = 0.00 ";

            var result = (code: "0000", message: "Successful Validation");

            if (decimal.TryParse(request.Claim.Amount, out var amount))
            {
                if (request.Payment.Type == "CLNPF")
                {
                    if (amount != 0)
                    {
                        result = (code: "0908", message: equalAmtMsg);
                    }
                }
                else
                {
                    if (amount <= 0)
                    {
                        result = (code: "0908", message: greaterThanAmtMsg);
                    }
                }
            }
            else
            {
                result = (code: "0930", message: $"{notEqualMsg} amount {request.Claim.Amount}");
            }

            if (!int.TryParse(request.CoveredItem.Year, out _))
            {
                result = (code: "0931", message: $"{notEqualMsg} year {request.CoveredItem.Year}");
            }

            if (!decimal.TryParse(request.CoveredItem.Deductible, NumberStyles.Float, CultureInfo.CurrentCulture, out _))
            {
                result = (code: "0932", message: $"{notEqualMsg} item deductible {request.CoveredItem.Deductible}");
            }

            if (!int.TryParse(request.Claim.ClaimOdometer, out _))
            {
                result = (code: "0933", message: $"{notEqualMsg} claimOdometer {request.Claim.ClaimOdometer}");
            }

            if (!int.TryParse(request.CoveredItem.BeginOdometer, out _))
            {
                result = (code: "0934", message: $"{notEqualMsg} beginOdometer {request.CoveredItem.BeginOdometer}");
            }

            if (!decimal.TryParse(request.Claim.ClaimDeductible, NumberStyles.Float, CultureInfo.CurrentCulture, out _))
            {
                result = (code: "0936", message: $"{notEqualMsg} claimDeductible {request.Claim.ClaimDeductible}");
            }

            if (!int.TryParse(request.Claim.ClaimDate, out _))
            {
                result = (code: "0935", message: $"{notEqualMsg} claimDate {request.Claim.ClaimDate}");
            }
            
            if (!int.TryParse(request.CoveredItem.BeginDate, out _))
            {
                result = (code: "0948", message: $"{notEqualMsg} beginDate {request.CoveredItem.BeginDate}");
            }

            if (!int.TryParse(request.CoveredItem.ExpireDate, out _))
            {
                result = (code: "0949", message: $"{notEqualMsg} expireDate {request.CoveredItem.ExpireDate}");
            }

            if (string.IsNullOrWhiteSpace(request.Payment.Client))
            {
                result = (code: "0912", message: $"Client Code cannot be Blank");
            }

            if (string.IsNullOrWhiteSpace(request.Claim.UserKey))
            {
                result = (code: "0903", message: $"User Key cannot be Blank");
            }

            if (string.IsNullOrWhiteSpace(request.Merchant.Fax))
            {
                result = (code: "0959", message: $"merchant fax cannot be Blank");
            }

            if (string.IsNullOrWhiteSpace(request.Merchant.PayeeName))
            {
                result = (code: "0904", message: $"Payee Name cannot be Blank");
            }

            if (string.IsNullOrWhiteSpace(request.Payment.BillCode))
            {
                result = (code: "0906", message: $"Bill Code cannot be blank");
            }

            if (string.IsNullOrWhiteSpace(request.Payment.Type))
            {
                result = (code: "0905", message: $"Bill Type cannot be blank");
            }

            if (string.IsNullOrWhiteSpace(request.Merchant.PayeeCode))
            {
                result = (code: "0909", message: $"Payee Code cannot be Blank");
            }

            if (request.Payment.Type == "CLCHK")
            {
                if (string.IsNullOrEmpty(request.CheckData.CheckDate))
                {
                    result = (code: "0952", message: $"Check Date cannot be Blank");
                }
                if (string.IsNullOrEmpty(request.CheckData.Address1))
                {
                    result = (code: "0953", message: $"Check Address1 cannot be Blank");
                }
                if (string.IsNullOrEmpty(request.CheckData.City))
                {
                    result = (code: "0954", message: $"Check City cannot be Blank");
                }
                if (string.IsNullOrEmpty(request.CheckData.StateOrProvince))
                {
                    result = (code: "0955", message: $"Check State cannot be Blank");
                }
                if (string.IsNullOrEmpty(request.CheckData.Zip))
                {
                    result = (code: "0956", message: $"Check Zip cannot be Blank");
                }
            }
            else if (request.Payment.Type == "CLEFT")
            {
                if (string.IsNullOrEmpty(request.Payment.RoutingNumber))
                {
                    result = (code: "0957", message: $"RoutingNumber cannot be Blank");
                }
                if (string.IsNullOrEmpty(request.Payment.AccountNumber))
                {
                    result = (code: "0958", message: $"AccountNumber cannot be Blank");
                }
            }

            return result;

        }

        private void ClearCommonData(StandardResponse response)
        {
            response.CommonData.ReasonCode = "";
            response.CommonData.ReasonDesc = "";
            response.CommonData.Token = "";
        }

        private StandardResponse PackAndUnpackResponse()
        {
            StandardRequest blankRequest = new StandardRequest()
            {
                CommonData = new CommonData(),
                CardData = new CardData(),
                CheckData = new CheckData(),
                Claim = new ClaimData(),
                CorrespondenceData = new CorrespondenceData(),
                CoveredItem = new CoveredItemData(),
                Merchant = new MerchantData(),
                Payment = new PaymentData(),
                SwitchTransaction = new SwitchTransactionData()
            };
            string packedRequest = blankRequest.Pack();
            StandardResponse unpackedResponse = TransactionWsStringHelpers.Unpack(packedRequest);

            return unpackedResponse;
        }
    }
}
