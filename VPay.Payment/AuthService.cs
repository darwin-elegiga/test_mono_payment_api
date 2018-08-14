using System.Threading.Tasks;
using VPay.Payment.Common;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;

namespace VPay.Payment
{
    public class AuthService : IAuthService
    {
        private readonly IDbPaymentOps _db;

        public AuthService(IDbPaymentOps db)
        {
            _db = db;
        }

        public async Task<bool> IsAuthenticated(AuthenticationValues av, string ipAddress)
        {
            var result = await _db.AuthenticateUser(new AuthenticationParam()
            {
                Id = av.Id,
                PassPhrase = av.PassPhrase,
                IpAddress = ipAddress
            });

            return result.Result == "0";
        }

        public async Task<AuthenticationResult> TestAuthentication(AuthenticationParam param)
        {
            var result = await _db.AuthenticateUser(param);

            return result;
        }
    }
}
