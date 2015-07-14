namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.Shipment;
    using Core.Shipment;
    using Prsd.Core;
    using TestHelpers;
    using Xunit;

    public class ShipmentInfoViewModelTests : IDisposable
    {
        private readonly ShipmentInfoViewModel model;

        public ShipmentInfoViewModelTests()
        {
            model = new ShipmentInfoViewModel
            {
                Units = ShipmentQuantityUnits.Kilograms,
                NumberOfShipments = "1",
                Quantity = 1
            };
            SystemTime.Freeze(new DateTime(2015, 5, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void StartDateCantBeInvalid()
        {
            model.StartDay = 31;
            model.StartMonth = 2;
            model.StartYear = 2016;

            model.EndDay = 31;
            model.EndMonth = 3;
            model.EndYear = 2016;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.True(errors.Any(p => p.ErrorMessage == "Please enter a valid first departure date"));
        }

        [Fact]
        public void EndDateCantBeInvalid()
        {
            model.StartDay = 1;
            model.StartMonth = 1;
            model.StartYear = 2016;

            model.EndDay = 31;
            model.EndMonth = 2;
            model.EndYear = 2016;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.True(errors.Any(p => p.ErrorMessage == "Please enter a valid last departure date"));
        }

        [Fact]
        public void StartDateCantBeInPast()
        {
            model.StartDay = 1;
            model.StartMonth = 1;
            model.StartYear = 2015;

            model.EndDay = 1;
            model.EndMonth = 2;
            model.EndYear = 2016;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.True(errors.Any(p => p.ErrorMessage == "The first departure date cannot be in the past"));
        }

        [Fact]
        public void StartDateMustBeBeforeEndDate()
        {
            model.StartDay = 1;
            model.StartMonth = 1;
            model.StartYear = 2016;

            model.EndDay = 1;
            model.EndMonth = 8;
            model.EndYear = 2015;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.True(errors.Any(p => p.ErrorMessage == "The first departure date must be before the last departure date"));
        }

        [Fact]
        public void NonPreconsentedNotificationDatesCantBeExactly12Months()
        {
            model.IsPreconsentedRecoveryFacility = false;

            model.StartDay = 1;
            model.StartMonth = 1;
            model.StartYear = 2016;

            model.EndDay = 1;
            model.EndMonth = 1;
            model.EndYear = 2017;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.True(errors.Any(p => p.ErrorMessage == "The first departure date and last departure date must be within a 12 month period."));
        }

        [Fact]
        public void PreconsentedNotificationDatesCantBeExactly36Months()
        {
            model.IsPreconsentedRecoveryFacility = true;

            model.StartDay = 1;
            model.StartMonth = 1;
            model.StartYear = 2016;

            model.EndDay = 1;
            model.EndMonth = 1;
            model.EndYear = 2019;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.True(errors.Any(p => p.ErrorMessage == "The first departure date and last departure date must be within a 36 month period."));
        }
    }
}