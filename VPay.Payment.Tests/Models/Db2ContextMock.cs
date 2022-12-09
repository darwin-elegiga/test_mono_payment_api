using System;
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

        public T GetRepository<T>() where T : IDb2BaseRepository
        {
            if (typeof(T) == typeof(IFax))
            {
                return (T)Fax;
            }

            if (typeof(T) == typeof(ISecurity))
            {
                return (T)Security;
            }

            if (typeof(T) == typeof(ITransactionWs))
            {
                return (T)TransactionWs;
            }

            if (typeof(T) == typeof(ITradingPostWs))
            {
                return (T)TradingPostWs;
            }

            if (typeof(T) == typeof(IPreferencing))
            {
                return (T)Preferencing;
            }

            if (typeof(T) == typeof(ICorrespondenceRepo))
            {
                return (T)Correspondence;
            }

            if (typeof(T) == typeof(IProviderRepository))
            {
                return (T)ProviderRepository;
            }

            throw new InvalidOperationException("Unknown type: " + typeof(T).FullName);
        }

        public IFax Fax => FaxMock.Object;
        public ISecurity Security => SecurityMock.Object;
        public ITransactionWs TransactionWs => TransactionWsMock.Object;
        public ITradingPostWs TradingPostWs => TradingPostWsMock.Object;
        public IPreferencing Preferencing => PreferencingMock.Object;
        public ICorrespondenceRepo Correspondence => CorrespondenceMock.Object;

        public IProviderRepository ProviderRepository => ProviderMock.Object;

        public Mock<IFax> FaxMock { get; } = new Mock<IFax>();
        public Mock<ISecurity> SecurityMock { get; } = new Mock<ISecurity>();
        public Mock<ITransactionWs> TransactionWsMock { get; } = new Mock<ITransactionWs>();
        public Mock<ITradingPostWs> TradingPostWsMock { get; } = new Mock<ITradingPostWs>();
        public Mock<IPreferencing> PreferencingMock { get; } = new Mock<IPreferencing>();
        public Mock<ICorrespondenceRepo> CorrespondenceMock { get; } = new Mock<ICorrespondenceRepo>();

        public Mock<IProviderRepository> ProviderMock { get; } = new Mock<IProviderRepository>();
    }
}
