namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetShipmentDateDataByMovementIdTests
    {
        private static readonly Guid UserId = new Guid("35745EEC-55E7-42F1-9D8E-3515AC6FA281");
        private static readonly Guid NotificationId = new Guid("28760D3F-E18F-4986-BC7E-06BCD72D554C");
        private static readonly Guid MovementId = new Guid("21BF0933-BF4F-4A51-910E-4DC078C5FEF7");
        private static readonly DateTime startDate = new DateTime(2015, 1, 1);
        private static readonly DateTime endDate = new DateTime(2016, 1, 1);

        private readonly IwsContext context;
        private readonly GetShipmentDateDataByMovementIdHandler handler;
        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly GetShipmentDateDataByMovementId request;

        public GetShipmentDateDataByMovementIdTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            shipmentInfo = new TestableShipmentInfo
            {
                FirstDate = startDate,
                LastDate = endDate
            };

            notificationApplication = new TestableNotificationApplication
            {
                Id = NotificationId,
                UserId = UserId,
                ShipmentInfo = shipmentInfo
            };

            context.NotificationApplications.Add(notificationApplication);

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplicationId = NotificationId
            };

            context.Movements.Add(movement);

            handler = new GetShipmentDateDataByMovementIdHandler(context);
            request = new GetShipmentDateDataByMovementId(MovementId);
        }

        [Fact]
        public async Task ReturnsCorrectDates()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(startDate, result.FirstDate);
            Assert.Equal(endDate, result.LastDate);
            Assert.Equal(MovementId, result.MovementId);
            Assert.Equal(null, result.ActualDate);
        }

        [Fact]
        public async Task GetMovementDoesNotExistThrows()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new GetShipmentDateDataByMovementId(Guid.Empty)));
        }
    }
}
