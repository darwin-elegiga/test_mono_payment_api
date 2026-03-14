using System;
using Xunit;
using VPay.Payment.Common;

namespace VPay.Payment.Common.Tests.Helpers
{
    public class StringHelpersTests
    {
        [Theory]
        [InlineData("Hello", "hello")]
        [InlineData("hello", "hello")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void FirstCharacterToLower_ShouldConvertFirstCharacterToLower(string input, string expected)
        {
            // Act
            var result = input.FirstCharacterToLower();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("1-800-123-4567", "8001234567")]
        [InlineData("(800) 123-4567", "8001234567")]
        [InlineData("800.123.4567", "8001234567")]
        [InlineData("800 123 4567", "8001234567")]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void CleanFaxNumber_ShouldRemoveSpecialCharacters(string input, string expected)
        {
            // Act
            var result = input.CleanFaxNumber();

            // Assert
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Hello\u2013World", "Hello-World")]
        [InlineData("Hello\u2014World", "Hello-World")]
        [InlineData("Hello\u2015World", "Hello-World")]
        [InlineData("Hello\u2017World", "Hello_World")]
        [InlineData("Hello\u2018World", "Hello'World")]
        [InlineData("Hello\u2019World", "Hello'World")]
        [InlineData("Hello\u201aWorld", "Hello,World")]
        [InlineData("Hello\u201bWorld", "Hello'World")]
        [InlineData("Hello\u201cWorld", "Hello\"World")]
        [InlineData("Hello\u201dWorld", "Hello\"World")]
        [InlineData("Hello\u201eWorld", "Hello\"World")]
        [InlineData("Hello\u2026World", "Hello...World")]
        [InlineData("Hello\u2032World", "Hello'World")]
        [InlineData("Hello\u2033World", "Hello\"World")]
        [InlineData("", "")]
        [InlineData(null, null)]
        public void CleanWordValues_ShouldReplaceSpecialCharacters(string input, string expected)
        {
            // Act
            var result = input.CleanWordValues();

            // Assert
            Assert.Equal(expected, result);
        }
    }
}