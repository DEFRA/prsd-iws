namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using Core.MovementReceipt;
    using Domain.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class ReceivedMovementServiceTests
    {
        private readonly ReceivedMovementService receivedMovementService;
        private readonly Movement movement1;
        private readonly Movement movement2;
        private readonly Movement[] movements;

        public ReceivedMovementServiceTests()
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

            receivedMovementService = new ReceivedMovementService();
        }

        [Fact]
        public void ReceivedMovements_ReturnsCorrectMovements()
        {
            Assert.Equal(1, receivedMovementService.ReceivedMovements(movements).Count);
        }
    }
}
