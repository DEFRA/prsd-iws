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
    public class RecoveryDateTests
    {
        private Movement movement;
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");
        private static readonly Guid NotificationId = new Guid("675287E3-42D3-4A58-86D4-691ECF620671");
        private static readonly Guid MovementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private static readonly DateTime Today = new DateTime(2017, 07, 19);
        private static readonly DateTime PastDate = Today.AddDays(-2);
        private static readonly DateTime FutureDate = Today.AddDays(2);

        public RecoveryDateTests()
        {
            SystemTime.Freeze(Today);
        }

        [Fact]
        public void WasteRecoveredDateCanBeInThePast()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Received,
                Receipt = new MovementReceipt(PastDate, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };
           
            movement.CompleteInternally(PastDate, userId);

            Assert.Equal(PastDate, movement.CompletedReceipt.Date);
        }

        [Fact]
        public void WasteRecoveredDateCanBeToday()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Received,
                Receipt = new MovementReceipt(Today, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };

            movement.CompleteInternally(Today, userId);

            Assert.Equal(Today, movement.CompletedReceipt.Date);
        }

        [Fact]
        public void WasteRecoveredDateBeforeWasteReceivedDate_Throws()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = Today,
                NotificationId = NotificationId,
                Status = MovementStatus.Received,
                Receipt = new MovementReceipt(Today, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };

            Assert.Throws<InvalidOperationException>(() => movement.CompleteInternally(PastDate, userId));
        }

        [Fact]
        public void WasteRecoveredDateSameasWasteReceivedDate()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = PastDate,
                NotificationId = NotificationId,
                Status = MovementStatus.Received,
                Receipt = new MovementReceipt(PastDate, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };

            movement.CompleteInternally(PastDate, userId);

            Assert.Equal(PastDate, movement.CompletedReceipt.Date);
        }

        [Fact]
        public void WasteRecoveredDateInfuture_Throws()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = Today,
                NotificationId = NotificationId,
                Status = MovementStatus.Received,
                Receipt = new MovementReceipt(Today, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };

            Assert.Throws<InvalidOperationException>(() => movement.CompleteInternally(FutureDate, userId));
        }

        private void SetMovementStatus(MovementStatus status)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);
        }
    }
}
