namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Domain.Movement;
    using EA.Prsd.Core;
    using System;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ActiveMovementTests
    {
        private readonly ActiveMovement activeMovement;
        private readonly TestableMovement movement;

        public ActiveMovementTests()
        {
            activeMovement = new ActiveMovement();
            movement = new TestableMovement();
        }

        [Fact]
        public void ReturnsTrueWhenDateInPast()
        {
            SystemTime.Freeze(new DateTime(2015, 9, 1));

            movement.Date = new DateTime(2015, 7, 1);

            Assert.True(activeMovement.IsActive(movement));

            SystemTime.Unfreeze();
        }

        [Fact]
        public void ReturnsFalseIfNoDate()
        {
            Assert.False(activeMovement.IsActive(movement));
        }

        [Fact]
        public void ReturnsFalseWhenDateInFuture()
        {
            SystemTime.Freeze(new DateTime(2015, 9, 1));

            movement.Date = new DateTime(2015, 9, 2);

            Assert.False(activeMovement.IsActive(movement));

            SystemTime.Unfreeze();
        }
    }
}
