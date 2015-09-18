namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using Core.MovementReceipt;
    using Core.Shared;
    using RequestHandlers.MovementReceipt;
    using Requests.MovementReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetMovementReceiptQuantityByMovementIdHandlerTests : TestBase
    {
        private readonly SetMovementReceiptQuantityByMovementIdHandler handler;
        private readonly SetMovementReceiptQuantityByMovementId request;

        public SetMovementReceiptQuantityByMovementIdHandlerTests()
        {
            Context.Movements.Add(Movement);
            handler = new SetMovementReceiptQuantityByMovementIdHandler(Context);
            request = new SetMovementReceiptQuantityByMovementId(MovementId, TestDecimal);
        }

        [Fact]
        public async Task Movement_DoesNotExist_Throws()
        {
            Func<Task> action =
                () =>
                    handler.HandleAsync(new SetMovementReceiptQuantityByMovementId(Guid.Empty,
                        AnyDecimal));

            await Assert.ThrowsAsync<InvalidOperationException>(action);
        }

        [Fact]
        public async Task Movement_HasNoQuantitySet_SetsQuantity()
        {
            Movement.Units = ShipmentQuantityUnits.Kilograms;
            Movement.Receipt = new TestableMovementReceipt
            {
                Decision = Decision.Accepted
            };

            await handler.HandleAsync(request);

            Assert.Equal(TestDecimal, Movement.Receipt.Quantity);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            Movement.Units = ShipmentQuantityUnits.Kilograms;
            Movement.Receipt = new TestableMovementReceipt
            {
                Decision = Decision.Accepted
            };

            await handler.HandleAsync(request);

            Assert.Equal(1, Context.SaveChangesCount);
        }
    }
}
