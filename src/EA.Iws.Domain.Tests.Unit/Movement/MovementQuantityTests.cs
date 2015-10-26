namespace EA.Iws.Domain.Tests.Unit.MovementReceipt
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementQuantityTests
    {
        private static readonly Guid NotificationId = new Guid("4B2015E0-5CCC-45C0-B904-F6CC49741E16");
        private readonly NotificationMovementsQuantity movementQuantity;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;

        public MovementQuantityTests()
        {
            movement = new TestableMovement
            {
                Units = ShipmentQuantityUnits.Kilograms,
                Status = MovementStatus.Received,
                Receipt = new TestableMovementReceipt
                {
                    Date = new DateTime(2015, 9, 1),
                    Decision = Core.MovementReceipt.Decision.Accepted,
                    Quantity = 5
                }
            };

            shipmentInfo = new TestableShipmentInfo
            {
                Quantity = 20,
                Units = ShipmentQuantityUnits.Kilograms
            };

            movementRepository = A.Fake<IMovementRepository>();
            shipmentRepository = A.Fake<IShipmentInfoRepository>();

            movementQuantity = new NotificationMovementsQuantity(movementRepository, shipmentRepository);
        }

        private void SetUpRepositoryCalls(Movement[] movements, ShipmentInfo shipment)
        {
            A.CallTo(() => movementRepository.GetMovementsByStatus(NotificationId, MovementStatus.Received)).Returns(movements.Where(m => m.Status == MovementStatus.Received));
            A.CallTo(() => movementRepository.GetMovementsByStatus(NotificationId, MovementStatus.Completed)).Returns(movements.Where(m => m.Status == MovementStatus.Completed));
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);
        }

        [Fact]
        public async Task ReturnsCorrectQuantityReceived()
        {
            var movements = new[] { movement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Received(NotificationId);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task ReturnsCorrectQuantityRemaining()
        {
            var movements = new[] { movement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Remaining(NotificationId);

            Assert.Equal(15, result);
        }

        [Fact]
        public async Task QuantityReceived_IfMovementsUnitsDiffer_ConvertsAndSums()
        {
            var movementWithOtherUnits = new TestableMovement
            {
                Units = ShipmentQuantityUnits.Tonnes,
                Status = MovementStatus.Received,
                Receipt = new TestableMovementReceipt
                {
                    Date = new DateTime(2015, 9, 2),
                    Decision = Core.MovementReceipt.Decision.Accepted,
                    Quantity = 0.001m
                }
            };

            var movements = new[] { movement, movementWithOtherUnits };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Received(NotificationId);

            Assert.Equal(6, result);
        }

        [Fact]
        public async Task QuantityRemaining_IfMovementsAndNotificationShipmentUnitsDiffer_ConvertsToNotificationUnits()
        {
            shipmentInfo.Units = ShipmentQuantityUnits.Tonnes;

            var movements = new[] { movement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Remaining(NotificationId);

            Assert.Equal(19.995m, result);
        }

        [Fact]
        public async Task QuantityReceived_CountsReceivedMovements()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Status = MovementStatus.Submitted,
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { movement, nonReceivedMovement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Received(NotificationId);

            Assert.Equal(5, result);
        }

        [Fact]
        public async Task QuantityRemaining_CountsReceivedMovements()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Status = MovementStatus.New,
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { movement, nonReceivedMovement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Remaining(NotificationId);

            Assert.Equal(15, result);
        }

        [Fact]
        public async Task QuantityReceived_CountsCompletedMovements()
        {
            var completedMovement = new TestableMovement
            {
                Status = MovementStatus.Completed,
                Units = ShipmentQuantityUnits.Kilograms,
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { movement, completedMovement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Received(NotificationId);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task QuantityRemaining_CountsCompletedMovements()
        {
            var completedMovement = new TestableMovement
            {
                Status = MovementStatus.Completed,
                Units = ShipmentQuantityUnits.Kilograms,
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { movement, completedMovement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Remaining(NotificationId);

            Assert.Equal(10, result);
        }

        [Fact]
        public async Task QuantityReceived_Zero_WhenNoMovementsReceived()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Status = MovementStatus.Submitted,
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { nonReceivedMovement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Received(NotificationId);

            Assert.Equal(0, result);
        }

        [Fact]
        public async Task QuantityRemaining_Unchanged_WhenNoMovementsReceived()
        {
            var nonReceivedMovement = new TestableMovement
            {
                Status = MovementStatus.Submitted,
                Receipt = new TestableMovementReceipt
                {
                    Quantity = 5
                }
            };

            var movements = new[] { nonReceivedMovement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Remaining(NotificationId);

            Assert.Equal(20, result);
        }

        [Fact]
        public async Task QuantityRemaining_ReturnedInShipmentInfoUnits()
        {
            shipmentInfo.Units = ShipmentQuantityUnits.Tonnes;

            var movements = new[] { movement };

            SetUpRepositoryCalls(movements, shipmentInfo);

            var result = await movementQuantity.Remaining(NotificationId);

            Assert.Equal(19.995m, result);
        }
    }
}
