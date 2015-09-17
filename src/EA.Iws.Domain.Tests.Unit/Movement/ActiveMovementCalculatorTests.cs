namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Domain.Movement;
    using EA.Prsd.Core;
    using System;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ActiveMovementCalculatorTests
    {
        private readonly ActiveMovementCalculator activeMovementCalculator;
        private readonly TestableMovement movement;

        public ActiveMovementCalculatorTests()
        {
            activeMovementCalculator = new ActiveMovementCalculator();
            movement = new TestableMovement();
        }

        [Fact]
        public void ReturnsTrueWhenDateInPast()
        {
            SystemTime.Freeze(new DateTime(2015, 9, 1));

            movement.Date = new DateTime(2015, 7, 1);

            Assert.True(activeMovementCalculator.IsActive(movement));

            SystemTime.Unfreeze();
        }

        [Fact]
        public void ReturnsFalseIfNoDate()
        {
            Assert.False(activeMovementCalculator.IsActive(movement));
        }

        [Fact]
        public void ReturnsFalseWhenDateInFuture()
        {
            SystemTime.Freeze(new DateTime(2015, 9, 1));

            movement.Date = new DateTime(2015, 9, 2);

            Assert.False(activeMovementCalculator.IsActive(movement));

            SystemTime.Unfreeze();
        }

        [Fact]
        public void ReturnsTotalActiveMovements()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            var movements = new[]
            {
                new TestableMovement
                {
                    Date = new DateTime(2014, 9, 7)
                },
                new TestableMovement
                {
                    Date = new DateTime(2014, 11, 15)
                },
                new TestableMovement
                {
                    Date = new DateTime(2015, 2, 4)
                }
            };

            Assert.Equal(2, activeMovementCalculator.TotalActiveMovements(movements));

            SystemTime.Unfreeze();
        }
    }
}
