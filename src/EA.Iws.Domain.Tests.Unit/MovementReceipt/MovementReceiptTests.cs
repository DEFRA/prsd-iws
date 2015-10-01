namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using Core.MovementReceipt;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using System;
    using Core.Shared;
    using Xunit;
    using NotificationType = Domain.NotificationApplication.NotificationType;

    public class MovementReceiptTests
    {
        private readonly Movement movement;
        private readonly MovementReceipt movementReceipt;
        private readonly ReceivedMovements receivedMovementService;

        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime MovementDate = new DateTime(2015, 12, 1);
        private static readonly DateTime BeforeMovementDate = new DateTime(2015, 10, 1);
        private static readonly DateTime AfterMovementDate = new DateTime(2016, 1, 1);
        private static readonly string AnyString = "text";

        public MovementReceiptTests()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationType.Recovery, UKCompetentAuthority.England, 0);
            movement = new Movement(1, notification.Id);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, MovementDate, movement);
            movementReceipt = new MovementReceipt(AfterMovementDate);
            receivedMovementService = new ReceivedMovements(new ActiveMovements());
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
        public void MovementNotYetActive_Throws()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, null, movement);

            Assert.Throws<InvalidOperationException>(() => movement.Receive(AnyDate));
        }

        [Fact]
        public void CanAcceptMovement()
        {
            movement.Receive(AfterMovementDate);

            movement.Receipt.Accept();

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Accepted, movement.Receipt.Decision);
        }

        [Fact]
        public void CanRejectMovement()
        {
            movement.Receive(AfterMovementDate);

            movement.Receipt.Reject(AnyString);

            Assert.NotNull(movement.Receipt.Decision);
            Assert.Equal(Decision.Rejected, movement.Receipt.Decision);
        }

        [Fact]
        public void RejectedMovementMustHaveReason()
        {
            movement.Receive(AfterMovementDate);

            movement.Receipt.Reject(AnyString);

            Assert.Equal(AnyString, movement.Receipt.RejectReason);
        }

        [Fact]
        public void SetQuantity_ReceiptNotAccepted_Throws()
        {
            Action action =
                () => movementReceipt.SetQuantity(10m);

            Assert.Throws<InvalidOperationException>(action);
        }

        [Fact]
        public void SetQuantity_ReceiptAccepted_Sets()
        {
            ObjectInstantiator<MovementReceipt>.SetProperty(x => x.Decision, Decision.Accepted, movementReceipt);

            movementReceipt.SetQuantity(10m);

            Assert.Equal(10, movementReceipt.Quantity);
        }

        [Fact]
        public void ReceiveMoreThanOnceUpdatesReceiptDate()
        {
            movement.Receive(AfterMovementDate);

            var newDate = AfterMovementDate.AddDays(1);

            movement.Receive(newDate);

            Assert.Equal(newDate, movement.Receipt.Date);
        }

        [Fact]
        public void ReceiveMoreThanOnceDoesNotCreateNewReceipt()
        {
            movement.Receive(AfterMovementDate);

            var newDate = AfterMovementDate.AddDays(1);

            movement.Receipt.Accept();

            movement.Receive(newDate);

            Assert.NotNull(movement.Receipt.Decision);
        }

        [Fact]
        public void MovementIsReceivedWhenEverythingIsComplete()
        {
            movement.Receive(AfterMovementDate);

            movement.Receipt.Accept();

            movement.Receipt.SetQuantity(10m);

            Assert.True(movement.IsReceived);
        }

        [Fact]
        public void IncompleteMovement_IsNotReceived()
        {
            movement.Receive(AfterMovementDate);

            Assert.False(movement.IsReceived);
        }

        [Fact]
        public void RejectedMovement_IsNotReceived()
        {
            movement.Receive(AfterMovementDate);

            movement.Receipt.Reject(AnyString);

            Assert.False(movement.IsReceived);
        }

        [Fact]
        public void CanSetCertificateFile()
        {
            var fileId = new Guid("5665B9BA-8B32-44C6-93F1-161765F9DF66");

            movement.Receive(AfterMovementDate);

            movement.Receipt.SetCertificateFile(fileId);

            Assert.Equal(fileId, movement.Receipt.FileId);
        }
    }
}
