namespace EA.Iws.Web.Tests.Unit.ViewModels.Movement
{
    using System;
    using Areas.Movement.ViewModels.Complete;
    using Prsd.Core;
    using TestHelpers;
    using Xunit;

    public class DateCompleteViewModelTests : IDisposable
    {
        private static readonly DateTime Today = new DateTime(2017, 1, 1);

        public DateCompleteViewModelTests()
        {
            SystemTime.Freeze(Today);
        }

        [Theory]
        [InlineData(2015, 1, 1)]
        [InlineData(2016, 2, 29)]
        public void ValidDateCombinationAllowed(int year, int month, int day)
        {
            var viewModel = CreateViewModel(year, month, day);

            Assert.Empty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Theory]
        [InlineData(2015, 9, 31)]
        [InlineData(2015, 2, 29)]
        public void InvalidDateCombinationDisallowed(int year, int month, int day)
        {
            var viewModel = CreateViewModel(year, month, day);

            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Fact]
        public void AllowsToday()
        {
            var viewModel = CreateViewModel(Today.Year, Today.Month, Today.Day);

            Assert.Empty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Fact]
        public void AllowsOlderDates()
        {
            var olderThanToday = Today.AddMonths(-1);

            var viewModel = CreateViewModel(olderThanToday.Year, olderThanToday.Month, olderThanToday.Day);

            Assert.Empty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Fact]
        public void DisallowsFutureDates()
        {
            var futureDate = Today.AddMonths(1);

            var viewModel = CreateViewModel(futureDate.Year, futureDate.Month, futureDate.Day);

            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        private DateViewModel CreateViewModel(int year, int month, int day)
        {
            return new DateViewModel
            {
                Day = day,
                Month = month,
                Year = year
            };
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}
