using System;
using System.Globalization;
using Newtonsoft.Json;

namespace VPay.Payment.Api
{
    public class DecimalJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal) || objectType == typeof(decimal?));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                string value = (string)reader.Value;
                if (value == string.Empty)
                {
                    return null;
                }
            }
            else if (reader.TokenType == JsonToken.Float ||
                     reader.TokenType == JsonToken.Integer)
            {
                return Convert.ToDecimal(reader.Value);
            }

            throw new JsonSerializationException("Unexpected token type: " + reader.TokenType.ToString());
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            decimal? dec = (decimal?)value;
            if (dec == null)
            {
                writer.WriteValue(string.Empty);
            }
            else
            {
                writer.WriteValue(((decimal)value).ToString(CultureInfo.InvariantCulture));
            }
        }
    }
}
