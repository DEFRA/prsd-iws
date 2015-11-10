namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using FakeItEasy;
    using Prsd.Core;
    using Xunit;

    public class MovementNumberGeneratorTests : IDisposable
    {
        private readonly MovementNumberGenerator generator;
        private static readonly Guid NotificationId = new Guid("89425659-DDDA-4168-BACD-3032A5EB1FB6");
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;

        public MovementNumberGeneratorTests()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            movementRepository = A.Fake<IMovementRepository>();
            shipmentRepository = A.Fake<IShipmentInfoRepository>();
            generator = new MovementNumberGenerator(movementRepository, shipmentRepository);
        }

        [Fact]
        public async Task GeneratesCorrectCount()
        {
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(10));

            var result = await generator.Generate(NotificationId, 4);

            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task GeneratesCorrectFirstAndLastNumber()
        {
            var movements = new[]
            {
                GetMovement(1, NotificationId),
                GetMovement(2, NotificationId)
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(10));
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(movements);

            var result = await generator.Generate(NotificationId, 6);

            Assert.Equal(3, result.Min());
            Assert.Equal(8, result.Max());
        }

        [Fact]
        public async Task ExceedingMaxAllowed_ThrowsException()
        {
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(4));

            await Assert.ThrowsAsync<InvalidOperationException>(() => generator.Generate(NotificationId, 5));
        }

        private ShipmentInfo CreateShipmentInfo(int maxNumberOfMovements)
        {
            var anyShipmentPeriod = new ShipmentPeriod(new DateTime(2015, 3, 1), new DateTime(2016, 1, 1), true);
            var anyQuantity = new ShipmentQuantity(5m, Core.Shared.ShipmentQuantityUnits.Tonnes);
            return new ShipmentInfo(NotificationId, anyShipmentPeriod, maxNumberOfMovements, anyQuantity);
        }

        private Movement GetMovement(int number, Guid notificationId)
        {
            Movement movement = new Movement(number, notificationId);
            movement.Date = DateTime.Today.AddDays(-number);
            return movement;
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}