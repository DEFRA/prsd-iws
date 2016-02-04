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
    using CompetentAuthorityEnum = Core.Notification.UKCompetentAuthority;

    public class MovementTests : IDisposable
    {
        private readonly Movement movement;

        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime FrozenTime = new DateTime(2015, 9, 1);
        private static readonly DateTime BeforeFrozenTime = new DateTime(2015, 7, 1);
        private static readonly DateTime AfterFrozenTime = new DateTime(2015, 9, 2);

        public MovementTests()
        {
            var notification = new NotificationApplication(
                Guid.NewGuid(), 
                NotificationType.Recovery, 
                CompetentAuthorityEnum.England, 
                0);

            movement = new Movement(1, notification.Id, AnyDate);

            SystemTime.Freeze(FrozenTime);
        }

        [Fact]
        public void HasShipped_MovementSubmitted_ShipmentDateInPast_ReturnsTrue()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, BeforeFrozenTime, movement);

            Assert.True(movement.HasShipped);
        }

        [Fact]
        public void HasShipped_MovementSubmitted_ShipmentDateInFuture_ReturnsFalse()
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, MovementStatus.Submitted, movement);
            ObjectInstantiator<Movement>.SetProperty(x => x.Date, AfterFrozenTime, movement);

            Assert.False(movement.HasShipped);
        }

        [Theory]
        [InlineData(MovementStatus.New)]
        [InlineData(MovementStatus.Cancelled)]
        [InlineData(MovementStatus.Captured)]
        [InlineData(MovementStatus.Received)]
        [InlineData(MovementStatus.Rejected)]
        [InlineData(MovementStatus.Completed)]
        public void HasShipped_MovementNotSubmitted_ReturnsFalse(MovementStatus status)
        {
            ObjectInstantiator<Movement>.SetProperty(x => x.Status, status, movement);

            Assert.False(movement.HasShipped);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}