using System;
using System.Data;

namespace VPay.Payment.Common
{
    public static class DbHelpers
    {
        public static string GetStringSafe(this IDataReader reader, int colIndex)
        {
            return GetStringSafe(reader, colIndex, null);
        }

        public static string GetStringSafe(this IDataReader reader, int colIndex, string defaultValue)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetString(colIndex);
            else
                return defaultValue;
        }

        public static string GetStringSafe(this IDataReader reader, string indexName)
        {
            return GetStringSafe(reader, reader.GetOrdinal(indexName));
        }

        public static string GetStringSafe(this IDataReader reader, string indexName, string defaultValue)
        {
            return GetStringSafe(reader, reader.GetOrdinal(indexName), defaultValue);
        }


        public static DateTime? GetNullableDateTime(this IDataReader reader, int colIndex)
        {
            return GetNullableDateTime(reader, colIndex, null);
        }

        public static DateTime? GetNullableDateTime(this IDataReader reader, int colIndex, DateTime? defaultValue)
        {
            if (!reader.IsDBNull(colIndex))
                return reader.GetDateTime(colIndex);
            else
                return defaultValue;
        }

        public static DateTime? GetNullableDateTime(this IDataReader reader, string indexName)
        {
            return GetNullableDateTime(reader, reader.GetOrdinal(indexName));
        }

        public static DateTime? GetNullableDateTime(this IDataReader reader, string indexName, DateTime? defaultValue)
        {
            return GetNullableDateTime(reader, reader.GetOrdinal(indexName), defaultValue);
        }
    }
}
