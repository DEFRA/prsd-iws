namespace EA.Iws.Web.Tests.Unit.ViewModels.Movement
{
    using System;
    using Areas.NotificationMovements.ViewModels.Create;
    using Core.Movement;
    using Prsd.Core;
    using TestHelpers;
    using Xunit;

    public class ShipmentDateViewModelTests : IDisposable
    {
        public ShipmentDateViewModelTests()
        {
            SystemTime.Freeze(new DateTime(2015, 5, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void DateCannotBeBeforeFirstDateTest()
        {
            var model = GetViewModel(new DateTime(2015, 1, 1));

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, errors.Count);
            Assert.Equal("The date is not within the given range", errors[0].ErrorMessage);
        }

        [Fact]
        public void DateCannotBeAfterLastDateTest()
        {
            var model = GetViewModel(new DateTime(2017, 1, 1));

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, errors.Count);
            Assert.Equal("The date is not within the given range", errors[0].ErrorMessage);
        }

        [Fact]
        public void DateHintTextIsCorrect()
        {
            var model = GetViewModel(new DateTime(2017, 1, 1));

            Assert.Equal("For example, 1 5 2015", model.DateHintText);
        }

        private ShipmentDateViewModel GetViewModel(DateTime? actualDate)
        {
            var shipmentDates = new ShipmentDates
            {
                StartDate = new DateTime(2015, 2, 1),
                EndDate = new DateTime(2016, 1, 1)
            };

            var shipmentDateViewModel = new ShipmentDateViewModel(shipmentDates, new[] { 10 });

            if (actualDate.HasValue)
            {
                shipmentDateViewModel.Day = actualDate.Value.Day;
                shipmentDateViewModel.Month = actualDate.Value.Month;
                shipmentDateViewModel.Year = actualDate.Value.Year;
            }

            return shipmentDateViewModel;
        }
    }
}