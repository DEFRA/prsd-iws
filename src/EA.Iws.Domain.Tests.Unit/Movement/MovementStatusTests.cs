﻿namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using Core.Movement;
    using Domain.Movement;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementStatusTests
    {
        private readonly Movement movement;
        private static readonly Guid AnyGuid = new Guid("B68806E3-524E-476E-A505-40B717B3191E");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");

        public MovementStatusTests()
        {
            movement = CreateMovement();
        }

        private void SetMovementStatus(MovementStatus status, Movement movement)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);
        }

        [Fact]
        public void DefaultStatusIsNew()
        {
            Assert.Equal(MovementStatus.New, movement.Status);
        }

        [Fact]
        public void SubmitChangesStatusToSubmitted()
        {
            movement.Submit(AnyGuid);

            Assert.Equal(MovementStatus.Submitted, movement.Status);
        }
        
        [Fact]
        public void CantSubmitTwice()
        {
            movement.Submit(AnyGuid);
            Action submitAgain = () => movement.Submit(AnyGuid);

            Assert.Throws<InvalidOperationException>(submitAgain);
        }

        [Fact]
        public void SubmitRaisesStatusChangedEvent()
        {
            movement.Submit(AnyGuid);

            Assert.Equal(movement,
                movement.Events.OfType<MovementStatusChangeEvent>()
                    .SingleOrDefault()
                    .Movement);
        }

        [Fact]
        public void CanCancelSubmittedMovement()
        {
            SetMovementStatus(MovementStatus.Submitted, movement);

            movement.Cancel();

            Assert.Equal(MovementStatus.Cancelled, movement.Status);
        }

        [Fact]
        public void CompleteChangesStatusToCompleted()
        {
            SetMovementStatus(MovementStatus.Received, movement);

            movement.Complete(AnyDate, AnyGuid, userId);

            Assert.Equal(MovementStatus.Completed, movement.Status);
        }

        [Fact]
        public void CompleteCreatesMovementCompletedReceipt()
        {
            SetMovementStatus(MovementStatus.Received, movement);

            movement.Complete(AnyDate, AnyGuid, userId);

            Assert.NotNull(movement.CompletedReceipt);
        }

        [Theory]
        [InlineData(MovementStatus.New)]
        [InlineData(MovementStatus.Rejected)]
        [InlineData(MovementStatus.Cancelled)]
        [InlineData(MovementStatus.Submitted)]
        public void CannotCompleteNonReceivedMovement(MovementStatus status)
        {
            SetMovementStatus(status, movement);

            Assert.Throws<InvalidOperationException>(() => movement.Complete(AnyDate, AnyGuid, userId));
        }

        private Movement CreateMovement()
        {
            var notificationId = new Guid("EAD34BEE-E962-4D4D-9D53-ADCD7240C333");
            return new Movement(1, notificationId, AnyDate, userId);
        }
    }
}