namespace EA.Iws.Web.Tests.Unit.ViewModels
{
    using System;
    using System.Linq;
    using Areas.NotificationApplication.ViewModels.Shipment;
    using Core.NotificationAssessment;
    using Core.Shared;
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
                Units = ShipmentQuantityUnits.Tonnes,
                NumberOfShipments = "1",
                Quantity = "1"
            };
            SystemTime.Freeze(new DateTime(2015, 5, 1));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Theory]
        [InlineData("1.1")]
        [InlineData("0")]
        [InlineData("100000")]
        public void NumberOfShipmentsValidationTests(string v)
        {
            AddValidDateToModel();
            model.NumberOfShipments = v;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, errors.Count);
            Assert.Equal("Please enter a valid number between 1 and 99999", errors[0].ErrorMessage);
        }

        [Fact]
        public void NumberOfShipmentsCanContainCommas()
        {
            AddValidDateToModel();
            model.NumberOfShipments = "1,000";

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(0, errors.Count);
        }

        [Theory]
        [InlineData("1000.1234")]
        [InlineData("1,000.1234")]
        [InlineData("1000.12")]
        [InlineData("1000123400")]
        [InlineData("1,000,123,400")]
        public void QuantyContainValidData(string v)
        {
            AddValidDateToModel();
            model.Quantity = v;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(0, errors.Count);
        }

        [Theory]
        [InlineData("1000.12345")]
        public void QuantityValidationTests(string v)
        {
            AddValidDateToModel();
            model.Quantity = v;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Equal(1, errors.Count);
            Assert.Equal("Please enter a valid number with a maximum of 4 decimal places", errors[0].ErrorMessage);
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
        public void StartDateCantBeInPastForNotSubmitted()
        {
            model.Status = NotificationStatus.NotSubmitted;

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
        public void StartDateCanBeInPastForSubmitted()
        {
            model.Status = NotificationStatus.Submitted;

            model.StartDay = 1;
            model.StartMonth = 1;
            model.StartYear = 2015;

            model.EndDay = 1;
            model.EndMonth = 12;
            model.EndYear = 2015;

            var errors = ViewModelValidator.ValidateViewModel(model);

            Assert.Empty(errors);
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

            Assert.True(errors.Any(p => p.ErrorMessage == "The first departure date and last departure date must be within a 12 month period"));
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

            Assert.True(errors.Any(p => p.ErrorMessage.Contains("The first departure date and last departure date must be within a 36 month period")));
        }

        private void AddValidDateToModel()
        {
            model.IsPreconsentedRecoveryFacility = true;

            model.StartDay = 2;
            model.StartMonth = 5;
            model.StartYear = 2015;

            model.EndDay = 1;
            model.EndMonth = 5;
            model.EndYear = 2016;
        }
    }
}