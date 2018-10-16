using System.Collections.Generic;
using System.Linq;
using System.Text;
using VPay.Data.Db2.Abstractions.CorrespondenceRepo;
using VPay.Data.Db2.Abstractions.TransactionWs;

namespace VPay.Payment.Common
{
    public static class ObjectToStringHelpers
    {

        public static string ToDisplayString(this StandardRequest request)
        {
            var sb = new StringBuilder();

            sb.Append(request.CommonData.ToDisplayString());
            sb.Append(request.CardData.ToDisplayString());
            sb.Append(request.CheckData.ToDisplayString());
            sb.Append(request.Claim.ToDisplayString());
            sb.Append(request.CorrespondenceData.ToDisplayString());
            sb.Append(request.CoveredItem.ToDisplayString());
            sb.Append(request.Merchant.ToDisplayString());
            sb.Append(request.Payment.ToDisplayString());
            sb.Append(request.SwitchTransaction.ToDisplayString());

            sb.Append($"Source={request.Source}");

            return sb.ToString();
        }

        public static string ToDisplayString(this StandardResponse response)
        {
            var sb = new StringBuilder();

            sb.Append(response.CommonData.ToDisplayString());
            sb.Append(response.CardData.ToDisplayString());
            sb.Append(response.CheckData.ToDisplayString());
            sb.Append(response.Claim.ToDisplayString());
            sb.Append(response.CorrespondenceData.ToDisplayString());
            sb.Append(response.CoveredItem.ToDisplayString());
            sb.Append(response.Merchant.ToDisplayString());
            sb.Append(response.Payment.ToDisplayString());
            sb.Append(response.SwitchTransaction.ToDisplayString());

            return sb.ToString();
        }

