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
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly ICarrierRepository repository;

        public CreateMovementCarriersHandler(IwsContext context, IMovementDetailsRepository movementDetailsRepository, ICarrierRepository repository)
        {
            this.context = context;
            this.movementDetailsRepository = movementDetailsRepository;
            this.repository = repository;
        }

        public async Task<bool> HandleAsync(CreateMovementCarriers message)
        {
            var movementCarriers = new List<MovementCarrier>();

            var carriers = await repository.GetByNotificationId(message.NotificationId);

           //TODO
            return true;
        }
    }
}
