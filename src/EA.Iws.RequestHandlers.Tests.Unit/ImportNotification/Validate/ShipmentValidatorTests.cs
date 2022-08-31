namespace EA.Iws.RequestHandlers.Tests.Unit.ImportNotification.Validate
{
    using System;
    using System.Collections.Generic;
    using Core.ImportNotification.Draft;
    using Core.Shared;
    using DataAccess.Draft;
    using FakeItEasy;
    using FluentValidation.TestHelper;
    using Prsd.Core;
    using RequestHandlers.ImportNotification.Validate;
    using Xunit;

    public class ShipmentValidatorTests : IDisposable
    {
        private readonly ShipmentValidator validator;
        private static readonly Guid ImportNotificationPreconsentedId = new Guid("36E1E901-8331-4414-9AFE-0ECC6FB49B8F");
        private static readonly Guid ImportNotificationNonPreconsentedId = new Guid("7CEDB8F8-9A53-4B7A-BC56-BF890EC32A74");
        private static readonly Guid ImportNotificationNoPreconsentedDataId = new Guid("D0BF468C-C7A9-4FE6-912E-321464B0ECC4");

        public ShipmentValidatorTests()
        {
            var draftRepo = A.Fake<IDraftImportNotificationRepository>();

            A.CallTo(() => draftRepo.GetDraftData<Preconsented>(ImportNotificationPreconsentedId))
                .Returns(new Preconsented(ImportNotificationPreconsentedId) { AllFacilitiesPreconsented = true });

            A.CallTo(() => draftRepo.GetDraftData<Preconsented>(ImportNotificationNonPreconsentedId))
                .Returns(new Preconsented(ImportNotificationNonPreconsentedId) { AllFacilitiesPreconsented = false });

            A.CallTo(() => draftRepo.GetDraftData<Preconsented>(ImportNotificationNoPreconsentedDataId))
                .Returns(new Preconsented(ImportNotificationNoPreconsentedDataId) { AllFacilitiesPreconsented = null });

            SystemTime.Freeze(new DateTime(2015, 12, 1));

            validator = new ShipmentValidator(draftRepo);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        private static Shipment GetValidShipment(Guid importNotificationId)
        {
            var shipment = new Shipment(importNotificationId)
            {
                TotalShipments = 10,
                Quantity = 1000.0000M,
                Unit = ShipmentQuantityUnits.Tonnes,
                StartDate = new DateTime(2015, 12, 1),
                EndDate = new DateTime(2016, 11, 30)
            };
            return shipment;
        }

        [Fact]
        public async void ValidShipment_ResultIsValid()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);

            var result = await validator.ValidateAsync(shipment);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        public async void InvalidTotalShipments_HasValidationError(int? input)
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.TotalShipments = input;

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.TotalShipments);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1.0)]
        [InlineData(0.0)]
        public async void InvalidQuantity_HasValidationError(double? input)
        {
            var decimalInput = Convert.ToDecimal(input);
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.Quantity = decimalInput;

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.Quantity);
        }

        [Fact]
        public async void InvalidUnit_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.Unit = null;

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.Unit);
        }

        [Fact]
        public async void StartDateNull_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = null;

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.StartDate);
        }

        [Fact]
        public async void EndDateNull_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.EndDate = null;

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.EndDate);
        }

        [Fact]
        public async void EndDateBeforeStartDate_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = new DateTime(2015, 12, 31);
            shipment.EndDate = new DateTime(2015, 12, 1);

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.EndDate);
        }

        [Fact]
        public async void PreconsentedFacilities_ShipmentPeriodCanBe3Years()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2018, 11, 30);

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldNotHaveValidationErrorFor(s => s.EndDate);
        }

        [Fact]
        public async void PreconsentedFacilities_ShipmentPeriodCantBeOver3Years()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2018, 12, 1);

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.EndDate);
        }

        [Theory]
        [MemberData(nameof(NonpreconsentedFacilities))]
        public async void NonpreconsentedFacilities_ShipmentPeriodCantBeOver1Year(Guid importNotificationId)
        {
            var shipment = GetValidShipment(importNotificationId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2016, 12, 1);

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldHaveValidationErrorFor(s => s.EndDate);
        }

        [Theory]
        [MemberData(nameof(NonpreconsentedFacilities))]
        public async void NonpreconsentedFacilities_ShipmentPeriodCanBe1Year(Guid importNotificationId)
        {
            var shipment = GetValidShipment(importNotificationId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2016, 11, 30);

            var result = await validator.TestValidateAsync(shipment);
            result.ShouldNotHaveValidationErrorFor(s => s.EndDate);
        }

        public static IEnumerable<object[]> NonpreconsentedFacilities()
        {
            return new[]
            {
                new object[] { ImportNotificationNonPreconsentedId },
                new object[] { ImportNotificationNoPreconsentedDataId }
            };
        }
    }
}