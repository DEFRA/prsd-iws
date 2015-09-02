namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Threading.Tasks;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class GetNumberOfCarriersByMovementIdHandlerTests
    {
        private static readonly Guid MovementId = new Guid("C3FB09EF-0272-41A1-8DB3-56683ECA669D");

        private readonly GetNumberOfCarriersByMovementIdHandler handler;
        private readonly TestableMovement movement;

        public GetNumberOfCarriersByMovementIdHandlerTests()
        {
            var context = new TestIwsContext();
            handler = new GetNumberOfCarriersByMovementIdHandler(context);

            movement = new TestableMovement
            {
                Id = MovementId,
            };

            context.Movements.Add(movement);
        }

        [Fact]
        public async Task MovementDoesNotExist_Throws()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new GetNumberOfCarriersByMovementId(Guid.Empty)));
        }

        [Fact]
        public async Task MovementHasNoCarriers_ReturnsNull()
        {
            movement.MovementCarriers = null;

            var result = await handler.HandleAsync(new GetNumberOfCarriersByMovementId(MovementId));

            Assert.Equal(null, result);
        }

        [Fact]
        public async Task MovementHasCarriers_ReturnsCorrectNumber()
        {
            var carriers = new[]
                {
                    new TestableMovementCarrier
                    {
                        Carrier = new TestableCarrier(),
                        Order = 0
                    },
                    new TestableMovementCarrier
                    {
                        Carrier = new TestableCarrier(),
                        Order = 1
                    }
                };

            movement.MovementCarriers = carriers;

            var result = await handler.HandleAsync(new GetNumberOfCarriersByMovementId(MovementId));

            Assert.Equal(2, result);
        }
    }
}
