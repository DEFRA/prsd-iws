namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetActualDateOfMovementHandlerTests
    {
        private static readonly Guid UserId = new Guid("35745EEC-55E7-42F1-9D8E-3515AC6FA281");
        private static readonly Guid NotificationId = new Guid("28760D3F-E18F-4986-BC7E-06BCD72D554C");
        private static readonly Guid MovementId = new Guid("21BF0933-BF4F-4A51-910E-4DC078C5FEF7");
        private static readonly DateTime startDate = new DateTime(2015, 1, 1);
        private static readonly DateTime endDate = new DateTime(2016, 1, 1);
        private static readonly DateTime shipmentDate = new DateTime(2015, 6, 1);

        private readonly TestIwsContext context;
        private readonly SetActualDateOfMovementHandler handler;
        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly SetActualDateOfMovement request;

        public SetActualDateOfMovementHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            shipmentInfo = new TestableShipmentInfo
            {
                NotificationId = NotificationId,
                ShipmentPeriod = new Domain.ShipmentPeriod(startDate, endDate, true)
            };

            notificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId
            };

            context.ShipmentInfos.Add(shipmentInfo);
            context.NotificationApplications.Add(notificationApplication);

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationId = NotificationId
            };

            context.Movements.Add(movement);

            handler = new SetActualDateOfMovementHandler(context, new SetActualDateOfShipment());
            request = new SetActualDateOfMovement(MovementId, shipmentDate);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            var result = await handler.HandleAsync(new SetActualDateOfMovement(MovementId, shipmentDate));

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task GetMovementDoesNotExistThrows()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new SetActualDateOfMovement(Guid.Empty, shipmentDate)));
        }
    }
}
