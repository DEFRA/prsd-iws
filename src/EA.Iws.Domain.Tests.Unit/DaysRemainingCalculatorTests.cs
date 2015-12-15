namespace EA.Iws.Domain.Tests.Unit
{
    using System;
    using Prsd.Core;
    using Xunit;

    public class DaysRemainingCalculatorTests : IDisposable
    {
        private readonly DaysRemainingCalculator calculator;
        private static readonly DateTime Today = new DateTime(2015, 1, 1);

        public DaysRemainingCalculatorTests()
        {
            calculator = new DaysRemainingCalculator();

            SystemTime.Freeze(Today);
        }

        [Fact]
        public void ReturnsNumberOfDaysWhenPositive()
        {
            Assert.Equal("5", calculator.Calculate(Today.AddDays(5)));
        }

        [Fact]
        public void ReturnsNumberOfDaysWhenZero()
        {
            Assert.Equal("0", calculator.Calculate(Today));
        }

        [Fact]
        public void ReturnsCorrectNumberWhenTimeInvolved()
        {
            SystemTime.Freeze(Today.AddHours(5).AddMinutes(30));
            Assert.Equal("3", calculator.Calculate(Today.AddDays(3).AddHours(5)));
        }

        [Fact]
        public void ReturnsOverdueWhenNegative()
        {
            Assert.Equal("Overdue", calculator.Calculate(Today.AddDays(-1)));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}