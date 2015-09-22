namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using DataAccess;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementQuantityDataByMovementIdHandlerTests
    {
        private static readonly Guid UserId = new Guid("35745EEC-55E7-42F1-9D8E-3515AC6FA281");
        private static readonly Guid NotificationId = new Guid("28760D3F-E18F-4986-BC7E-06BCD72D554C");
        private static readonly Guid MovementId = new Guid("21BF0933-BF4F-4A51-910E-4DC078C5FEF7");
        private const decimal AnyDecimal = 1000m;
        
        private readonly IwsContext context;
        private readonly GetMovementQuantityDataByMovementIdHandler handler;
        private readonly TestableNotificationApplication notificationApplication;
        private readonly TestableMovement movement;
        private readonly TestableShipmentInfo shipmentInfo;
        private readonly GetMovementQuantityDataByMovementId request;

        public GetMovementQuantityDataByMovementIdHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));

            shipmentInfo = new TestableShipmentInfo
            {
                Quantity = AnyDecimal,
                Units = ShipmentQuantityUnits.CubicMetres
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
                NotificationId = NotificationId
            };

            context.Movements.Add(movement);

            handler = new GetMovementQuantityDataByMovementIdHandler(context);
            request = new GetMovementQuantityDataByMovementId(MovementId);
        }

        [Fact]
        public async Task GetMovementDoesNotExistThrows()
        {
            await
                Assert.ThrowsAsync<InvalidOperationException>(
                    () => handler.HandleAsync(new GetMovementQuantityDataByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsCorrectTotalValue()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(AnyDecimal, result.TotalNotifiedQuantity);
        }

        [Fact]
        public async Task ReturnsCorrectCurrentlyUsedWhereNonZero()
        {
            var occupiedQuantity = 500m;

            context.Movements.Add(new TestableMovement
            {
                NotificationId = NotificationId,
                Quantity = occupiedQuantity
            });

            var result = await handler.HandleAsync(request);

            Assert.Equal(occupiedQuantity, result.TotalCurrentlyUsedQuantity);
        }

        [Fact]
        public async Task ReturnsCorrectCurrentlyUsedWhereZero()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(0, result.TotalCurrentlyUsedQuantity);
        }

        [Fact]
        public async Task ReturnsCorrectThisMovementQuantityWhereZero()
        {
            movement.Quantity = 0;

            var result = await handler.HandleAsync(request);

            Assert.Equal(0, result.ThisMovementQuantity);
        }

        [Fact]
        public async Task ReturnsCorrectThisMovementQuantityWhereNonZero()
        {
            movement.Quantity = 70m;

            var result = await handler.HandleAsync(request);

            Assert.Equal(70m, result.ThisMovementQuantity);
        }

        [Fact]
        public async Task ReturnsThisMovementQuantityEqualsTotalUsedQuantity()
        {
            movement.Quantity = 70m;

            var result = await handler.HandleAsync(request);

            Assert.Equal(70m, result.TotalCurrentlyUsedQuantity);
            Assert.Equal(result.ThisMovementQuantity, result.TotalCurrentlyUsedQuantity);
        }

        [Fact]
        public async Task ReturnsThisMovementQuantityContainedInTotalUsedQuantity()
        {
            context.Movements.Add(new TestableMovement
            {
                NotificationId = NotificationId,
                Quantity = 50m
            });

            movement.Quantity = 20m;

            var result = await handler.HandleAsync(request);

            Assert.Equal(20, result.ThisMovementQuantity);
            Assert.Equal(70, result.TotalCurrentlyUsedQuantity);
            Assert.Equal(AnyDecimal, result.TotalNotifiedQuantity);
            Assert.Equal(AnyDecimal - 70, result.AvailableQuantity);
        }

        [Fact]
        public void CalculatesAvailableQuantityCorrectly()
        {
            var data = new MovementQuantityData
            {
                TotalNotifiedQuantity = AnyDecimal,
                TotalCurrentlyUsedQuantity = 50,
                ThisMovementQuantity = 10
            };

            Assert.Equal(AnyDecimal - 50, data.AvailableQuantity);
        }

        [Fact]
        public async Task ReturnsUnitsCorrectly()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(shipmentInfo.Units, result.Units);
        }
    }
}
