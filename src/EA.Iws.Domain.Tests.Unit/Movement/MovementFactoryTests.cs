namespace EA.Iws.Domain.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.NotificationAssessment;
    using Domain.Movement;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class MovementFactoryTests
    {
        private static readonly Guid NotificationId = new Guid("0E38E99F-A997-4014-8438-62B56E0398DF");
        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);
        private readonly MovementFactory factory;
        private readonly IMovementRepository movementRepository;
        private readonly IShipmentInfoRepository shipmentRepository;
        private readonly INotificationAssessmentRepository assessmentRepository;

        public MovementFactoryTests()
        {
            shipmentRepository = A.Fake<IShipmentInfoRepository>();
            movementRepository = A.Fake<IMovementRepository>();
            assessmentRepository = A.Fake<INotificationAssessmentRepository>();

            var movementNumberGenerator = new MovementNumberGenerator(new NextAvailableMovementNumberGenerator(movementRepository), movementRepository, shipmentRepository);
            var numberOfMovements = new NumberOfMovements(movementRepository, shipmentRepository);
            factory = new MovementFactory(numberOfMovements, assessmentRepository, movementNumberGenerator);
        }

        [Fact]
        public async Task NewMovementExceedsShipmentLimit_Throws()
        {
            CreateShipmentInfo(maxNumberOfShipments: 1);

            var existingMovement = new TestableMovement
            {
                Id = new Guid("1584B5F6-4E33-441D-A9C9-17C1C3B28BA2"),
                NotificationId = NotificationId,
            };

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new[] { existingMovement });

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, AnyDate));
        }

        [Fact]
        public async Task NotificationNotConsented_Throws()
        {
            CreateShipmentInfo(maxNumberOfShipments: 1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId)).Returns(new Movement[0]);

            await Assert.ThrowsAsync<InvalidOperationException>(() => factory.Create(NotificationId, AnyDate));
        }

        [Fact]
        public async Task ReturnsMovement()
        {
            CreateShipmentInfo(maxNumberOfShipments: 1);

            A.CallTo(() => movementRepository.GetAllMovements(NotificationId))
                .Returns(new Movement[0]);

            A.CallTo(() => assessmentRepository.GetByNotificationId(NotificationId))
                .Returns(new TestableNotificationAssessment { Status = NotificationStatus.Consented });

            var movement = await factory.Create(NotificationId, AnyDate);

            Assert.NotNull(movement);
        }

        private void CreateShipmentInfo(int maxNumberOfShipments)
        {
            var shipment = new TestableShipmentInfo
            {
                Id = new Guid("2DA8E281-A6A4-459A-A38A-B4B0643E0726"),
                NotificationId = NotificationId,
                NumberOfShipments = maxNumberOfShipments
            };

            A.CallTo(() => shipmentRepository.GetByNotificationId(NotificationId)).Returns(shipment);
        }
    }
}