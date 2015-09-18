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
            Movement.Units = ShipmentQuantityUnits.Kilograms;

            Context.Movements.Add(Movement);
        }

        [Fact]
        public async Task ReceiptIsNull_ReturnsShipmentDisplayUnits_QuantityNull()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(ShipmentQuantityUnits.Kilograms, result.Unit);
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
            Movement.Receipt = new TestableMovementReceipt
            {
                Quantity = 10
            };

            var result = await handler.HandleAsync(request);

            Assert.Equal(ShipmentQuantityUnits.Tonnes, result.Unit);
            Assert.Equal(10, result.Quantity);
        }
    }
}
