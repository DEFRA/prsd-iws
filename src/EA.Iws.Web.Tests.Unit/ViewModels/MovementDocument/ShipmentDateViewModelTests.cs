namespace EA.Iws.Web.Tests.Unit.ViewModels.MovementDocument
{
    using System;
    using Areas.MovementDocument.ViewModels;
    using Prsd.Core;
    using Requests.Movement;
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
        public void DateCannotBeInThePastTest()
        {
            var model = GetViewModel(new DateTime(2015, 3, 2));

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, errors.Count);
            Assert.Equal("The shipment date cannot be in the past", errors[0].ErrorMessage);
        }

        [Fact]
        public void DateCannotBeBeforeFirstDateTest()
        {
            var model = GetViewModel(new DateTime(2015, 1, 1));

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(2, errors.Count);
            Assert.Equal("The date must be between 01.02.2015 and 01.01.2016", errors[0].ErrorMessage);
        }

        [Fact]
        public void DateCannotBeAfterLastDateTest()
        {
            var model = GetViewModel(new DateTime(2017, 1, 1));

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, errors.Count);
            Assert.Equal("The date must be between 01.02.2015 and 01.01.2016", errors[0].ErrorMessage);
        }

        private ShipmentDateViewModel GetViewModel(DateTime actualDate)
        {
            var movementData = new MovementDatesData
            {
                ActualDate = actualDate,
                FirstDate = new DateTime(2015, 2, 1),
                LastDate = new DateTime(2016, 1, 1)
            };

            return new ShipmentDateViewModel(movementData);
        }
    }
}
