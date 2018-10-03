using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions.Attributes;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using VPay.Payment.Common.Models;

namespace VPay.Payment
{
    public class LegacyValidationService: ILegacyValidationService
    {

        public Task<ValidationMessage> ValidateStandardRequest(StandardRequest standardRequest,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            var validate = ValidateLength(standardRequest.CardData);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.CheckData);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.Claim);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.CommonData);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.CorrespondenceData);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.CoveredItem);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.Merchant);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.Payment);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            validate = ValidateLength(standardRequest.SwitchTransaction);
            if (validate != null)
            {
                return Task.FromResult(validate);
            }

            return Task.FromResult(new ValidationMessage()
            {
                Code = "0000",
                Message = "Successful Validation"
            });
        }

        public async Task<ValidationMessage> ValidateChangeFaxNumberRequest(StandardRequest standardRequest, string originalFaxNumber,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (string.IsNullOrWhiteSpace(standardRequest?.CorrespondenceData?.PhoneNumber))
            {
                return new ValidationMessage()
                {
                    Code = "0055",
                    Message = "No Fax Number Provided"
                };
            }

            if (!Regex.IsMatch(standardRequest.CorrespondenceData.PhoneNumber, "^[0-9]{10,}$"))
            {
                return new ValidationMessage()
                {
                    Code = "0005",
                    Message = $"New Fax Number Invalid: {originalFaxNumber}"
                };
            }

            return await ValidateStandardRequest(standardRequest, cancellationToken);
        }

        public async Task<ValidationMessage> ValidateResendFaxNumberRequest(StandardRequest standardRequest, string originalFaxNumber,
            CancellationToken cancellationToken = default(CancellationToken))
        {
            if (!string.IsNullOrWhiteSpace(standardRequest.CorrespondenceData.PhoneNumber) && !Regex.IsMatch(standardRequest.CorrespondenceData.PhoneNumber, "^[0-9]{10,}$"))
            {
                return new ValidationMessage()
                {
                    Code = "0005",
                    Message = $"New Fax Number Invalid: {originalFaxNumber}"
                };
            }

            return await ValidateStandardRequest(standardRequest, cancellationToken);
        }


        private ValidationMessage ValidateLength<T>(T entity)
        {
            if (entity == null)
            {
                return null;
            }
            
            var invalidProperties = new List<string>();

            foreach (var (prop, attr) in ModelTools.GetTypePropertyAttributes<FixedLengthAttribute>(typeof(T)))
            {
                var value = (string)prop.GetValue(entity);

                if (value != null && value.Length > attr.Length)
                {
                    invalidProperties.Add(prop.Name.FirstCharacterToLower());
                }
            }

            if (invalidProperties.Any())
            {
                return new ValidationMessage()
                {
                    Code = "0990",
                    Message = $"Fields too long: {string.Join(", ", invalidProperties)}, "
                };
            }

            return null;
        }
    }
}
