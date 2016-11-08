namespace EA.Iws.RequestHandlers.NotificationMovements.Capture
{
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.Movement;
    using Prsd.Core.Mediator;
    using Requests.NotificationMovements.Capture;

    internal class CreateMovementInternalHandler : IRequestHandler<CreateMovementInternal, bool>
    {
        private readonly ICapturedMovementFactory factory;
        private readonly IMovementRepository movementRepository;
        private readonly IwsContext context;

        public CreateMovementInternalHandler(ICapturedMovementFactory factory,
            IMovementRepository movementRepository,
            IwsContext context)
        {
            this.factory = factory;
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(CreateMovementInternal message)
        {
            try
            {
                var movement = await factory.Create(message.NotificationId,
                        message.Number,
                        message.PrenotificationDate,
                        message.ActualShipmentDate,
                        message.HasNoPrenotification);

                movementRepository.Add(movement);

                await context.SaveChangesAsync();
            }
            catch (MovementNumberException)
            {
                return false;
            }

            return true;
        }
    }
}
