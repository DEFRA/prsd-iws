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

        private static readonly DateTime beforeStartDate = new DateTime(2014, 1, 1);
        private static readonly DateTime startDate = new DateTime(2015, 1, 1);
        private static readonly DateTime validDate = new DateTime(2015, 2, 2);
        private static readonly DateTime endDate = new DateTime(2016, 1, 1);
        private static readonly DateTime afterEndDate = new DateTime(2017, 1, 1);

        private readonly TestableShipmentInfo shipmentInfo;
        private readonly Movement movement;
        private readonly SetActualDateOfShipment dateSetter;

        public UpdateMovementTests()
        {
            shipmentInfo = new TestableShipmentInfo
            {
                ShipmentPeriod = new ShipmentPeriod(startDate, endDate, true),
                Quantity = 520,
                Units = ShipmentQuantityUnits.Tonnes,
                NotificationId = NotificationId
            };

            movement = new Movement(5, NotificationId);
            dateSetter = new SetActualDateOfShipment();

            SystemTime.Freeze(new DateTime(2015, 06, 01));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        [Fact]
        public void ShipmentInfoAndMovementNotificationIdsDiffer_Throws()
        {
            shipmentInfo.NotificationId = Guid.NewGuid();

            Action updateMovementDate = () => dateSetter.Apply(validDate, movement, shipmentInfo);

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }

        [Fact]
        public void DateCannotBeBeforeStartDate()
        {
            Action updateMovementDate = () => dateSetter.Apply(beforeStartDate, movement, shipmentInfo);

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }

        [Fact]
        public void DateCannotBeAfterTheEndDate()
        {
            Action updateMovementDate = () => dateSetter.Apply(afterEndDate, movement, shipmentInfo);

            Assert.Throws<InvalidOperationException>(updateMovementDate);
        }

        [Fact]
        public void ValidDateAndMatchingNotifications_SetsDate()
        {
            dateSetter.Apply(validDate, movement, shipmentInfo);

            Assert.Equal(validDate, movement.Date);
        }

        [Fact]
        public void SetQuantity_SetsDisplayUnitsToPassedValue()
        {
            movement.SetQuantity(new ShipmentQuantity(10, ShipmentQuantityUnits.Tonnes));

            AssertQuantity(10, ShipmentQuantityUnits.Tonnes);
        }

        [Fact]
        public void SetQuantity_ConvertsQuantity_SetsUnitsCorrectly()
        {
            movement.SetQuantity(new ShipmentQuantity(100, ShipmentQuantityUnits.Kilograms));

            AssertQuantity(100, ShipmentQuantityUnits.Kilograms);
        }

        private void AssertQuantity(decimal quantity, ShipmentQuantityUnits unit)
        {
            Assert.Equal(quantity, movement.Quantity);
            Assert.Equal(unit, movement.Units);
        }
    }
}
