using System.Data;
using System.Data.Odbc;

namespace VPay.Payment.Db2.Helpers
{
    public static class OdbcHelpers
    {

        public static OdbcParameter AddWithValue(this OdbcParameterCollection collection, string parameterName, object value,
            OdbcType odbcType, int size, ParameterDirection direction)
        {
            var param = collection.Add(parameterName, odbcType, size);
            param.Direction = direction;
            param.Value = value;

            return param;
        }

    }
}
