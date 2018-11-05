using System.Threading;
using System.Threading.Tasks;
using Moq;
using VPay.Data.Db2.Abstractions;

namespace VPay.Payment.Tests.Models
{
    public class Db2ContextMock : IDb2Context
    {

        public bool CanConnectMock { get; set; } = true;

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            return Task.FromResult(CanConnectMock);
        }

        public IFax Fax => FaxMock.Object;
        public ISecurity Security => SecurityMock.Object;
        public ITransactionWs TransactionWs => TransactionWsMock.Object;
        public IPreferencing Preferencing => PreferencingMock.Object;
        public ICorrespondenceRepo Correspondence => CorrespondenceMock.Object;

        public Mock<IFax> FaxMock { get; } = new Mock<IFax>();
        public Mock<ISecurity> SecurityMock { get; } = new Mock<ISecurity>();
        public Mock<ITransactionWs> TransactionWsMock { get; } = new Mock<ITransactionWs>();
        public Mock<IPreferencing> PreferencingMock { get; } = new Mock<IPreferencing>();
        public Mock<ICorrespondenceRepo> CorrespondenceMock { get; } = new Mock<ICorrespondenceRepo>();
    }
}
