namespace EA.Iws.Web.Tests.Unit.ViewModels.Movement
{
    using System;
    using Areas.ExportMovement.ViewModels;
    using Prsd.Core;
    using TestHelpers;
    using Xunit;

    public class DateReceivedViewModelTests : IDisposable
    {
        private static readonly DateTime MovementDate = new DateTime(2015, 9, 1);

        public DateReceivedViewModelTests()
        {
            SystemTime.Freeze(new DateTime(2016, 3, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Theory]
        [InlineData(1, 1, 2016)]
        [InlineData(29, 2, 2016)]
        public void ValidDateCombinationPasses(int day, int month, int year)
        {
            var viewModel = new DateReceivedViewModel
            {
                MovementDate = MovementDate,
                Day = day,
                Month = month,
                Year = year
            };

            Assert.Empty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Theory]
        [InlineData(31, 9, 2017)]
        [InlineData(29, 2, 2017)]
        public void InvalidDateCombinationFails(int day, int month, int year)
        {
            var viewModel = new DateReceivedViewModel
            {
                MovementDate = MovementDate,
                Day = day,
                Month = month,
                Year = year
            };

            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Fact]
        public void DateBeforeMovementDateFails()
        {
            var viewModel = new DateReceivedViewModel
            {
                MovementDate = MovementDate,
                Day = 1,
                Month = 8,
                Year = 2015
            };

            Assert.NotEmpty(ViewModelValidator.ValidateViewModel(viewModel));
        }

        [Fact]
        public void DateAfterMovementDatePasses()
        {
            var viewModel = new DateReceivedViewModel
            {
                MovementDate = MovementDate,
                Day = 1,
                Month = 1,
                Year = 2016
            };

            Assert.Empty(ViewModelValidator.ValidateViewModel(viewModel));
        }
    }
}
