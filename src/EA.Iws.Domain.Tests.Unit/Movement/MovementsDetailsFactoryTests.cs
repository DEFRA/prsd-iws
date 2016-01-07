namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Movement;
    using Core.Shared;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementsDetailsFactoryTests
    {
        private readonly MovementDetailsFactory factory;
        private readonly Guid notificationId = new Guid("83A0D663-0523-4675-ABC1-7E64330F88DC");
        private TestableMovement movement;

        public MovementsDetailsFactoryTests()
        {
            var movementRepository = A.Fake<IMovementRepository>();
            var shipmentRepository = A.Fake<IShipmentInfoRepository>();

            // Setup notification with 1000Kg intended quantity and 950Kg received

            movement = new TestableMovement
            {
                Id = new Guid("B3A80B02-52BF-4051-9BE8-7AA9F7758E0E"),
                NotificationId = notificationId,
                Receipt = new TestableMovementReceipt
                {
                    QuantityReceived = new ShipmentQuantity(950, ShipmentQuantityUnits.Kilograms)
                }
            };

            A.CallTo(() => movementRepository.GetMovementsByStatus(notificationId, MovementStatus.Received))
                .Returns(new[] { movement });

            var shipment = new TestableShipmentInfo
            {
                NotificationId = notificationId,
                Quantity = 1000,
                Units = ShipmentQuantityUnits.Kilograms
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(notificationId))
                .Returns(shipment);

            var movementQuantity = new NotificationMovementsQuantity(movementRepository, shipmentRepository);
            factory = new MovementDetailsFactory(movementQuantity);
        }

        [Fact]
        public async Task QuantityExceedsRemaining_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(movement, new ShipmentQuantity(100, ShipmentQuantityUnits.Kilograms), 1,
                new[]
                {
                    new TestableMovementCarrier()
                },
                new[]
                {
                    new TestablePackagingInfo()
                }));
        }

        [Fact]
        public async Task QuantityWithinRemaining_CreatesDetails()
        {
            var details = await factory.Create(movement, new ShipmentQuantity(20, ShipmentQuantityUnits.Kilograms), 1,
                new[]
                {
                    new TestableMovementCarrier()
                },
                new[]
                {
                    new TestablePackagingInfo()
                });

            Assert.IsType<MovementDetails>(details);
        }
    }
}