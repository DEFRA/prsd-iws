namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetMovementQuantityByMovementIdHandlerTests : TestBase
    {
        private readonly SetMovementQuantityByMovementIdHandler handler;
        private readonly SetMovementQuantityByMovementId request;

        public SetMovementQuantityByMovementIdHandlerTests()
        {
            var shipmentInfo = new TestableShipmentInfo
            {
                NotificationId = NotificationId,
                Units = ShipmentQuantityUnits.Litres
            };

            Context.ShipmentInfos.Add(shipmentInfo);
            Context.Movements.Add(Movement);
            Context.NotificationApplications.Add(NotificationApplication);

            handler = new SetMovementQuantityByMovementIdHandler(Context);
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

            Assert.Equal(1, Context.SaveChangesCount);
        }

        [Fact]
        public async Task SetsQuantityDifferentToNotification()
        {
            await
                handler.HandleAsync(new SetMovementQuantityByMovementId(MovementId, 10,
                    ShipmentQuantityUnits.CubicMetres));

            Assert.Equal(10, Movement.Quantity);
            Assert.Equal(ShipmentQuantityUnits.CubicMetres, Movement.Units);
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
            Movement.Quantity = 0;

            await handler.HandleAsync(request);

            Assert.Equal(request.Quantity, Movement.Quantity);
        }

        [Fact]
        public async Task ReturnsTrue()
        {
            var result = await handler.HandleAsync(request);

            Assert.True(result);
        }
    }
}
