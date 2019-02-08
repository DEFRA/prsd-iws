namespace EA.Iws.RequestHandlers.NotificationMovements.Create
{
    using DataAccess;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Create;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;

    internal class CreateMovementCarriersHandler : IRequestHandler<CreateMovementCarriers, bool>
    {
        private readonly IwsContext context;
        private readonly IMovementRepository movementRepository;
        private readonly ICarrierRepository repository;

        public CreateMovementCarriersHandler(IwsContext context, IMovementRepository movementRepository, ICarrierRepository repository)
        {
            this.context = context;
            this.movementRepository = movementRepository;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CreateMovementCarriers message)
        {
            var movementCarriers = new List<MovementCarrier>();
            var carriers = await repository.GetByNotificationId(message.NotificationId);

            foreach (var movementId in message.MovementId)
            {
                var movement = await movementRepository.GetById(movementId);

                foreach (var carrierId in message.SelectedCarriers)
                {
                    Carrier carrier = carriers.GetCarrier(carrierId.Value);
                    var movementCarrier = new MovementCarrier(movement.Id, carrier.Id, carrierId.Key);
                    context.MovementCarrier.Add(movementCarrier);
                }
                await context.SaveChangesAsync();
            }
            return true;
        }
    }
}
