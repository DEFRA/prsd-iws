namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using EA.Iws.RequestHandlers.Movement;
    using EA.Iws.Requests.Movement;
    using EA.Iws.TestHelpers.DomainFakes;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class GetNotificationIdByMovementIdHandlerTests : TestBase
    {
        private readonly TestableMovement movement;
        private readonly GetNotificationIdByMovementIdHandler handler;

        public GetNotificationIdByMovementIdHandlerTests()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplication = NotificationApplication,
                NotificationApplicationId = NotificationId
            };

            Context.Movements.Add(movement);
            Context.NotificationApplications.Add(NotificationApplication);

            handler = new GetNotificationIdByMovementIdHandler(Context);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.HandleAsync(new GetNotificationIdByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsCorrectId()
        {
            var result = await handler.HandleAsync(new GetNotificationIdByMovementId(MovementId));

            Assert.Equal(NotificationId, result);
        }
    }
}
