namespace EA.Iws.RequestHandlers.Tests.Unit.MovementReceipt
{
    using System;
    using System.Threading.Tasks;
    using EA.Iws.TestHelpers.DomainFakes;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using Xunit;

    public class GetMovementDateByMovementIdHandlerTests : TestBase
    {
        private readonly GetMovementDateByMovementIdHandler handler;
        private readonly GetMovementDateByMovementId request;
        private readonly TestableMovement movement;

        private static readonly DateTime MovementDate = new DateTime(2015, 6, 1);
        private static readonly DateTime DateReceived = new DateTime(2015, 9, 1);

        public GetMovementDateByMovementIdHandlerTests()
        {
            movement = new TestableMovement
            {
                Id = MovementId,
                Date = MovementDate
            };

            Context.Movements.Add(movement);

            handler = new GetMovementDateByMovementIdHandler(Context);
            request = new GetMovementDateByMovementId(MovementId);
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new GetMovementDateByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task ReturnsMovementDate()
        {
            var result = await handler.HandleAsync(request);

            Assert.Equal(MovementDate, result);
        }
    }
}
