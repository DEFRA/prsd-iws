namespace EA.Iws.RequestHandlers.Tests.Unit.NotificationMovements.Create
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using FakeItEasy;
    using Prsd.Core;
    using RequestHandlers.NotificationMovements.Create;
    using Requests.NotificationMovements.Create;
    using TestHelpers.DomainFakes;
    using Xunit;

    public class CreateMovementCarriersHandlerTests
    {
        private static readonly Guid CarrierId = new Guid("83F7E877-3812-4CA0-BAFC-8BBF3DA61475");

        private readonly CreateMovementCarriersHandler handler;
        private readonly CreateMovementCarriers request;
        private readonly TestIwsContext context;
        private readonly Guid notificationId = new Guid("442191BE-6016-4545-B245-75E6941AA7BB");
        private Guid[] movementIds = new Guid[1];
        private Guid[] emptyMovementIds = null;
        private IMovementRepository repository = A.Fake<IMovementRepository>();
        private ICarrierRepository carrierRepository = A.Fake<ICarrierRepository>();
        private TestableCarrier carrier = new TestableCarrier
        {
            Id = CarrierId
        };

        public CreateMovementCarriersHandlerTests()
        {
            SystemTime.Freeze(new DateTime(2019, 1, 1));
            movementIds[0] = new Guid("AF1839A1-DA40-430B-9DFE-D79194175DFD");
            
            var carrierRepository = A.Fake<ICarrierRepository>();
            context = new TestIwsContext();

            handler = new CreateMovementCarriersHandler(context, repository, carrierRepository);
            request = new CreateMovementCarriers(notificationId, movementIds, new Dictionary<int, Guid>() { { 1, CarrierId } });
        }

        [Fact]
        public async Task MovementDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<NullReferenceException>(() => handler.HandleAsync(new CreateMovementCarriers(notificationId, emptyMovementIds, new Dictionary<int, Guid>() { { 1, CarrierId } })));
        }

        [Fact]
        public async Task CarrierDoesNotExistThrows()
        {
            await Assert.ThrowsAsync<InvalidOperationException>(() => handler.HandleAsync(new CreateMovementCarriers(notificationId, movementIds, new Dictionary<int, Guid> { { 1, new Guid("2CC7FA6C-9F52-41A8-B664-C03D2DAFCDAC") } })));
        }       
    }
}
