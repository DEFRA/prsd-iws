namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using Xunit;

    public class CreateMovementForNotificationByIdHandlerTests
    {
        private readonly CreateMovementForNotificationByIdHandler handler;
        private readonly TestService service;
        private static readonly Guid notificationId = new Guid("4F2C1FC0-44F6-478A-BEBC-33DFEE22D977");
        private readonly TestIwsContext testContext;

        public CreateMovementForNotificationByIdHandlerTests()
        {
            testContext = new TestIwsContext();
            service = new TestService();

            handler = new CreateMovementForNotificationByIdHandler(testContext, service);
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
            service.CanCreate = true;

            var result = await handler.HandleAsync(new CreateMovementForNotificationById(notificationId));

            Assert.Single(testContext.Movements, m => m.NotificationApplicationId == notificationId);
        }

        [Fact]
        public async Task SaveChangesIsCalled()
        {
            service.CanCreate = true;

            var result = await handler.HandleAsync(new CreateMovementForNotificationById(notificationId));

            Assert.Equal(1, testContext.SaveChangesCount);
        }

        [Fact]
        public async Task CannotCreateThrows()
        {
            service.CanCreate = false;

            await Assert.ThrowsAsync<InvalidOperationException>(
                () => handler.HandleAsync(new CreateMovementForNotificationById(notificationId)));
        }
    }
}
