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

        public async Task<List<ReasonCodeType>> ReasonCodesData(string token, string user, string txid)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL SP_GET_WEB_REASCODESRS(?,?,?,?,?,?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PUSTOKEN", token);
                cmd.Parameters.AddWithValue("PRETCODE", "");
                cmd.Parameters.AddWithValue("PRETDESC", "");
                cmd.Parameters.AddWithValue("PLOGIN", user);
                cmd.Parameters.AddWithValue("PTMCLIC", "");
                cmd.Parameters.AddWithValue("PTMBILT", "");
                cmd.Parameters.AddWithValue("PTMBILC", "");
                cmd.Parameters.AddWithValue("PTMTXID", txid);
                cmd.Parameters.AddWithValue("PREASCODE", "");
                cmd.Parameters.AddWithValue("PREASDESC", "");

                cmd.Parameters[0].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[0].OdbcType = OdbcType.Char;
                cmd.Parameters[0].Size = 128;
                cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[1].OdbcType = OdbcType.Char;
                cmd.Parameters[1].Size = 4;
                cmd.Parameters[2].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[2].OdbcType = OdbcType.Char;
                cmd.Parameters[2].Size = 256;

                List<ReasonCodeType> reasonCodes = new List<ReasonCodeType>();
                var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read())
                {
                    ReasonCodeType nextReasonCode = new ReasonCodeType();
                    nextReasonCode.ReasonCode = reader.GetString(0);
                    nextReasonCode.ReasonDesc = reader.GetString(1);
                    nextReasonCode.ReasonAdsc = reader.GetString(2);

                    reasonCodes.Add(nextReasonCode);
                }

                return reasonCodes;

            }
        }

        public async Task<List<Detail>> TransactionDetailsData(string token, string client, string billc, string txid)
        {
            var connection = await GetOpenConnection();

            using (var cmd = new OdbcCommand("CALL sewcps.SP_GET_DETAIL_TRANSACTION(?,?,?,?,?,?,?,?,?,?)", connection))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("PUSTOKEN", token);
                cmd.Parameters.AddWithValue("PRETCODE", "");
                cmd.Parameters.AddWithValue("PRETDESC", "");
                cmd.Parameters.AddWithValue("PORDER_COL", "ctrtran");
                cmd.Parameters.AddWithValue("PORDER_DESC", "asc");
                cmd.Parameters.AddWithValue("PCLIC", client.Trim());
                cmd.Parameters.AddWithValue("PBILC", billc.Trim());
                cmd.Parameters.AddWithValue("PTXID", txid);
                cmd.Parameters.Add("O_SUCCESS", OdbcType.Int, 10);
                cmd.Parameters.Add("QRY", OdbcType.Char, 60);

                cmd.Parameters[0].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[0].OdbcType = OdbcType.Char;
                cmd.Parameters[0].Size = 128;
                cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[1].OdbcType = OdbcType.Char;
                cmd.Parameters[1].Size = 4;
                cmd.Parameters[2].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[2].OdbcType = OdbcType.Char;
                cmd.Parameters[2].Size = 256;
                cmd.Parameters[8].Direction = ParameterDirection.Output;
                cmd.Parameters[9].Direction = ParameterDirection.Output;

                var reader = await cmd.ExecuteReaderAsync();
                List<Detail> details = new List<Detail>();
                while (reader.Read()) // or ReadAsync
                {
                    Detail nextDetail = new Detail();
                    nextDetail.ActionCode = MyStringOf(reader, "CRRACTCOD");
                    nextDetail.ActionDesc = MyStringOf(reader, "ACTIONCODEDESC");
                    nextDetail.Amount = FormattedNumberOf(reader, "CTRRAMT");
                    nextDetail.AuthCode = MyStringOf(reader, "CTRANBR");
                    nextDetail.BatchNumber = FormattedNumberOf(reader, "CTRBATN");
                    nextDetail.Expiration = FormattedNumberOf(reader, "CTRPAEX");
                    nextDetail.FinancialType = MyStringOf(reader, "FINANCIAL_TYPE");
                    nextDetail.LoadTran = MyIntOf(reader, "CTRLTRN");
                    nextDetail.MerchantCode = MyStringOf(reader, "CTRMCC");
                    nextDetail.MerchantName = MyStringOf(reader, "CTRMERCH");
                    nextDetail.ReasonCode = MyStringOf(reader, "CTRRESP");
                    nextDetail.ReasonDesc = MyStringOf(reader, "CTRREASON");
                    nextDetail.RequesterName = MyStringOf(reader, "CTRREQNM");
                    nextDetail.Status = MyStringOf(reader, "CTRSTAT");
                    nextDetail.StatusDesc = MyStringOf(reader, "STATUS_DESCRIPTION");
                    nextDetail.TranId = MyIntOf(reader, "CTRLTRN");
                    nextDetail.TranTimeStamp = DateStringOf(reader, "CTRTRTS");

                    details.Add(nextDetail);
                }

                return details;
            }
        }

        public async Task<List<HeaderData>> TransactionHeadersData(string token, string user, string password, string txid, AuthenticationValues authValues, string ipAddress)
        {
            var connection = await GetOpenConnection();
            List<HeaderData> headerDatas = new List<HeaderData>();

            using (var cmd = new OdbcCommand("CALL sewcps.GET_HEADER_TRANSACTION(?,?,?,?)", connection))
            {
                cmd.Parameters.AddWithValue("PUSTOKEN", token);
                cmd.Parameters.AddWithValue("PRETCODE", "");
                cmd.Parameters.AddWithValue("PRETDESC", "");
                cmd.Parameters.AddWithValue("PTXID", txid);

                cmd.Parameters[0].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[0].OdbcType = OdbcType.Char;
                cmd.Parameters[0].Size = 128;
                cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[1].OdbcType = OdbcType.Char;
                cmd.Parameters[1].Size = 4;
                cmd.Parameters[2].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[2].OdbcType = OdbcType.Char;
                cmd.Parameters[2].Size = 256;
                
                var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read()) // or ReadAsync
                {
                    HeaderData nextHeaderData = new HeaderData(); 
                    nextHeaderData.AvailBalance = FormattedNumberOf(reader, "TMAVAL");
                    nextHeaderData.BillCode = MyStringOf(reader, "TMBILC");
                    nextHeaderData.BillType = MyStringOf(reader, "TMBILT");
                    nextHeaderData.Client = MyStringOf(reader, "TMCLIC");
                    nextHeaderData.CurrentBalance = FormattedNumberOf(reader, "TMCVAL");
                    nextHeaderData.PayeeCode = MyStringOf(reader, "TMPAYEE");
                    nextHeaderData.PayeeName = MyStringOf(reader, "TMPAYEN");
                    nextHeaderData.ProviderName = MyStringOf(reader, "PROVIDER_NAME");
                    nextHeaderData.RequesterId = MyStringOf(reader, "TMRQID");
                    nextHeaderData.RequesterName = MyStringOf(reader, "TMRQNM");
                    nextHeaderData.TaxId = MyStringOf(reader, "TAXID");
                    nextHeaderData.TransNumber = MyIntOf(reader, "TMTXID");
                    nextHeaderData.UserField1 = MyStringOf(reader, "TMUDSP1");
                    nextHeaderData.UserField2 = MyStringOf(reader, "TMUDSP2");
                    nextHeaderData.UserField3 = MyStringOf(reader, "TMUDSP3");

                    headerDatas.Add(nextHeaderData);
                }
            }

            return headerDatas;
        }

        public async Task<List<CorespDtl>> TransactionCorrespondenceData(string token, string user, string txid)
        {
            var connection = await GetOpenConnection();
            List<CorespDtl> correspList = new List<CorespDtl>();

            using (var cmd = new OdbcCommand("CALL sewcps.SP_GET_CORRESPONDENCE_FOR_TRANSACTION(?,?,?,?)", connection))
            {
                cmd.Parameters.AddWithValue("PUSTOKEN", token);
                cmd.Parameters.AddWithValue("PRETCODE", "");
                cmd.Parameters.AddWithValue("PRETDESC", "");
                cmd.Parameters.AddWithValue("PTXID", txid);

                cmd.Parameters[0].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[0].OdbcType = OdbcType.Char;
                cmd.Parameters[0].Size = 128;
                cmd.Parameters[1].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[1].OdbcType = OdbcType.Char;
                cmd.Parameters[1].Size = 4;
                cmd.Parameters[2].Direction = ParameterDirection.InputOutput;
                cmd.Parameters[2].OdbcType = OdbcType.Char;
                cmd.Parameters[2].Size = 256;

                var reader = await cmd.ExecuteReaderAsync();
                while (reader.Read()) // or ReadAsync
                {
                    CorespDtl nextCorresp = new CorespDtl();
                    nextCorresp.DmRecId = MyIntOf(reader, "DMRECID");
                    nextCorresp.Direction = MyStringOf(reader, "DMCMDR");
                    nextCorresp.Type = MyStringOf(reader, "DMCMTH");
                    nextCorresp.Status = MyStringOf(reader, "DMSTAT");
                    nextCorresp.RequestDate = MyIntOf(reader, "DMENDT");
                    nextCorresp.StatusDate = MyIntOf(reader, "DMUPDT");
                    nextCorresp.SentBehalfName = MyStringOf(reader, "DMSNDNM");
                    nextCorresp.FromName = MyStringOf(reader, "DMSNDNM");
                    nextCorresp.FromAddress1 = MyStringOf(reader, "DMSNDAD1");
                    nextCorresp.FromAddress2 = MyStringOf(reader, "DMSNDAD2");
                    nextCorresp.FromCity = MyStringOf(reader, "DMSNDCTY");
                    nextCorresp.FromState = MyStringOf(reader, "DMSNDST");
                    nextCorresp.FromPostalCode = MyStringOf(reader, "DMSNDZIP");
                    nextCorresp.FromFax = MyStringOf(reader, "DMSNDFAX");
                    nextCorresp.ToName = MyStringOf(reader, "DMRCVNM");
                    nextCorresp.ToAddress1 = MyStringOf(reader, "DMRCVAD1");
                    nextCorresp.ToAddress2 = MyStringOf(reader, "DMRCVAD2");
                    nextCorresp.ToCity = MyStringOf(reader, "DMRCVCTY");
                    nextCorresp.ToState = MyStringOf(reader, "DMRCVST");
                    nextCorresp.ToPostalCode = MyStringOf(reader, "DMRCVZIP");
                    nextCorresp.ToFax = MyStringOf(reader, "DMRCVFAX");
                    nextCorresp.ToPhone = MyStringOf(reader, "DMRCVPHN");

                    correspList.Add(nextCorresp);
                }
            }

            return correspList;
        }

        private async Task<OdbcConnection> GetOpenConnection()
        {
            return await _connection.GetOpenConnectionAsync();
        }

        private string MyStringOf(DbDataReader reader, string columnName)
        {
            int columnIndex = reader.GetOrdinal(columnName);
            string returnValue = reader.GetString(columnIndex);

            return returnValue;
        }

        private int MyIntOf(DbDataReader reader, string columnName)
        {
            int columnIndex = reader.GetOrdinal(columnName);
            int returnValue = (int) reader.GetDecimal(columnIndex);

            return returnValue;
        }

        private string FormattedNumberOf(DbDataReader reader, string columnName)
        {
            int columnIndex = reader.GetOrdinal(columnName);
            decimal numberInColumn = reader.GetDecimal(columnIndex);
            string returnValue = numberInColumn.ToString("F");

            return returnValue;
        }

        private string DateStringOf(DbDataReader reader, string columnName)
        {
            int columnIndex = reader.GetOrdinal(columnName);
            DateTime dateInColumn = reader.GetDateTime(columnIndex);
            string returnValue = dateInColumn.ToString();

            return returnValue;
        }

    }
}
