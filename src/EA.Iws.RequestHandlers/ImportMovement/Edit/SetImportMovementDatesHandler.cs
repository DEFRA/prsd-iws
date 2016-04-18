namespace EA.Iws.RequestHandlers.ImportMovement.Edit
{
    using System;
    using System.Threading.Tasks;
    using DataAccess;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Edit;

    internal class SetImportMovementDatesHandler : IRequestHandler<SetImportMovementDates, bool>
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IwsContext context;

        public SetImportMovementDatesHandler(IImportMovementRepository movementRepository, 
            IwsContext context)
        {
            this.movementRepository = movementRepository;
            this.context = context;
        }

        public async Task<bool> HandleAsync(SetImportMovementDates message)
        {
            var movement = await movementRepository.Get(message.MovementId);

            movement.SetActualShipmentDate(message.ActualShipmentDate);

            if (message.PrenotificationDate.HasValue)
            {
                movement.SetPrenotificationDate(message.PrenotificationDate.Value);
            }

            await context.SaveChangesAsync();

            return true;
        }
    }
}
