namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
{
    using DocumentGeneration.Formatters;
    using Xunit;

    public class WasteRecoveryFormatterTests
    {
        private readonly WasteRecoveryFormatter formatter = new WasteRecoveryFormatter();

        [Fact]
        public void NullableDecimalAsPercentage_NoDecimalPlaces_ReturnsCorrectString()
        {
            var result = formatter.NullableDecimalAsPercentage(97m);

            Assert.Equal("97%", result);
        }

        [Fact]
        public void NullableDecimalAsPercentage_OneDecimalPlace_ReturnsCorrectString()
        {
            var result = formatter.NullableDecimalAsPercentage(69.3m);

            Assert.Equal("69.3%", result);
        }

        [Fact]
        public void NullableDecimalAsPercentage_Null_ReturnsEmptyString()
        {
            var result = formatter.NullableDecimalAsPercentage(null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void NullableDecimalAsPercentage_FiveDecimalPlaces_ReturnsCorrectString()
        {
            var result = formatter.NullableDecimalAsPercentage(25.33773m);

            Assert.Equal("25.33773%", result);
        }

        [Fact]
        public void NullableDecimalAsPercentage_BlankDecimalPlaces_ReturnsRoundedString()
        {
            var result = formatter.NullableDecimalAsPercentage(69.00m);

            Assert.Equal("69%", result);
        }
    }
}
