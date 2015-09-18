namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.MovementReceipt;
    using Core.Shared;
    using Domain.Movement;
    using Domain.MovementReceipt;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;
    using NotificationApplicationType = Domain.NotificationApplication.NotificationType;

    public class MovementTests : IDisposable
    {
        private readonly Movement movement;

        private static readonly DateTime FrozenTime = new DateTime(2015, 9, 1);
        private static readonly DateTime BeforeFrozenTime = new DateTime(2015, 7, 1);
        private static readonly DateTime AfterFrozenTime = new DateTime(2015, 9, 2);

        public MovementTests()
        {
            var notification = new NotificationApplication(
                Guid.NewGuid(), 
                NotificationApplicationType.Recovery, 
                UKCompetentAuthority.England, 
                0);

            movement = new Movement(notification, 1);

            SystemTime.Freeze(FrozenTime);
        }

        [Fact]
        public void IsActive_ReturnsTrue_WhenDateInPast()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, BeforeFrozenTime, movement);

            Assert.True(movement.IsActive);
        }

        [Fact]
        public void IsActive_ReturnsFalse_IfNoDate()
        {
            Assert.False(movement.IsActive);
        }
        
        [Fact]
        public void IsActive_ReturnsFalse_WhenDateInFuture()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, AfterFrozenTime, movement);

            Assert.False(movement.IsActive);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        private void FullyReceiveMovement()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, BeforeFrozenTime, movement);
            ObjectInstantiator<Movement>.SetProperty(x => x.Units, ShipmentQuantityUnits.Tonnes, movement);

            movement.Receive(FrozenTime);
            movement.Accept();
            movement.Receipt.SetQuantity(5.0m, ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes);
        }
    }
}