namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using RequestHandlers.MovementReceipt;
    using Requests.MovementReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementReceiptQuantityByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementReceiptQuantityByMovementIdHandler handler;
        private readonly GetMovementReceiptQuantityByMovementId request;

        public GetMovementReceiptQuantityByMovementIdHandlerTests()
        {
            handler = new GetMovementReceiptQuantityByMovementIdHandler(Context);

            request = new GetMovementReceiptQuantityByMovementId(MovementId);
            Context.Movements.Add(Movement);
        }

        [Fact]
        public async Task ReceiptIsNull_ReturnsShipmentDisplayUnits_QuantityNull()
        {
            Movement.Units = ShipmentQuantityUnits.Kilograms;
            Movement.DisplayUnits = ShipmentQuantityUnits.Tonnes;

            var result = await handler.HandleAsync(request);

            Assert.Equal(ShipmentQuantityUnits.Tonnes, result.MovementUnit);
            Assert.Null(result.Quantity);
        }
        
        [Fact]
        public async Task Movement_DoesNotExist_Throws()
        {
            Func<Task> handle = () => handler.HandleAsync(new GetMovementReceiptQuantityByMovementId(Guid.Empty));

            await Assert.ThrowsAsync<InvalidOperationException>(handle);
        }

        [Fact]
        public async Task Receipt_HasQuantity_ReturnsQuantity()
        {
            Movement.Units = ShipmentQuantityUnits.Tonnes;
            Movement.DisplayUnits = ShipmentQuantityUnits.Tonnes;
            Movement.Receipt = new TestableMovementReceipt
            {
                Quantity = 10
            };

            var result = await handler.HandleAsync(request);

            Assert.Equal(ShipmentQuantityUnits.Tonnes, result.MovementUnit);
            Assert.Equal(10, result.Quantity);
        }

        [Theory]
        [InlineData(ShipmentQuantityUnits.Tonnes, ShipmentQuantityUnits.Kilograms)]
        [InlineData(ShipmentQuantityUnits.CubicMetres, ShipmentQuantityUnits.Litres)]
        public async Task Receipt_HasQuantityWithDifferentUnits_ReturnsConvertedUpQuantity(ShipmentQuantityUnits movementUnits, ShipmentQuantityUnits displayUnits)
        {
            Movement.Units = movementUnits;
            Movement.DisplayUnits = displayUnits;
            Movement.Receipt = new TestableMovementReceipt
            {
                Quantity = 10
            };

            var result = await handler.HandleAsync(request);

            Assert.Equal(displayUnits, result.MovementUnit);
            Assert.Equal(10 * 1000, result.Quantity);
        }

        [Theory]
        [InlineData(ShipmentQuantityUnits.Kilograms, ShipmentQuantityUnits.Tonnes)]
        [InlineData(ShipmentQuantityUnits.Litres, ShipmentQuantityUnits.CubicMetres)]
        public async Task Receipt_HasQuantityWithDifferentUnits_ReturnsConvertedDownQuantity(ShipmentQuantityUnits movementUnits, ShipmentQuantityUnits displayUnits)
        {
            Movement.Units = movementUnits;
            Movement.DisplayUnits = displayUnits;
            Movement.Receipt = new TestableMovementReceipt
            {
                Quantity = 10
            };

            var result = await handler.HandleAsync(request);

            Assert.Equal(displayUnits, result.MovementUnit);
            Assert.Equal(10 / 1000.0m, result.Quantity);
        }
    }
}
