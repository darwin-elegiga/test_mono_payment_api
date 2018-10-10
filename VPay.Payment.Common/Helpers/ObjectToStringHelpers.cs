using System.Collections.Generic;
using System.Text;
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

    }
}