        public static string ToDisplayString(this ReasonCodeRequest request)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(request.TransNumber))
            {
                listValues.Add($"TransNumber={request.TransNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(request.User))
            {
                listValues.Add($"User={request.User.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(request.Token))
            {
                listValues.Add(request.Token.Length > 63 ? $"Token={request.Token.Substring(1, 4)}..." : $"Token=****");
            }

            return listValues.Count == 0 ? "" : $"ReasonCodeRequest [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this ReasonCodeResponse response)
        {
            var sb = new StringBuilder();

            sb.Append(response.CommonData.ToDisplayString());

            if (response.ReasonCodeList != null && response.ReasonCodeList.Any())
            {
                sb.Append("ReasonCodes [ \r\n");
                foreach (var reasonCode in response.ReasonCodeList)
                {
                    sb.AppendLine(reasonCode.ToDisplayString());
                }

                sb.Append("] ");
            }


            return sb.ToString();
        }

        public static string ToDisplayString(this TransactionDetailResponse response)
        {
            var sb = new StringBuilder();

            sb.Append(response.CommonData.ToDisplayString());
            sb.Append(response.HeaderData.ToDisplayString());
            sb.Append(response.PayTypeDetail.ToDisplayString());

            if (response.DetailList != null && response.DetailList.Any())
            {
                sb.Append("DetailList [ \r\n");

                foreach (var detail in response.DetailList)
                {
                    sb.Append(detail.ToDisplayString());
                }

                sb.Append("] \r\n");
            }

            if (response.CorrespondenceList != null && response.CorrespondenceList.Any())
            {
                sb.Append("CorrespondenceList [ \r\n");

                foreach (var detail in response.CorrespondenceList)
                {
                    sb.Append(detail.ToDisplayString());
                }

                sb.Append("] \r\n");
            }


            return sb.ToString();
        }

        public static string ToDisplayString(this CommonData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.TransNumber))
            {
                listValues.Add($"TransNumber={entity.TransNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SeClaimID))
            {
                listValues.Add($"SeClaimId={entity.SeClaimID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.TpaClaimID))
            {
                listValues.Add($"TpaClaimId={entity.TpaClaimID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ProgramID))
            {
                listValues.Add($"ProgramId={entity.ProgramID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReasonCode))
            {
                listValues.Add($"ReasonCode={entity.ReasonCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReasonDesc))
            {
                listValues.Add($"ReasonDesc={entity.ReasonDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ResponseCode))
            {
                listValues.Add($"ResponseCode={entity.ResponseCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ResponseDesc))
            {
                listValues.Add($"ResponseDesc={entity.ResponseDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SuccessCode))
            {
                listValues.Add($"SuccessCode={entity.SuccessCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SuccessDesc))
            {
                listValues.Add($"SuccessDesc={entity.SuccessDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.User))
            {
                listValues.Add($"User={entity.User.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PassWord))
            {
                listValues.Add(@"PassWord=""*********""");
            }

            if (!string.IsNullOrWhiteSpace(entity.Token))
            {
                listValues.Add(entity.Token.Length > 63 ? $"Token={entity.Token.Substring(1, 4)}..." : $"Token=****");
            }

            return listValues.Count == 0 ? "" : $"CommonData [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this CardData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.CardType))
            {
                listValues.Add($"CardType={entity.CardType.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardNumber))
            {
                if (entity.CardNumber.Length > 15)
                {
                    listValues.Add($"CardNumber={entity.CardNumber.Substring(0, 4)}*********{entity.CardNumber.Substring(14, 2)}");
                }
                else
                {
                    listValues.Add($"CardNumber=****");
                }
            }

            if (!string.IsNullOrWhiteSpace(entity.CardCvv2))
            {
                listValues.Add($"CardCvv2={entity.CardCvv2.Substring(0, 1)}***");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardExpiration))
            {
                listValues.Add($"CardExpiration={entity.CardExpiration.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.LoadTransId))
            {
                listValues.Add($"LoadTransId={entity.LoadTransId.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.LoadAmount))
            {
                listValues.Add($"LoadAmount={entity.LoadAmount.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.LoadFee))
            {
                listValues.Add($"LoadFee={entity.LoadFee.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PayeeName))
            {
                listValues.Add($"PayeeName={entity.PayeeName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardholderName))
            {
                listValues.Add($"CardholderName={entity.CardholderName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardPostalCode))
            {
                listValues.Add($"CardPostalCode={entity.CardPostalCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UnloadCode))
            {
                listValues.Add($"UnloadCode={entity.UnloadCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UnloadDesc))
            {
                listValues.Add($"UnloadDesc={entity.UnloadDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.VcRef))
            {
                listValues.Add($"VcRef={entity.VcRef.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.DisplayCVV2))
            {
                listValues.Add($"DisplayCVV2={entity.DisplayCVV2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.MaskPan))
            {
                listValues.Add($"MaskPan={entity.MaskPan.Trim()}");
            }
            
            return listValues.Count == 0 ? "" : $"CardData [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this CheckData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.CheckDate))
            {
                listValues.Add($"CheckDate={entity.CheckDate.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CheckNumber))
            {
                listValues.Add($"CheckNumber={entity.CheckNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PosPayNumber))
            {
                listValues.Add($"PosPayNumber={entity.PosPayNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SwitchNumber))
            {
                listValues.Add($"SwitchNumber={entity.SwitchNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ChkNum1))
            {
                listValues.Add($"ChkNum1={entity.ChkNum1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ChkNum2))
            {
                listValues.Add($"ChkNum2={entity.ChkNum2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Address1))
            {
                listValues.Add($"Address1={entity.Address1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Address2))
            {
                listValues.Add($"Address2={entity.Address2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Address3))
            {
                listValues.Add($"Address3={entity.Address3.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.City))
            {
                listValues.Add($"City={entity.City.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Country))
            {
                listValues.Add($"Country={entity.Country.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.County))
            {
                listValues.Add($"County={entity.County.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Memo))
            {
                listValues.Add($"Memo={entity.Memo.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Region))
            {
                listValues.Add($"Region={entity.Region.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.StateOrProvince))
            {
                listValues.Add($"StateOrProvince={entity.StateOrProvince.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Zip))
            {
                listValues.Add($"Zip={entity.Zip.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"CheckData [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this ClaimData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.UserKey))
            {
                listValues.Add($"UserKey={entity.UserKey.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UserField1))
            {
                listValues.Add($"UserField1={entity.UserField1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UserField2))
            {
                listValues.Add($"UserField2={entity.UserField2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UserField3))
            {
                listValues.Add($"UserField3={entity.UserField3.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CurrencyType))
            {
                listValues.Add($"CurrencyType={entity.CurrencyType.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Amount))
            {
                listValues.Add($"Amount={entity.Amount.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ClaimDeductible))
            {
                listValues.Add($"ClaimDeductible={entity.ClaimDeductible.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ClaimOdometer))
            {
                listValues.Add($"ClaimOdometer={entity.ClaimOdometer.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ClaimDescription))
            {
                listValues.Add($"ClaimDescription={entity.ClaimDescription.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RequesterId))
            {
                listValues.Add($"RequesterId={entity.RequesterId.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RequesterName))
            {
                listValues.Add($"RequesterName={entity.RequesterName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RepairOrderId))
            {
                listValues.Add($"RepairOrderId={entity.RepairOrderId.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ClaimNotes))
            {
                listValues.Add($"ClaimNotes={entity.ClaimNotes.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"Claim [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this CorrespondenceData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.AttachmentLocation))
            {
                listValues.Add($"AttachmentLocation={entity.AttachmentLocation.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.DocumentID))
            {
                listValues.Add($"DocumentID={entity.DocumentID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Email))
            {
                listValues.Add($"Email={entity.Email.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FaxCode))
            {
                listValues.Add($"FaxCode={entity.FaxCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FaxStat))
            {
                listValues.Add($"FaxStat={entity.FaxStat.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PhoneNumber))
            {
                listValues.Add($"PhoneNumber={entity.PhoneNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Type))
            {
                listValues.Add($"Type={entity.Type.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"CorrespondenceData [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this CoveredItemData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.ItemType))
            {
                listValues.Add($"ItemType={entity.ItemType.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ItemId))
            {
                listValues.Add($"ItemId={entity.ItemId.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ItemYear))
            {
                listValues.Add($"ItemYear={entity.ItemYear.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Manufacturer))
            {
                listValues.Add($"Manufacturer={entity.Manufacturer.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Model))
            {
                listValues.Add($"Model={entity.Model.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BookStateOrProvince))
            {
                listValues.Add($"BookStateOrProvince={entity.BookStateOrProvince.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PostalCode))
            {
                listValues.Add($"PostalCode={entity.PostalCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PlanCode))
            {
                listValues.Add($"PlanCode={entity.PlanCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PlanDescription))
            {
                listValues.Add($"PlanDescription={entity.PlanDescription.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Deductible))
            {
                listValues.Add($"Deductible={entity.Deductible.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.NewUsed))
            {
                listValues.Add($"NewUsed={entity.NewUsed.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BeginDate))
            {
                listValues.Add($"BeginDate={entity.BeginDate.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ExpireDate))
            {
                listValues.Add($"ExpireDate={entity.ExpireDate.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.OdometerType))
            {
                listValues.Add($"OdometerType={entity.OdometerType.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BeginOdometer))
            {
                listValues.Add($"BeginOdometer={entity.BeginOdometer.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ExpireOdometer))
            {
                listValues.Add($"ExpireOdometer={entity.ExpireOdometer.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.OwnerLastName))
            {
                listValues.Add($"OwnerLastName={entity.OwnerLastName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.OwnerFirstName))
            {
                listValues.Add($"OwnerFirstName={entity.OwnerFirstName.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"Covered Item [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this MerchantData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.PayeeCode))
            {
                listValues.Add($"PayeeCode={entity.PayeeCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PayeeName))
            {
                listValues.Add($"PayeeName={entity.PayeeName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ContactPerson))
            {
                listValues.Add($"ContactPerson={entity.ContactPerson.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PostalCode))
            {
                listValues.Add($"PostalCode={entity.PostalCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Telephone))
            {
                listValues.Add($"Telephone={entity.Telephone.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Fax))
            {
                listValues.Add($"Fax={entity.Fax.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PostalCode))
            {
                listValues.Add($"PostalCode={entity.PostalCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.EmailAddress))
            {
                listValues.Add($"EmailAddress={entity.EmailAddress.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"Merchant [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this PaymentData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.AccountingCode))
            {
                listValues.Add($"AccountingCode={entity.AccountingCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AccountingDesc))
            {
                listValues.Add($"AccountingDesc={entity.AccountingDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AccountNumber))
            {
                listValues.Add($"AccountNumber={entity.AccountNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Action))
            {
                listValues.Add($"Action={entity.Action.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AvailableBalance))
            {
                listValues.Add($"AvailableBalance={entity.AvailableBalance.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BillCode))
            {
                listValues.Add($"BillCode={entity.BillCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Client))
            {
                listValues.Add($"Client={entity.Client.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CurrencyCode))
            {
                listValues.Add($"CurrencyCode={entity.CurrencyCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CurrentBalance))
            {
                listValues.Add($"CurrentBalance={entity.CurrentBalance.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Free))
            {
                listValues.Add($"Free={entity.Free.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FutureUse))
            {
                listValues.Add($"FutureUse={entity.FutureUse.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Id))
            {
                listValues.Add($"Id={entity.Id.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.LoadAmount))
            {
                listValues.Add($"LoadAmount={entity.LoadAmount.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PanNumber))
            {
                if (entity.PanNumber.Length > 15)
                {
                    listValues.Add($"PanNumber=*********{entity.PanNumber.Substring(14, 2)}****************");
                }
                else
                {
                    listValues.Add($"PanNumber=*****");
                }
            }

            if (!string.IsNullOrWhiteSpace(entity.RequestedAmount))
            {
                listValues.Add($"RequestedAmount={entity.RequestedAmount.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RoutingNumber))
            {
                listValues.Add($"RoutingNumber={entity.RoutingNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Type))
            {
                listValues.Add($"Type={entity.Type.Trim()}");
            }
            
            return listValues.Count == 0 ? "" : $"Payment [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this SwitchTransactionData entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.AcquireID))
            {
                listValues.Add($"AcquireID={entity.AcquireID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AuthCode))
            {
                listValues.Add($"AuthCode={entity.AuthCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AvailableBal))
            {
                listValues.Add($"AvailableBal={entity.AvailableBal.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CaptureTS))
            {
                listValues.Add($"CaptureTS={entity.CaptureTS.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CurrentBal))
            {
                listValues.Add($"CurrentBal={entity.CurrentBal.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.MerchantID))
            {
                listValues.Add($"MerchantID={entity.MerchantID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.OlsLogID))
            {
                listValues.Add($"OlsLogID={entity.OlsLogID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Stan))
            {
                listValues.Add($"Stan={entity.Stan.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Switch))
            {
                listValues.Add($"Switch={entity.Switch.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.TerminalID))
            {
                listValues.Add($"TerminalID={entity.TerminalID.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.TransactionTS))
            {
                listValues.Add($"TransactionTS={entity.TransactionTS.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"SwitchTransaction [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this ReasonCodeType entity)
        {
            if (entity == null)
            {
                return "";
            }
            return $"ReasonCode={entity.ReasonCode}, ReasonDesc={entity.ReasonDesc}, ActionDesc={entity.ReasonAdsc}";
        }

        public static string ToDisplayString(this HeaderData entity)
        {
            var listValues = new List<string>();

            listValues.Add($"TransNumber={entity.TransNumber}");

            if (!string.IsNullOrWhiteSpace(entity.Client))
            {
                listValues.Add($"Client={entity.Client.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BillCode))
            {
                listValues.Add($"BillCode={entity.BillCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BillType))
            {
                listValues.Add($"BillType={entity.BillType.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AvailBalance))
            {
                listValues.Add($"AvailBalance={entity.AvailBalance.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CurrentBalance))
            {
                listValues.Add($"CurrentBalance={entity.CurrentBalance.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SwitchAvailBal))
            {
                listValues.Add($"SwitchAvailBal={entity.SwitchAvailBal.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SwitchCurrentBal))
            {
                listValues.Add($"SwitchCurrentBal={entity.SwitchCurrentBal.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PayeeCode))
            {
                listValues.Add($"PayeeCode={entity.PayeeCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PayeeName))
            {
                listValues.Add($"PayeeName={entity.PayeeName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ProviderName))
            {
                listValues.Add($"ProviderName={entity.ProviderName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RequesterId))
            {
                listValues.Add($"RequesterId={entity.RequesterId.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RequesterName))
            {
                listValues.Add($"RequesterName={entity.RequesterName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.TaxId))
            {
                listValues.Add($"TaxId=*****");
            }

            if (!string.IsNullOrWhiteSpace(entity.UserField1))
            {
                listValues.Add($"UserField1={entity.UserField1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UserField2))
            {
                listValues.Add($"UserField2={entity.UserField2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.UserField3))
            {
                listValues.Add($"UserField3={entity.UserField3.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"HeaderData [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this PayTypeDetail entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.Association))
            {
                listValues.Add($"Association={entity.Association.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Bank))
            {
                listValues.Add($"Bank={entity.Bank.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardCvv2))
            {
                listValues.Add($"CardCvv2={entity.CardCvv2.Substring(0, 1)}***");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardExp))
            {
                listValues.Add($"CardExp={entity.CardExp.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.CardNumber))
            {
                if (entity.CardNumber.Length > 15)
                {
                    listValues.Add($"CardNumber={entity.CardNumber.Substring(0, 4)}*********{entity.CardNumber.Substring(14, 2)}");
                }
                else
                {
                    listValues.Add($"CardNumber=****");
                }
            }
            if (!string.IsNullOrWhiteSpace(entity.OutsideCheck))
            {
                listValues.Add($"OutsideCheck={entity.OutsideCheck.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ClearCheck))
            {
                listValues.Add($"ClearCheck={entity.ClearCheck.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.PosPayCheck))
            {
                listValues.Add($"PosPayCheck={entity.PosPayCheck.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SwitchNumber))
            {
                listValues.Add($"SwitchNumber={entity.SwitchNumber.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"PayTypeDetail [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this Detail entity)
        {
            var listValues = new List<string>();


            listValues.Add($"TranId={entity.TranId}");
            listValues.Add($"LoadTran={entity.LoadTran}");

            if (!string.IsNullOrWhiteSpace(entity.Status))
            {
                listValues.Add($"Status={entity.Status.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Amount))
            {
                listValues.Add($"Amount={entity.Amount.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.AuthCode))
            {
                listValues.Add($"AuthCode={entity.AuthCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.TranTimeStamp))
            {
                listValues.Add($"TranTimeStamp={entity.TranTimeStamp.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Expiration))
            {
                listValues.Add($"Expiration={entity.Expiration.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.MerchantCode))
            {
                listValues.Add($"MerchantCode={entity.MerchantCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.MerchantName))
            {
                listValues.Add($"MerchantName={entity.MerchantName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReasonCode))
            {
                listValues.Add($"ReasonCode={entity.ReasonCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReasonDesc))
            {
                listValues.Add($"ReasonDesc={entity.ReasonDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ActionCode))
            {
                listValues.Add($"ActionCode={entity.ActionCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ActionDesc))
            {
                listValues.Add($"ActionDesc={entity.ActionDesc.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FinancialType))
            {
                listValues.Add($"FinancialType={entity.FinancialType.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.RequesterName))
            {
                listValues.Add($"RequesterName={entity.RequesterName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.BatchNumber))
            {
                listValues.Add($"BatchNumber={entity.BatchNumber.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.StatusDesc))
            {
                listValues.Add($"StatusDesc={entity.StatusDesc.Trim()}");
            }

            return listValues.Count == 0 ? "" : $"Detail [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this CorespDtl entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.Direction))
            {
                listValues.Add($"Direction={entity.Direction.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Type))
            {
                listValues.Add($"Type={entity.Type.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Status))
            {
                listValues.Add($"Status={entity.Status.Trim()}");
            }



            listValues.Add($"RequestDate={entity.RequestDate}");
            listValues.Add($"StatusDate={entity.StatusDate}");

            if (!string.IsNullOrWhiteSpace(entity.SentBehalfName))
            {
                listValues.Add($"SentBehalfName={entity.SentBehalfName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromName))
            {
                listValues.Add($"FromName={entity.FromName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromAddress1))
            {
                listValues.Add($"FromAddress1={entity.FromAddress1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromAddress2))
            {
                listValues.Add($"FromAddress2={entity.FromAddress2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromCity))
            {
                listValues.Add($"FromCity={entity.FromCity.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromState))
            {
                listValues.Add($"FromState={entity.FromState.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromPostalCode))
            {
                listValues.Add($"FromPostalCode={entity.FromPostalCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.FromFax))
            {
                listValues.Add($"FromFax={entity.FromFax.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToName))
            {
                listValues.Add($"ToName={entity.ToName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToAddress1))
            {
                listValues.Add($"ToAddress1={entity.ToAddress1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToAddress2))
            {
                listValues.Add($"ToAddress2={entity.ToAddress2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToCity))
            {
                listValues.Add($"ToCity={entity.ToCity.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToState))
            {
                listValues.Add($"ToState={entity.ToState.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToPostalCode))
            {
                listValues.Add($"ToPostalCode={entity.ToPostalCode.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToFax))
            {
                listValues.Add($"ToFax={entity.ToFax.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ToPhone))
            {
                listValues.Add($"ToPhone={entity.ToPhone.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.StatusText))
            {
                listValues.Add($"StatusText={entity.StatusText.Trim()}");
            }

            if (entity.FaxJobList != null)
            {
                listValues.Add($"FaxJobListCount={entity.FaxJobList.Count}");
            }

            listValues.Add($"DmRecId={entity.DmRecId}");

            return listValues.Count == 0 ? "" : $"Correspondence [ {string.Join(", ", listValues)} ]\r\n";
        }

        public static string ToDisplayString(this Correspondence entity)
        {
            var listValues = new List<string>();

            if (!string.IsNullOrWhiteSpace(entity.Direction))
            {
                listValues.Add($"Direction={entity.Direction.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Type))
            {
                listValues.Add($"Type={entity.Type.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Status))
            {
                listValues.Add($"Status={entity.Status.Trim()}");
            }



            listValues.Add($"RequestDate={entity.RequestDateTime}");
            listValues.Add($"StatusDate={entity.StatusDatePart}");

            if (!string.IsNullOrWhiteSpace(entity.SenderName))
            {
                listValues.Add($"SentBehalfName={entity.SenderName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderName))
            {
                listValues.Add($"FromName={entity.SenderName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderAddress1))
            {
                listValues.Add($"FromAddress1={entity.SenderAddress1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderAddress2))
            {
                listValues.Add($"FromAddress2={entity.SenderAddress2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderCity))
            {
                listValues.Add($"FromCity={entity.SenderCity.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderState))
            {
                listValues.Add($"FromState={entity.SenderState.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderZip))
            {
                listValues.Add($"FromPostalCode={entity.SenderZip.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.SenderFax))
            {
                listValues.Add($"FromFax={entity.SenderFax.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverName))
            {
                listValues.Add($"ToName={entity.ReceiverName.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverAddress1))
            {
                listValues.Add($"ToAddress1={entity.ReceiverAddress1.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverAddress2))
            {
                listValues.Add($"ToAddress2={entity.ReceiverAddress2.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverCity))
            {
                listValues.Add($"ToCity={entity.ReceiverCity.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverState))
            {
                listValues.Add($"ToState={entity.ReceiverState.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverZip))
            {
                listValues.Add($"ToPostalCode={entity.ReceiverZip.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverFax))
            {
                listValues.Add($"ToFax={entity.ReceiverFax.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.ReceiverPhone))
            {
                listValues.Add($"ToPhone={entity.ReceiverPhone.Trim()}");
            }

            if (!string.IsNullOrWhiteSpace(entity.Status))
            {
                listValues.Add($"StatusText={entity.Status.Trim()}");
            }

            if (entity.FaxJobList != null)
            {
                listValues.Add($"FaxJobListCount={entity.FaxJobList.Count}");
            }

            listValues.Add($"DmRecId={entity.Id}");

            return listValues.Count == 0 ? "" : $"Correspondence [ {string.Join(", ", listValues)} ]\r\n";
        }

    }
}
