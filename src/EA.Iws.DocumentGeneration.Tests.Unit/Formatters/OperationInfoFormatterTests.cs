namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
{
    using DocumentGeneration.Formatters;
    using Domain.NotificationApplication;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class OperationInfoFormatterTests
    {
        private readonly OperationInfoFormatter formatter = new OperationInfoFormatter();

        [Fact]
        public void NullEnumerableReturnsEmptyString()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(null);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void EmptyEnumerableReturnsEmptyString()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new OperationInfo[0]);

            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void OneItemsReturnsCorrectString()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.R1 }
            });

            Assert.Equal("R1", result);
        }

        [Fact]
        public void TwoItemsReturnsCorrectString()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.R1 },
                new TestableOperationInfo { OperationCode = OperationCode.R2 }
            });

            Assert.Equal("R1, R2", result);
        }

        [Fact]
        public void TwoItemsUnorderedReturnsOrderedString()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.D12 },
                new TestableOperationInfo { OperationCode = OperationCode.D3 },
                new TestableOperationInfo { OperationCode = OperationCode.D7 },
            });

            Assert.Equal("D3, D7, D12", result);
        }
    }
}
