namespace EA.Iws.Domain.Tests.Unit.MovementOperationReceipt
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Core.Shared;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Domain.NotificationApplication;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationType = Domain.NotificationApplication.NotificationType;

    public class MovementOperationReceiptTests
    {
        private readonly Movement movement;

        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime MovementDate = new DateTime(2015, 12, 1);
        private static readonly DateTime AfterMovementDate = new DateTime(2016, 1, 1);
        private static readonly DateTime AfterReceiptDate = new DateTime(2016, 2, 1);

        public MovementOperationReceiptTests()
        {
            var notification = new NotificationApplication(Guid.NewGuid(), NotificationApplicationType.Recovery, UKCompetentAuthority.England, 0);

            movement = new Movement(notification, 1);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, MovementDate, movement);
            ObjectInstantiator<Movement>.SetProperty(x => x.Units, ShipmentQuantityUnits.Kilograms, movement);
        }

        [Fact]
        public void CompleteMovementOperation_WhenNotReceived_Throws()
        {
            Assert.Throws<InvalidOperationException>(() =>
                movement.CompleteMovement(AnyDate));
        }

        [Fact]
        public void CanComplete_IfMovementReceived()
        {
            FullyReceiveMovement();

            movement.CompleteMovement(AfterReceiptDate);

            Assert.NotNull(movement.Receipt.OperationReceipt);
        }

        private void FullyReceiveMovement()
        {
            movement.Receive(AfterMovementDate);
            movement.Accept();
            movement.Receipt.SetQuantity(5.0m, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes);
        }
    }
}
