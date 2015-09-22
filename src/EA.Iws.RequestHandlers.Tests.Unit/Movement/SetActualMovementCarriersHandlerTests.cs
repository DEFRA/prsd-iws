namespace EA.Iws.RequestHandlers.Tests.Unit.Movement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using RequestHandlers.Movement;
    using Requests.Movement;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class SetActualMovementCarriersHandlerTests : TestBase
    {
        private static readonly Guid CarrierId = new Guid("83F7E877-3812-4CA0-BAFC-8BBF3DA61475");

        private readonly SetActualMovementCarriersHandler handler;
        private readonly SetActualMovementCarriers request;

        public SetActualMovementCarriersHandlerTests()
        {
            var carrier = new TestableCarrier
            {
                Id = CarrierId
            };

            NotificationApplication.Carriers = new[] { carrier };

            Context.NotificationApplications.Add(NotificationApplication);

            Context.Movements.Add(Movement);

            handler = new SetActualMovementCarriersHandler(Context);
            request = new SetActualMovementCarriers(MovementId, new Dictionary<int, Guid>() { { 0, CarrierId } });
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                handler.HandleAsync(new SetActualMovementCarriers(Guid.Empty, new Dictionary<int, Guid>())));
        }

        [Fact]
        public async Task CarriersAreSet()
        {
            Movement.MovementCarriers = new TestableMovementCarrier[0];

            await handler.HandleAsync(request);

            Assert.Equal(CarrierId, Movement.MovementCarriers.Select(mc => mc.Carrier.Id).Single());
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            Movement.MovementCarriers = new TestableMovementCarrier[0];

            await handler.HandleAsync(request);

            Assert.Equal(1, Context.SaveChangesCount);
        }
    }
}
