using System;
using System.IO;
using Newtonsoft.Json;
using VPay.Payment.Api;
using Xunit;

namespace VPay.Payment.Api.Tests
{
    public class DecimalJsonConverterTests
    {
        private readonly JsonSerializerSettings _settings;

        public DecimalJsonConverterTests()
        {
            _settings = new JsonSerializerSettings
            {
                Converters = new[] { new DecimalJsonConverter() }
            };
        }

        [Theory]
        [InlineData("123.45", 123.45)]
        [InlineData("0", 0)]
        [InlineData("-123.45", -123.45)]
        public void ReadJson_ValidDecimalString_ShouldReturnDecimal(string input, decimal expected)
        {
            // Act
            var result = JsonConvert.DeserializeObject<decimal>(input, _settings);

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("")]
        public void ReadJson_EmptyOrNullString_ShouldReturnNull(string input)
        {
            // Arrange
            var json = input == null ? "null" : $"\"{input}\"";

            // Act
            var result = JsonConvert.DeserializeObject<decimal?>(json, _settings);

            // Assert
            Assert.Null(result);
        }

        [Theory]
        [InlineData(123.45, "123.45")]
        [InlineData(0, "0")]
        [InlineData(-123.45, "-123.45")]
        public void WriteJson_ValidDecimal_ShouldWriteDecimalString(decimal input, string expected)
        {
            // Arrange
            var obj = new { Value = input };

            // Act
            var json = JsonConvert.SerializeObject(obj, _settings);

            // Assert
            Assert.Contains($"\"Value\":\"{expected}\"", json);
        }

        [Fact]
        public void WriteJson_NullDecimal_ShouldWriteEmptyString()
        {
            // Arrange
            var obj = new { Value = (decimal?)null };

            // Act
            var json = JsonConvert.SerializeObject(obj, _settings);

            // Assert
            Assert.Contains("{\"Value\":null}", json);
        }

        [Fact]
        public void ReadJson_InvalidTokenType_ShouldThrowJsonSerializationException()
        {
            // Arrange
            var json = "{\"Value\":{}}";

            // Act & Assert
            Assert.Throws<JsonSerializationException>(() => JsonConvert.DeserializeObject<decimal>(json, _settings));
        }
    }
}
