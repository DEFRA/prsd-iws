namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
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
        private readonly Guid userId = new Guid("E45663E5-1BD0-4AC3-999B-0E9975BE86FC");

        public MovementNumberGeneratorTests()
        {
            SystemTime.Freeze(new DateTime(2015, 1, 1));

            movementRepository = A.Fake<IMovementRepository>();
            shipmentRepository = A.Fake<IShipmentInfoRepository>();
            generator = new MovementNumberGenerator(new NextAvailableMovementNumberGenerator(movementRepository), movementRepository, shipmentRepository);
        }

        [Fact]
        public async Task StartsWithOne()
        {
            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(10));
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new Movement[0]);

            var result = await generator.Generate(NotificationId);

            Assert.Equal(1, result);
        }
   
        [Fact]
        public async Task FillsGapIfOneExists()
        {
            var movements = new[]
            {
                GetMovement(1, NotificationId),
                GetMovement(3, NotificationId)
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(10));
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(movements);

            var result = await generator.Generate(NotificationId);

            Assert.Equal(2, result);
        }

        [Fact]
        public async Task PicksNextNumberIfNoGaps()
        {
            var movements = new[]
            {
                GetMovement(1, NotificationId),
                GetMovement(2, NotificationId)
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(10));
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(movements);

            var result = await generator.Generate(NotificationId);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task MaxShipmentsReached_Throws()
        {
            var movements = new[]
            {
                GetMovement(1, NotificationId),
                GetMovement(2, NotificationId)
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(CreateShipmentInfo(2));
            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(movements);

            await Assert.ThrowsAsync<InvalidOperationException>(() => generator.Generate(NotificationId));
        }

        private ShipmentInfo CreateShipmentInfo(int maxNumberOfMovements)
        {
            var anyShipmentPeriod = new ShipmentPeriod(new DateTime(2015, 3, 1), new DateTime(2016, 1, 1), true);
            var anyQuantity = new ShipmentQuantity(5m, Core.Shared.ShipmentQuantityUnits.Tonnes);
            return new ShipmentInfo(NotificationId, anyShipmentPeriod, maxNumberOfMovements, anyQuantity);
        }

        private Movement GetMovement(int number, Guid notificationId)
        {
            return new Movement(number, notificationId, DateTime.Today.AddDays(-number), userId);
        }

        public void Dispose()
        {
            SystemTime.Unfreeze();
        }
    }
}