namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using RequestHandlers.MovementReceipt;
    using Requests.MovementReceipt;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CreateMovementReceiptForMovementHandlerTests : TestBase
    {
        private static readonly DateTime ReceivedDate = new DateTime(2015, 10, 1);
        
        private readonly CreateMovementReceiptForMovementHandler handler;
        private readonly TestableMovement movement;

        public CreateMovementReceiptForMovementHandlerTests()
        {
            movement = new TestableMovement 
            { 
                Id = MovementId,
                Date = new DateTime(2015, 9, 1)
            };

            Context.Movements.Add(movement);

            handler = new CreateMovementReceiptForMovementHandler(Context);
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

            Assert.Equal(1, Context.SaveChangesCount);
        }
    }
}
