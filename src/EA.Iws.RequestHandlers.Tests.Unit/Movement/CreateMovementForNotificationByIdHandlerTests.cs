namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Domain.Movement;
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

        public CreateMovementForNotificationByIdHandlerTests()
        {
            testContext = new TestIwsContext();

            shipmentInfo = new TestableShipmentInfo
            {
                NotificationId = notificationId,
                NumberOfShipments = 1
            };

            testContext.NotificationApplications.Add(new TestableNotificationApplication
            {
                Id = notificationId,
                UserId = TestIwsContext.UserId
            });

            testContext.NotificationAssessments.Add(new TestableNotificationAssessment
            {
                NotificationApplicationId = notificationId
            });

            testContext.ShipmentInfos.Add(shipmentInfo);

            var factory = new MovementFactory();

            handler = new CreateMovementForNotificationByIdHandler(testContext, factory);
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

            testContext.Movements.Add(new TestableMovement
            {
                NotificationId = notificationId
            });

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new CreateMovementForNotificationById(notificationId)));
        }
    }
}
