namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using EA.Iws.Core.Shared;
    using EA.Iws.Domain.Movement;
    using RequestHandlers.MovementReceipt;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementQuantityCalculatorTests
    {
        private readonly MovementQuantityCalculator movementQuantityCalculator;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;

        public MovementQuantityCalculatorTests()
        {
            movement = new TestableMovement();

            movement.Units = ShipmentQuantityUnits.Kilograms;

            movement.Receipt = new TestableMovementReceipt
            {
                Date = new DateTime(2015, 9, 1),
                Decision = Core.MovementReceipt.Decision.Accepted,
                Quantity = 5
            };

            shipmentInfo = new TestableShipmentInfo
            {
                Quantity = 20,
                Units = ShipmentQuantityUnits.Kilograms
            };

            movementQuantityCalculator = new MovementQuantityCalculator(new ReceivedMovementService());
        }

        [Fact]
        public void ReturnsCorrectQuantityReceived()
        {
            var movements = new[] { movement };

            Assert.Equal(5, movementQuantityCalculator.QuantityReceived(movements));
        }

        [Fact]
        public void ReturnsCorrectQuantityRemaining()
        {
            var movements = new[] { movement };

            Assert.Equal(15, movementQuantityCalculator.QuantityRemaining(shipmentInfo, movements));
        }

        [Fact]
        public void QuantityReceived_IfMovementsUnitsDiffer_Throws()
        {
            var movementWithOtherUnits = new TestableMovement
            {
                Units = ShipmentQuantityUnits.Tonnes,
                Receipt = new TestableMovementReceipt
                {
                    Date = new DateTime(2015, 9, 2),
                    Decision = Core.MovementReceipt.Decision.Accepted,
                    Quantity = 0.01m
                }
            };

            var movements = new[] { movement, movementWithOtherUnits };

            Assert.Throws<InvalidOperationException>(() => 
                movementQuantityCalculator.QuantityReceived(movements));
        }

        [Fact]
        public void QuantityRemaining_IfMovementsAndNotificationShipmentUnitsDiffer_Throws()
        {
            shipmentInfo.Units = ShipmentQuantityUnits.Tonnes;

            var movements = new[] { movement };

            Assert.Throws<InvalidOperationException>(() =>
                movementQuantityCalculator.QuantityRemaining(shipmentInfo, movements));
        }

        [Fact]
        public void QuantityReceived_OnlyCountsReceivedMovements()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { movement, nonReceivedMovement };

            Assert.Equal(5, movementQuantityCalculator.QuantityReceived(movements));
        }

        [Fact]
        public void QuantityRemaining_OnlyCountsReceivedMovements()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { movement, nonReceivedMovement };

            Assert.Equal(15, movementQuantityCalculator.QuantityRemaining(shipmentInfo, movements));
        }

        [Fact]
        public void QuantityReceived_Zero_WhenNoMovementsReceived()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { nonReceivedMovement };

            Assert.Equal(0, movementQuantityCalculator.QuantityReceived(movements));
        }

        [Fact]
        public void QuantityRemaining_Unchanged_WhenNoMovementsReceived()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { nonReceivedMovement };

            Assert.Equal(20, movementQuantityCalculator.QuantityRemaining(shipmentInfo, movements));
        }
    }
}
