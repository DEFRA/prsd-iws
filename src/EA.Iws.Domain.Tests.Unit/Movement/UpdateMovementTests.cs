namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.Shared;
    using Domain.Movement;
    using Prsd.Core;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class UpdateMovementTests : IDisposable
    {
        private static readonly Guid UserId = new Guid("35745EEC-55E7-42F1-9D8E-3515AC6FA281");
        private static readonly Guid NotificationId = new Guid("28760D3F-E18F-4986-BC7E-06BCD72D554C");
        private static readonly DateTime startDate = new DateTime(2015, 1, 1);
        private static readonly DateTime endDate = new DateTime(2016, 1, 1);

        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly Movement movement;

        public UpdateMovementTests()
        {
            shipmentInfo = new TestableShipmentInfo
            {
                FirstDate = startDate,
                LastDate = endDate,
                Quantity = 520,
                Units = ShipmentQuantityUnits.Tonnes
            };

            notificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId,
                ShipmentInfo = shipmentInfo
            };

            movement = new Movement(notificationApplication, 5);

            SystemTime.Freeze(new DateTime(2015, 06, 01));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void DateCannotBeBeforeStartDate()
        {
            Action updateMovementDate = () => movement.UpdateDate(new DateTime(2014, 1, 1));

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }

        [Fact]
        public void DateCannotBeAfterTheEndDate()
        {
            Action updateMovementDate = () => movement.UpdateDate(new DateTime(2017, 1, 1));

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }

        [Fact]
        public void SetQuantity_SetsDisplayUnitsToPassedValue()
        {
            movement.SetQuantity(10, ShipmentQuantityUnits.Tonnes);

            AssertQuantity(10, ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Tonnes);
        }

        [Fact]
        public void SetQuantity_ConvertsQuantity_SetsUnitsCorrectly()
        {
            movement.SetQuantity(100, ShipmentQuantityUnits.Kilograms);

            AssertQuantity(0.1m, ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms);
        }

        private void AssertQuantity(decimal quantity, ShipmentQuantityUnits unit, ShipmentQuantityUnits displayUnit)
        {
            Assert.Equal(quantity, movement.Quantity);
            Assert.Equal(unit, movement.Units);
            Assert.Equal(displayUnit, movement.DisplayUnits);
        }
    }
}
