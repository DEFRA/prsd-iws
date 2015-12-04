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
        public void ValidShipment_ResultIsValid()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);

            var result = validator.Validate(shipment);

            Assert.True(result.IsValid);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1)]
        [InlineData(0)]
        public void InvalidTotalShipments_HasValidationError(int? input)
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.TotalShipments = input;

            validator.ShouldHaveValidationErrorFor(x => x.TotalShipments, shipment);
        }

        [Theory]
        [InlineData(null)]
        [InlineData(-1.0)]
        [InlineData(0.0)]
        public void InvalidQuantity_HasValidationError(double? input)
        {
            var decimalInput = Convert.ToDecimal(input);
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.Quantity = decimalInput;

            validator.ShouldHaveValidationErrorFor(x => x.Quantity, shipment);
        }

        [Fact]
        public void InvalidUnit_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.Unit = null;

            validator.ShouldHaveValidationErrorFor(x => x.Unit, shipment);
        }

        [Fact]
        public void StartDateNull_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = null;

            validator.ShouldHaveValidationErrorFor(x => x.StartDate, shipment);
        }

        [Fact]
        public void EndDateNull_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.EndDate = null;

            validator.ShouldHaveValidationErrorFor(x => x.EndDate, shipment);
        }

        [Fact]
        public void EndDateBeforeStartDate_HasValidationError()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = new DateTime(2015, 12, 31);
            shipment.EndDate = new DateTime(2015, 12, 1);

            validator.ShouldHaveValidationErrorFor(x => x.EndDate, shipment);
        }

        [Fact]
        public void PreconsentedFacilities_ShipmentPeriodCanBe3Years()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2018, 11, 30);

            validator.ShouldNotHaveValidationErrorFor(x => x.EndDate, shipment);
        }

        [Fact]
        public void PreconsentedFacilities_ShipmentPeriodCantBeOver3Years()
        {
            var shipment = GetValidShipment(ImportNotificationPreconsentedId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2018, 12, 1);

            validator.ShouldHaveValidationErrorFor(x => x.EndDate, shipment);
        }

        [Theory]
        [MemberData("NonpreconsentedFacilities")]
        public void NonpreconsentedFacilities_ShipmentPeriodCantBeOver1Year(Guid importNotificationId)
        {
            var shipment = GetValidShipment(importNotificationId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2016, 12, 1);

            validator.ShouldHaveValidationErrorFor(x => x.EndDate, shipment);
        }

        [Theory]
        [MemberData("NonpreconsentedFacilities")]
        public void NonpreconsentedFacilities_ShipmentPeriodCanBe1Year(Guid importNotificationId)
        {
            var shipment = GetValidShipment(importNotificationId);
            shipment.StartDate = new DateTime(2015, 12, 1);
            shipment.EndDate = new DateTime(2016, 11, 30);

            validator.ShouldNotHaveValidationErrorFor(x => x.EndDate, shipment);
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