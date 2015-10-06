namespace EA.Iws.Web.Tests.Unit.Extensions
{
    using System;
    using Infrastructure;
    using Xunit;

    public class StringExtensionsTests
    {
        [Theory]
        [InlineData("10")]
        [InlineData("10.5")]
        [InlineData("10.57")]
        [InlineData("100")]
        [InlineData("1000")]
        [InlineData("1,000")]
        [InlineData("1,234.5")]
        [InlineData("12,345.67")]
        [InlineData("123,456")]
        [InlineData("1,234,567.89")]
        public void ValidMoneyDecimalPasses(string value)
        {
            Assert.True(value.IsValidMoneyDecimal());
        }

        [Theory]
        [InlineData("")]
        [InlineData("text")]
        [InlineData("1.234")]
        [InlineData("1,23")]
        [InlineData("1234,56")]
        [InlineData("1234,567.00")]
        public void InvalidMoneyDecimalFails(string value)
        {
            Assert.False(value.IsValidMoneyDecimal());
        }

        [Fact]
        public void ValidMoneyDecimal_ConvertsToDecimal()
        {
            var value = "123,456.77";

            Assert.Equal(123456.77m, value.ToMoneyDecimal());
        }

        [Fact]
        public void InvalidMoneyDecimal_Throws()
        {
            var invalidValue = "123,45";

            Assert.Throws<ArgumentException>(() => invalidValue.ToMoneyDecimal());
        }
    }
}
