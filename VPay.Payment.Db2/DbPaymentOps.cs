using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Threading;
using System.Threading.Tasks;
using VPay.Data.Db2.Abstractions;
using VPay.Data.Db2.Abstractions.Helpers;
using VPay.Data.Db2.Abstractions.TransactionWs;
using VPay.Payment.Common;
using VPay.Payment.Common.DataWebService;
using VPay.Payment.Common.Db2;

namespace VPay.Payment.Db2
{
    public class DbPaymentOps : IDbPaymentOps, IHealthCheck
    {
        private readonly IDataConnection<OdbcConnection> _connection;
        private readonly IDb2Context _db2Context;

        public DbPaymentOps(IDataConnection<OdbcConnection> connection, IDb2Context context)
        {
            _connection = connection;
            _db2Context = context;

        }

        public string Component { get; } = "Db2";

        public async Task<bool> IsHealthy()
        {
            return await _db2Context.CanConnectAsync();
        }
        
        public void Dispose()
        {
            
        }

    }
}
