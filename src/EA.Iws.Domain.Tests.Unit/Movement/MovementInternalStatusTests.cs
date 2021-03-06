﻿namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using System;
    using TestHelpers.DomainFakes;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementInternalStatusTests
    {
        private static readonly DateTime Date = new DateTime(2015, 7, 7);
        private readonly Movement movement;
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");
        private static readonly Guid MovementId = new Guid("DD7A435F-BB74-4EA6-8680-C1811C46500A");
        private Movement movementForRecoveredTest;
        public MovementInternalStatusTests()
        {
            movement = new Movement(1, Guid.Empty, new DateTime(2015, 1, 1), userId);
            SetMovementStatus(MovementStatus.Captured);
        }

        [Fact]
        public void Captured_ToSubmitted()
        {
            movement.SubmitInternally(Date);

            Assert.Equal(MovementStatus.Submitted, movement.Status);
            Assert.Equal(Date, movement.PrenotificationDate.GetValueOrDefault());
        }

        [Fact]
        public void Captured_ToReceived()
        {
            movement.ReceiveInternally(Date, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId);

            Assert.Equal(MovementStatus.Received, movement.Status);
            Assert.Equal(Date, movement.Receipt.Date);
            Assert.Equal(10, movement.Receipt.QuantityReceived.Quantity);
            Assert.Equal(ShipmentQuantityUnits.Kilograms, movement.Receipt.QuantityReceived.Units);
        }

        [Fact]
        public void Captured_CannotBeSubmittedExternally()
        {
            Action submitExternally = 
                () => movement.Submit(new Guid("F306419A-80C8-4589-91A1-7B9E8D32374A"));

            Assert.Throws<InvalidOperationException>(submitExternally);
        }

        [Fact]
        public void Captured_ToSubmitted_ThenReceived()
        {
            movement.SubmitInternally(Date);

            movement.Receive(new Guid("2A24F5A5-000F-437D-9C00-9EA01EDD0668"), Date.AddDays(5), new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId);

            Assert.Equal(MovementStatus.Received, movement.Status);
            Assert.Equal(Date, movement.PrenotificationDate);
            Assert.Equal(Date.AddDays(5), movement.Receipt.Date);
        }

        [Fact]
        public void Received_ToComplete()
        {
            movementForRecoveredTest = new TestableMovement
            {
                Id = MovementId,
                Date = Date,
                NotificationId = Guid.Empty,
                Status = MovementStatus.Received,
                Receipt = new MovementReceipt(Date, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };

            movementForRecoveredTest.CompleteInternally(Date, userId);

            Assert.Equal(MovementStatus.Completed, movementForRecoveredTest.Status);
            Assert.Equal(Date, movementForRecoveredTest.CompletedReceipt.Date);
        }

        [Theory]
        [InlineData(MovementStatus.Completed)]
        [InlineData(MovementStatus.Cancelled)]
        [InlineData(MovementStatus.Captured)]
        [InlineData(MovementStatus.New)]
        [InlineData(MovementStatus.Rejected)]
        [InlineData(MovementStatus.Submitted)]
        public void CanOnlyComplete_FromReceived(MovementStatus status)
        {           
            movementForRecoveredTest = new TestableMovement
            {
                Id = MovementId,
                Date = Date,
                NotificationId = Guid.Empty,
                Receipt = new MovementReceipt(Date, new ShipmentQuantity(10, ShipmentQuantityUnits.Kilograms), userId)
            };

            SetMovementStatusForRecovered(status);

            Action complete = () => movementForRecoveredTest.CompleteInternally(Date, userId);

            Assert.Throws<InvalidOperationException>(complete);
        }

        private void SetMovementStatus(MovementStatus status)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);
        }
        private void SetMovementStatusForRecovered(MovementStatus status)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movementForRecoveredTest);
        }
    }
}
