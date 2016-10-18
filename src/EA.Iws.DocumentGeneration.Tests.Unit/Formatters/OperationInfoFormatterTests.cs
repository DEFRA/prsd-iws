namespace EA.Iws.DocumentGeneration.Tests.Unit.Formatters
{
    using Core.OperationCodes;
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
                new TestableOperationInfo { OperationCode = OperationCode.R11 }
            });

            Assert.Equal("R1, R11", result);
        }

        [Fact]
        public void TwoItemsUnorderedReturnsOrderedString()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.D11 },
                new TestableOperationInfo { OperationCode = OperationCode.D3 },
                new TestableOperationInfo { OperationCode = OperationCode.D7 },
            });

            Assert.Equal("D3, D7, D11", result);
        }

        [Fact]
        public void OneR_Interim_OrdersFirst()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.R5 },
                new TestableOperationInfo { OperationCode = OperationCode.R1 },
                new TestableOperationInfo { OperationCode = OperationCode.R12 },
            });

            Assert.Equal("R12, R1, R5", result);
        }

        [Fact]
        public void TwoR_Interims_OrdersFirst()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.R5 },
                new TestableOperationInfo { OperationCode = OperationCode.R13 },
                new TestableOperationInfo { OperationCode = OperationCode.R12 },
            });

            Assert.Equal("R12, R13, R5", result);
        }

        [Fact]
        public void OneD_Interim_OrdersFirst()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.D5 },
                new TestableOperationInfo { OperationCode = OperationCode.D1 },
                new TestableOperationInfo { OperationCode = OperationCode.D12 },
            });

            Assert.Equal("D12, D1, D5", result);
        }

        [Fact]
        public void ThreeD_Interims_OrdersFirst()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.D5 },
                new TestableOperationInfo { OperationCode = OperationCode.D15 },
                new TestableOperationInfo { OperationCode = OperationCode.D14 },
                new TestableOperationInfo { OperationCode = OperationCode.D12 }
            });

            Assert.Equal("D12, D14, D15, D5", result);
        }

        [Fact]
        public void AllD_Interims_OrdersFirst()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.D5 },
                new TestableOperationInfo { OperationCode = OperationCode.D4 },
                new TestableOperationInfo { OperationCode = OperationCode.D7 },
                new TestableOperationInfo { OperationCode = OperationCode.D13 },
                new TestableOperationInfo { OperationCode = OperationCode.D2 },
                new TestableOperationInfo { OperationCode = OperationCode.D10 },
                new TestableOperationInfo { OperationCode = OperationCode.D3 },
                new TestableOperationInfo { OperationCode = OperationCode.D1 },
                new TestableOperationInfo { OperationCode = OperationCode.D15 },
                new TestableOperationInfo { OperationCode = OperationCode.D8 },
                new TestableOperationInfo { OperationCode = OperationCode.D12 },
                new TestableOperationInfo { OperationCode = OperationCode.D9 },
                new TestableOperationInfo { OperationCode = OperationCode.D11 },
                new TestableOperationInfo { OperationCode = OperationCode.D14 },
                new TestableOperationInfo { OperationCode = OperationCode.D6 }
            });

            Assert.Equal("D12, D13, D14, D15, D1, D2, D3, D4, D5, D6, D7, D8, D9, D10, D11", result);
        }

        [Fact]
        public void ALLR_Interim_OrdersFirst()
        {
            var result = formatter.OperationInfosToCommaDelimitedList(new[]
            {
                new TestableOperationInfo { OperationCode = OperationCode.R5 },
                new TestableOperationInfo { OperationCode = OperationCode.R1 },
                new TestableOperationInfo { OperationCode = OperationCode.R12 },
                new TestableOperationInfo { OperationCode = OperationCode.R9 },
                new TestableOperationInfo { OperationCode = OperationCode.R6 },
                new TestableOperationInfo { OperationCode = OperationCode.R8 },
                new TestableOperationInfo { OperationCode = OperationCode.R3 },
                new TestableOperationInfo { OperationCode = OperationCode.R7 },
                new TestableOperationInfo { OperationCode = OperationCode.R10 },
                new TestableOperationInfo { OperationCode = OperationCode.R4 },
                new TestableOperationInfo { OperationCode = OperationCode.R13 },
                new TestableOperationInfo { OperationCode = OperationCode.R2 },
                new TestableOperationInfo { OperationCode = OperationCode.R11 },
            });

            Assert.Equal("R12, R13, R1, R2, R3, R4, R5, R6, R7, R8, R9, R10, R11", result);
        }
    }
}
