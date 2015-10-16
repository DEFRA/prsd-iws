namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using EA.Iws.RequestHandlers.MovementReceipt;
    using EA.Iws.Requests.MovementReceipt;
    using EA.Iws.TestHelpers.DomainFakes;
    using Xunit;

    public class GetMovementReceiptDateByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementReceiptDateByMovementIdHandler handler;
        private readonly GetMovementReceiptDateByMovementId request;
        private readonly TestableMovement movement;

        private static readonly DateTime MovementDate = new DateTime(2015, 6, 1);
        private static readonly DateTime DateReceived = new DateTime(2015, 9, 1);

        public GetMovementReceiptDateByMovementIdHandlerTests()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = MovementDate
            };

            Context.Movements.Add(movement);

            handler = new GetMovementReceiptDateByMovementIdHandler(Context);
            request = new GetMovementReceiptDateByMovementId(MovementId);
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new GetMovementReceiptDateByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsMovementDate()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(MovementDate, result.MovementDate);
        }

        [Fact]
        public async Task MovementNotRecieved_ReturnsNullDateReceived()
        {
            var result = await handler.HandleAsync(request);

            Assert.Null(result.DateReceived);
        }
    }
}
