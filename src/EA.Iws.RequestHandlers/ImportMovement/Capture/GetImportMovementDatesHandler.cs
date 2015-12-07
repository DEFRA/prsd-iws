namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class GetImportMovementDatesHandler : IRequestHandler<GetImportMovementDates, ImportMovementDates>
    {
        private readonly IImportMovementRepository movementRepository;

        public GetImportMovementDatesHandler(IImportMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<ImportMovementDates> HandleAsync(GetImportMovementDates message)
        {
            var movement = await movementRepository.Get(message.MovementId);

            return new ImportMovementDates
            {
                ActualDate = movement.ActualShipmentDate,
                Number = movement.Number,
                PreNotificationDate = movement.PrenotificationDate
            };
        }
    }
}
