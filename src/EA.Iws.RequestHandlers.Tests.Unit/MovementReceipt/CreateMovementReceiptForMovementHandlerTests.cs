namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using EA.Iws.RequestHandlers.MovementReceipt;
    using EA.Iws.Requests.MovementReceipt;
    using EA.Iws.TestHelpers.DomainFakes;
    using System;
    using System.Threading.Tasks;
    using Xunit;

    public class CreateMovementReceiptForMovementHandlerTests
    {
        private readonly CreateMovementReceiptForMovementHandler handler;
        private readonly TestableMovement movement;
        private readonly TestIwsContext context;

        private static readonly DateTime ReceivedDate = new DateTime(2015, 10, 1);
        private static readonly Guid MovementId = new Guid("11BB497E-51FA-4956-BC93-F1AE6EAAE4F2");

        public CreateMovementReceiptForMovementHandlerTests()
        {
            context = new TestIwsContext();

            movement = new TestableMovement 
            { 
                Id = MovementId,
                Date = new DateTime(2015, 9, 1)
            };

            context.Movements.Add(movement);

            handler = new CreateMovementReceiptForMovementHandler(context);
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                handler.HandleAsync(new CreateMovementReceiptForMovement(Guid.Empty, ReceivedDate)));
        }

        [Fact]
        public async Task ReceiptIsCreated()
        {
            await handler.HandleAsync(new CreateMovementReceiptForMovement(MovementId, ReceivedDate));

            Assert.NotNull(movement.Receipt);
        }

        [Fact]
        public async Task ReceivedDateIsSet()
        {
            await handler.HandleAsync(new CreateMovementReceiptForMovement(MovementId, ReceivedDate));

            Assert.Equal(ReceivedDate, movement.Receipt.Date);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            await handler.HandleAsync(new CreateMovementReceiptForMovement(MovementId, ReceivedDate));

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
