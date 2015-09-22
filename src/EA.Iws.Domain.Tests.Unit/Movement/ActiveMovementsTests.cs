namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using Domain.Movement;
    using EA.Prsd.Core;
    using System;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ActiveMovementsTests
    {
        private readonly ActiveMovements activeMovements;
        private readonly TestableMovement movement;

        public ActiveMovementsTests()
        {
            activeMovements = new ActiveMovements();
            movement = new TestableMovement();
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

            Assert.Equal(2, activeMovements.Total(movements));

            SystemTime.Unfreeze();
        }
    }
}
