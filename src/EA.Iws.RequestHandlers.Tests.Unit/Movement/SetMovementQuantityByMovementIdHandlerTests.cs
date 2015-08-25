namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetMovementQuantityByMovementIdHandlerTests
    {
        private static readonly Guid UserId = new Guid("F128624E-3BC3-432C-899E-0A48EBD98332");
        private static readonly Guid MovementId = new Guid("F128624E-3BC3-432C-899E-0A48EBD98332");
        private static readonly Guid NotificationId = new Guid("F128624E-3BC3-432C-899E-0A48EBD98332");

        private readonly SetMovementQuantityByMovementIdHandler handler;
        private readonly TestIwsContext context;
        private readonly TestableMovement movement;
        private readonly SetMovementQuantityByMovementId request;

        public SetMovementQuantityByMovementIdHandlerTests()
        {
            context = new TestIwsContext(new TestUserContext(UserId));
            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplicationId = NotificationId,
                NotificationApplication = new TestableNotificationApplication
                {
                    Id = NotificationId,
                    ShipmentInfo = new TestableShipmentInfo
                    {
                        Units = ShipmentQuantityUnits.Litres
                    }
                }
            };
            context.Movements.Add(movement);

            handler = new SetMovementQuantityByMovementIdHandler(context);
            request = new SetMovementQuantityByMovementId(MovementId, 10, ShipmentQuantityUnits.Litres);
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(()
                =>
                handler.HandleAsync(new SetMovementQuantityByMovementId(Guid.Empty, 10, ShipmentQuantityUnits.Litres)));
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            await handler.HandleAsync(request);

            Assert.Equal(1, context.SaveChangesCount);
        }

        [Fact]
        public async Task UnitDimensionsDoNotMatchThrows()
        {
            await
                Assert.ThrowsAsync<ArgumentException>(
                    () =>
                        handler.HandleAsync(new SetMovementQuantityByMovementId(MovementId, 10,
                            ShipmentQuantityUnits.Kilograms)));
        }

        [Fact]
        public async Task UnitsAreDifferentButConvertibleSetsValueAndConvertsUnits()
        {
            await
                handler.HandleAsync(new SetMovementQuantityByMovementId(MovementId, 10,
                    ShipmentQuantityUnits.CubicMetres));

            Assert.Equal(10 * 1000, movement.Quantity);
            Assert.Equal(ShipmentQuantityUnits.Litres, movement.Units);
        }

        [Fact]
        public async Task SetQuantityNegativeThrows()
        {
            await
                Assert.ThrowsAsync<ArgumentOutOfRangeException>(
                    () =>
                        handler.HandleAsync(new SetMovementQuantityByMovementId(MovementId, -10,
                            ShipmentQuantityUnits.Litres)));
        }

        [Fact]
        public async Task SetsQuantity()
        {
            movement.Quantity = 0;

            await handler.HandleAsync(request);

            Assert.Equal(request.Quantity, movement.Quantity);
        }

        [Fact]
        public async Task ReturnsTrue()
        {
            var result = await handler.HandleAsync(request);

            Assert.True(result);
        }
    }
}
