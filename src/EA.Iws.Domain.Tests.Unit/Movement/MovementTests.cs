namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.Movement;
    using Core.Notification;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core;
    using TestHelpers.Helpers;
    using Xunit;

    public class MovementTests : IDisposable
    {
        private readonly Movement movement;

        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private static readonly DateTime FrozenTime = new DateTime(2015, 9, 1);
        private static readonly DateTime BeforeFrozenTime = new DateTime(2015, 7, 1);
        private static readonly DateTime AfterFrozenTime = new DateTime(2015, 9, 2);
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");

        public MovementTests()
        {
            var notification = new NotificationApplication(
                Guid.NewGuid(),
                NotificationType.Recovery,
                UKCompetentAuthority.England,
                0);

            movement = new Movement(1, notification.Id, AnyDate, userId);

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

        [Fact]
        public void CanSetComments()
        {
            movement.SetComments("testing");

            Assert.Equal("testing", movement.Comments);
        }

        [Fact]
        public void CommentsCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => movement.SetComments(null));
        }

        [Fact]
        public void CommentsCantBeEmpty()
        {
            Assert.Throws<ArgumentException>(() => movement.SetComments(string.Empty));
        }

        [Fact]
        public void CanSetStatsMarking()
        {
            movement.SetStatsMarking("testing");

            Assert.Equal("testing", movement.StatsMarking);
        }

        [Fact]
        public void StatsMarkingCantBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => movement.SetStatsMarking(null));
        }

        [Fact]
        public void StatsMarkingCantBeEmpty()
        {
            Assert.Throws<ArgumentException>(() => movement.SetStatsMarking(string.Empty));
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}