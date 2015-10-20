namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

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
                NotificationType.Recovery, 
                UKCompetentAuthority.England, 
                0);

            movement = new Movement(1, notification.Id);

            SystemTime.Freeze(FrozenTime);
        }

        [Fact]
        public void IsActive_ReturnsTrue_WhenDateInPast()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, BeforeFrozenTime, movement);

            Assert.True(movement.HasShipped);
        }

        [Fact]
        public void IsActive_ReturnsFalse_IfNoDate()
        {
            Assert.False(movement.HasShipped);
        }
        
        [Fact]
        public void IsActive_ReturnsFalse_WhenDateInFuture()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, AfterFrozenTime, movement);

            Assert.False(movement.HasShipped);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }

        private void FullyReceiveMovement()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, BeforeFrozenTime, movement);
            ObjectInstantiator<Movement>.SetProperty(x => x.Units, ShipmentQuantityUnits.Tonnes, movement);

            movement.Receive(Guid.NewGuid(), FrozenTime, 5.0m);
            movement.Receipt.Accept();
        }
    }
}