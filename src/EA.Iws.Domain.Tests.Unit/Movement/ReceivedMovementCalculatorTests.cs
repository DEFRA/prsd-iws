namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.MovementReceipt;
    using Domain.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceivedMovementCalculatorTests
    {
        private readonly ReceivedMovementCalculator receivedMovementCalculator;
        private readonly Movement movement1;
        private readonly Movement movement2;
        private readonly Movement[] movements;

        public ReceivedMovementCalculatorTests()
        {
            movement1 = new TestableMovement
            {
                Receipt = new TestableMovementReceipt
                {
                    Date = new DateTime(2015, 9, 1),
                    Decision = Decision.Accepted,
                    Quantity = 5
                }
            };

            movement2 = new TestableMovement
            {
                Receipt = new TestableMovementReceipt
                {
                    Date = new DateTime(2015, 9, 1),
                    Decision = Decision.Rejected,
                    RejectReason = "Lost at sea."
                }
            };

            movements = new[] { movement1, movement2 };

            receivedMovementCalculator = new ReceivedMovementCalculator();
        }

        [Fact]
        public void IsReceived_True_WhenAllFieldsComplete()
        {
            Assert.True(receivedMovementCalculator.IsReceived(movement1));
        }

        [Fact]
        public void IsReceived_False_WhenRejected()
        {
            Assert.False(receivedMovementCalculator.IsReceived(movement2));
        }

        [Fact]
        public void ReceivedMovements_ReturnsCorrectMovements()
        {
            Assert.Equal(1, receivedMovementCalculator.ReceivedMovements(movements).Count);
        }
    }
}
