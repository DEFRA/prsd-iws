namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
{
    using System.Linq;
    using Core.MeansOfTransport;
    using DocumentGeneration.Formatters;
    using Xunit;

    public class MeansOfTransportFormatterTests
    {
        private readonly MeansOfTransportFormatter formatter = new MeansOfTransportFormatter();

        [Fact]
        public void NullInputReturnsEmptyString()
        {
            var result = formatter.MeansOfTransportAsString(null);

            Assert.Equal(string.Empty, result);
        }
        
        [Fact]
        public void EmptyInputReturnsEmptyString()
        {
            var result = formatter.MeansOfTransportAsString(new MeansOfTransport[0].OrderBy(m => m));

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void OneInputReturnsExpectedResult()
        {
            var result = formatter.MeansOfTransportAsString(new[]
            {
                MeansOfTransport.Air
            }.OrderBy(m => m));

            Assert.Equal("A", result);
        }

        [Fact]
        public void TwoInputsReturnsDelimitedList()
        {
            var result = formatter.MeansOfTransportAsString(new[]
            {
                MeansOfTransport.Road,
                MeansOfTransport.Air
            }.OrderBy(m => m.Symbol));

            Assert.Equal("A-R", result);
        }

        [Fact]
        public void MultipleInputsReturnsOrderedOutput()
        {
            var result = formatter.MeansOfTransportAsString(new[]
            {
                MeansOfTransport.Road,
                MeansOfTransport.Air,
                MeansOfTransport.Sea
            }.OrderBy(m => m.Symbol));

            Assert.Equal("A-R-S", result);
        }
    }
}
