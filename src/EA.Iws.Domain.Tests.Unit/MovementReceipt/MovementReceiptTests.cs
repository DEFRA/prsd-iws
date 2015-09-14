namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using Core.MovementReceipt;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using System;
    using Xunit;

    public class MovementReceiptTests
    {
        private readonly Movement movement;
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime MovementDate = new DateTime(2015, 12, 1);
        private static readonly DateTime BeforeMovementDate = new DateTime(2015, 10, 1);
        private static readonly DateTime AfterMovementDate = new DateTime(2016, 1, 1);
        private static readonly string AnyString = "text";

        public MovementReceiptTests()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
            movement = new Movement(notification, 1);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, MovementDate, movement);
        }

        [Fact]
        public void CanReceiveMovementWithDateProvided()
        {
            movement.Receive(AfterMovementDate);

            Assert.NotNull(movement.Receipt);
            Assert.Equal(AfterMovementDate, movement.Receipt.Date);
        }

        [Fact]
        public void MovementReceivedBeforeMovementDate_Throws()
        {
            Assert.Throws<InvalidOperationException>(() => movement.Receive(BeforeMovementDate));
        }

        [Fact]
        public void MovementAlreadyReceived_Throws()
        {
            movement.Receive(AfterMovementDate);

            Assert.Throws<InvalidOperationException>(() => movement.Receive(AfterMovementDate));
        }

        [Fact]
        public void MovementNotYetActive_Throws()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, null, movement);

            Assert.Throws<InvalidOperationException>(() => movement.Receive(AnyDate));
        }

        [Fact]
        public void AcceptThrowsIfNoReceipt()
        {
            Assert.Throws<InvalidOperationException>(() => movement.Accept());
        }

        [Fact]
        public void RejectThrowsIfNoReceipt()
        {
            Assert.Throws<InvalidOperationException>(() => movement.Reject(AnyString));
        }

        [Fact]
        public void CanAcceptMovement()
        {
            movement.Receive(AfterMovementDate);

            movement.Accept();

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Accepted, movement.Receipt.Decision);
        }

        [Fact]
        public void CanRejectMovement()
        {
            movement.Receive(AfterMovementDate);

            movement.Reject(AnyString);

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Rejected, movement.Receipt.Decision);
        }

        [Fact]
        public void RejectedMovementMustHaveReason()
        {
            movement.Receive(AfterMovementDate);

            movement.Reject(AnyString);

            Assert.Equal(AnyString, movement.Receipt.RejectReason);
        }
    }
}
