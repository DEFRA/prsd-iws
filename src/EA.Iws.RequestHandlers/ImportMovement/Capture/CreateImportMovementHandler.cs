namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class CreateImportMovementHandler : IRequestHandler<CreateImportMovement, Guid>
    {
        private readonly IImportMovementFactory importMovementFactory;
        private readonly IImportMovementRepository movementRepository;
        private readonly ImportNotificationContext context;

        public CreateImportMovementHandler(IImportMovementFactory importMovementFactory,
            IImportMovementRepository movementRepository, 
            ImportNotificationContext context)
        {
            this.importMovementFactory = importMovementFactory;
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<Guid> HandleAsync(CreateImportMovement message)
        {
            var movement =
                await importMovementFactory.Create(message.NotificationId, 
                message.Number, 
                message.ActualShipmentDate, message.PrenotificationDate);

            if (message.PrenotificationDate.HasValue)
            {
                movement.SetPrenotificationDate(message.PrenotificationDate.Value);
            }

            movementRepository.Add(movement);

            await context.SaveChangesAsync();

            return movement.Id;
        }
    }
}
