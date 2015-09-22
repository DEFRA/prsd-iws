namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using EA.Iws.Core.Shared;
    using EA.Iws.Domain.Movement;
    using RequestHandlers.MovementReceipt;
    using System;
    using System.Threading.Tasks;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementQuantityTests
    {
        private readonly MovementQuantity movementQuantity;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;

        public MovementQuantityTests()
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

            movementQuantity = new MovementQuantity(new ReceivedMovements());
        }

        [Fact]
        public void ReturnsCorrectQuantityReceived()
        {
            var movements = new[] { movement };

            Assert.Equal(5, movementQuantity.Received(shipmentInfo, movements));
        }

        [Fact]
        public void ReturnsCorrectQuantityRemaining()
        {
            var movements = new[] { movement };

            Assert.Equal(15, movementQuantity.Remaining(shipmentInfo, movements));
        }

        [Fact]
        public void QuantityReceived_IfMovementsUnitsDiffer_ConvertsAndSums()
        {
            var movementWithOtherUnits = new TestableMovement
            {
                Units = ShipmentQuantityUnits.Tonnes,
                Receipt = new TestableMovementReceipt
                {
                    Date = new DateTime(2015, 9, 2),
                    Decision = Core.MovementReceipt.Decision.Accepted,
                    Quantity = 0.001m
                }
            };

            var movements = new[] { movement, movementWithOtherUnits };

            Assert.Equal(6, movementQuantity.Received(shipmentInfo, movements));
        }

        [Fact]
        public void QuantityRemaining_IfMovementsAndNotificationShipmentUnitsDiffer_ConvertsToNotificationUnits()
        {
            shipmentInfo.Units = ShipmentQuantityUnits.Tonnes;

            var movements = new[] { movement };

            Assert.Equal(0.005m, movementQuantity.Received(shipmentInfo, movements));
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

            Assert.Equal(5, movementQuantity.Received(shipmentInfo, movements));
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

            Assert.Equal(15, movementQuantity.Remaining(shipmentInfo, movements));
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

            Assert.Equal(0, movementQuantity.Received(shipmentInfo, movements));
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

            Assert.Equal(20, movementQuantity.Remaining(shipmentInfo, movements));
        }

        [Fact]
        public void QuantityRemaining_ReturnedInShipmentInfoUnits()
        {
            shipmentInfo.Units = ShipmentQuantityUnits.Tonnes;

            var movements = new[] { movement };

            Assert.Equal(19.995m, movementQuantity.Remaining(shipmentInfo, movements));
        }
    }
}
