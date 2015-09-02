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

    public class SetActualMovementCarriersHandlerTests
    {
        private static readonly Guid MovementId = new Guid("78651374-2CD0-4BF2-BA31-1B70E0EABA0B");
        private static readonly Guid CarrierId = new Guid("83F7E877-3812-4CA0-BAFC-8BBF3DA61475");

        private readonly TestIwsContext context;
        private readonly TestableMovement movement;
        private readonly SetActualMovementCarriersHandler handler;
        private readonly SetActualMovementCarriers request;

        public SetActualMovementCarriersHandlerTests()
        {
            context = new TestIwsContext();

            var carrier = new TestableCarrier
            {
                Id = CarrierId
            };

            var notification = new TestableNotificationApplication
            {
                Carriers = new[] { carrier }
            };

            context.NotificationApplications.Add(notification);

            movement = new TestableMovement
            {
                Id = MovementId,
                NotificationApplication = notification
            };

            context.Movements.Add(movement);

            handler = new SetActualMovementCarriersHandler(context);
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
            movement.MovementCarriers = new TestableMovementCarrier[0];

            await handler.HandleAsync(request);

            Assert.Equal(CarrierId, movement.MovementCarriers.Select(mc => mc.Carrier.Id).Single());
        }

        [Fact]
        public async Task CallsSaveChanges()
        {
            movement.MovementCarriers = new TestableMovementCarrier[0];

            await handler.HandleAsync(request);

            Assert.Equal(1, context.SaveChangesCount);
        }
    }
}
