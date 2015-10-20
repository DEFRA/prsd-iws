namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;
    using Domain.NotificationAssessment;
    using FakeItEasy;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CreateMovementForNotificationByIdHandlerTests
    {
        private readonly CreateMovementForNotificationByIdHandler handler;
        private static readonly Guid notificationId = new Guid("4F2C1FC0-44F6-478A-BEBC-33DFEE22D977");
        private readonly TestIwsContext testContext;
        private readonly TestableShipmentInfo shipmentInfo;
        private IMovementRepository movementRepository;

        public CreateMovementForNotificationByIdHandlerTests()
        {
            testContext = new TestIwsContext();

            shipmentInfo = new TestableShipmentInfo
            {
                NotificationId = notificationId,
                NumberOfShipments = 1
            };
            
            var factory = new MovementFactory();

            var shipmentRepository = A.Fake<IShipmentInfoRepository>();
            A.CallTo(() => shipmentRepository.GetByNotificationId(notificationId))
                .Returns(shipmentInfo);

            movementRepository = A.Fake<IMovementRepository>();
            A.CallTo(() => movementRepository.GetAllMovements(notificationId))
                .Returns(new Movement[0]);

            var notificationRepository = A.Fake<INotificationApplicationRepository>();
            A.CallTo(() => notificationRepository.GetById(notificationId))
                .Returns(new TestableNotificationApplication
                {
                    Id = notificationId,
                    UserId = TestIwsContext.UserId
                });

            var assessmentRepository = A.Fake<INotificationAssessmentRepository>();
            A.CallTo(() => assessmentRepository.GetByNotificationId(notificationId))
                .Returns(new TestableNotificationAssessment
                {
                    NotificationApplicationId = notificationId
                });

            handler = new CreateMovementForNotificationByIdHandler(
                testContext, 
                factory,
                movementRepository,
                notificationRepository,
                assessmentRepository, 
                shipmentRepository);
        }

        [Fact]
        public async Task NotificationDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.HandleAsync(new CreateMovementForNotificationById(Guid.Empty)));
        }

        [Fact]
        public async Task NotificationExistsAddsMovement()
        {
            var result = await handler.HandleAsync(new CreateMovementForNotificationById(notificationId));

            Assert.Single(testContext.Movements, m => m.NotificationId == notificationId);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            var result = await handler.HandleAsync(new CreateMovementForNotificationById(notificationId));

            Assert.Equal(1, testContext.SaveChangesCount);
        }

        [Fact]
        public async Task CannotCreateThrows()
        {
            shipmentInfo.NumberOfShipments = 1;

            A.CallTo(() => movementRepository.GetAllMovements(notificationId))
                .Returns(new[] 
                {
                    new TestableMovement
                    {
                        NotificationId = notificationId
                    }
                });
            
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new CreateMovementForNotificationById(notificationId)));
        }
    }
}
