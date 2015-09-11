namespace EA.Iws.Web.Tests.Unit.ViewModels.MovementDocument
{
    using System;
    using Areas.Movement.ViewModels;
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
        public void DateHIntTextIsCorrect()
        {
            var model = GetViewModel(new DateTime(2017, 1, 1));

            Assert.Equal("For example, 1 5 2015", model.DateHintText);
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
