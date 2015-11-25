namespace EA.Iws.RequestHandlers.Movement.Receive
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.Movement.Receive;

    internal class RecordReceiptInternalHandler : IRequestHandler<RecordReceiptInternal, bool>
    {
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;

        public RecordReceiptInternalHandler(IMovementRepository movementRepository, IwsContext context)
        {
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(RecordReceiptInternal message)
        {
            var movement = await movementRepository.GetById(message.MovementId);

            movement.ReceiveInternally(message.ReceivedDate, new ShipmentQuantity(message.ActualQuantity, message.Units));

            await context.SaveChangesAsync();

            return true;
        }
    }
}
