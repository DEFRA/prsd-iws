namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Prsd.Core;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;
    public class ReceiptDateTests
    {
        private Movement movement;
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");
        private static readonly Guid NotificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid MovementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);

        public ReceiptDateTests()
        {
            SystemTime.Freeze(Today);
        }

        [Fact]
        public void WasteReceivedDateCanBeInThePast()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            movement.ReceiveInternally(PastDate, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId);

            Assert.Equal(PastDate, movement.Receipt.Date);
        }

        [Fact]
        public void WasteReceivedDateCanBeToday()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            movement.ReceiveInternally(Today, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId);

            Assert.Equal(Today, movement.Receipt.Date);
        }

        [Fact]
        public void WasteReceivedDateBeforeActualShipmentDate_Throws()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = FutureDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            Assert.Throws<InvalidOperationException>(() => movement.ReceiveInternally(Today, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId));
        }

        [Fact]
        public void WasteReceivedDateSameasActualShipmentDate()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            movement.ReceiveInternally(PastDate, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId);

            Assert.Equal(PastDate, movement.Receipt.Date);
        }

        [Fact]
        public void WasteReceivedDateInfuture_Throws()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = Today,
                NotificationId = NotificationId,
                Status = MovementStatus.Captured
            };

            Assert.Throws<InvalidOperationException>(() => movement.ReceiveInternally(FutureDate, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId));
        }

        private void SetMovementStatus(MovementStatus status)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);
        }
    }
}
