namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using Core.MovementReceipt;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Domain.NotificationApplication;
    using System;
    using Xunit;

    public class MovementReceiptTests
    {
        private readonly Movement movement;
        private static readonly DateTime AnyDate = new DateTime(2015, 12, 31);
        private static readonly string AnyString = "text";

        public MovementReceiptTests()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
            movement = new Movement(notification, 1);
        }

        [Fact]
        public void CanReceiveMovementWithDateProvided()
        {
            movement.Receive(AnyDate);

            Assert.NotNull(movement.Receipt);
            Assert.Equal(AnyDate, movement.Receipt.Date);
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
            movement.Receive(AnyDate);

            movement.Accept();

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Accepted, movement.Receipt.Decision);
        }

        [Fact]
        public void CanRejectMovement()
        {
            movement.Receive(AnyDate);

            movement.Reject(AnyString);

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Rejected, movement.Receipt.Decision);
        }

        [Fact]
        public void RejectedMovementMustHaveReason()
        {
            movement.Receive(AnyDate);

            movement.Reject(AnyString);

            Assert.Equal(AnyString, movement.Receipt.RejectReason);
        }
    }
}
