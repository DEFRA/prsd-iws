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
        private readonly IwsContext context;

        public CreateImportMovementHandler(IImportMovementFactory importMovementFactory,
            IImportMovementRepository movementRepository, 
            IwsContext context)
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
                new DateTimeOffset(message.ActualShipmentDate, TimeSpan.Zero));

            if (message.PrenotificationDate.HasValue)
            {
                movement.SetPrenotificationDate(new DateTimeOffset(message.PrenotificationDate.Value, 
                    TimeSpan.Zero));
            }

            movementRepository.Add(movement);

            await context.SaveChangesAsync();

            return movement.Id;
        }
    }
}
