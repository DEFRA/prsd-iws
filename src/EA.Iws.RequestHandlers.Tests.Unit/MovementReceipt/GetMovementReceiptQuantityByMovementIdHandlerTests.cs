namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementReceiptQuantityByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementUnitsByMovementIdHandler handler;
        private readonly GetMovementUnitsByMovementId request;

        public GetMovementReceiptQuantityByMovementIdHandlerTests()
        {
            handler = new GetMovementUnitsByMovementIdHandler(Context);

            request = new GetMovementUnitsByMovementId(MovementId);
            Movement.Units = ShipmentQuantityUnits.Kilograms;

            Context.Movements.Add(Movement);
        }

        [Fact]
        public async Task ReceiptIsNull_ReturnsShipmentDisplayUnits()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(ShipmentQuantityUnits.Kilograms, result);
        }
        
        [Fact]
        public async Task Movement_DoesNotExist_Throws()
        {
            Func<Task> handle = () => handler.HandleAsync(new GetMovementUnitsByMovementId(Guid.Empty));

            await Assert.ThrowsAsync<InvalidOperationException>(handle);
        }
    }
}
