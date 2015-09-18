namespace EA.Iws.RequestHandlers.Tests.Unit.MovementOperationReceipt
{
    using EA.Iws.RequestHandlers.MovementOperationReceipt;
    using EA.Iws.Requests.MovementOperationReceipt;
    using EA.Iws.TestHelpers.DomainFakes;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class CreateMovementOperationReceiptForMovementHandlerTests : TestBase
    {
        private readonly CreateMovementOperationReceiptForMovementHandler handler;

        private static readonly DateTime AnyDate = new DateTime(2015, 1, 1);

        public CreateMovementOperationReceiptForMovementHandlerTests()
        {
            Context.Movements.Add(Movement);

            handler = new CreateMovementOperationReceiptForMovementHandler(Context);
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new CreateMovementOperationReceiptForMovement(Guid.Empty, AnyDate)));
        }

        [Fact]
        public async Task MovementNotReceivedThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new CreateMovementOperationReceiptForMovement(MovementId, AnyDate)));
        }

        [Fact]
        public async Task ReceiptIsCreated()
        {
            SetMovementReceipt();

            await handler.HandleAsync(new CreateMovementOperationReceiptForMovement(MovementId, AnyDate));

            Assert.NotNull(Movement.Receipt.OperationReceipt);
        }

        [Fact]
        public async Task DateIsSet()
        {
            SetMovementReceipt();

            await handler.HandleAsync(new CreateMovementOperationReceiptForMovement(MovementId, AnyDate));

            Assert.Equal(AnyDate, Movement.Receipt.OperationReceipt.Date);
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            SetMovementReceipt();

            await handler.HandleAsync(new CreateMovementOperationReceiptForMovement(MovementId, AnyDate));

            Assert.Equal(1, Context.SaveChangesCount);
        }

        private void SetMovementReceipt()
        {
            Movement.Receipt = new TestableMovementReceipt
            {
                Date = AnyDate,
                Decision = Core.MovementReceipt.Decision.Accepted,
                Quantity = 5
            };
        }
    }
}
