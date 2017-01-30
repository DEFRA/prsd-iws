namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;

    internal class GetImportMovementDatesHandler : IRequestHandler<GetImportMovementDates, ImportMovementData>
    {
        private readonly IImportMovementRepository movementRepository;

        public GetImportMovementDatesHandler(IImportMovementRepository movementRepository)
        {
            this.movementRepository = movementRepository;
        }

        public async Task<ImportMovementData> HandleAsync(GetImportMovementDates message)
        {
            var movement = await movementRepository.Get(message.MovementId);

            return new ImportMovementData
            {
                NotificationId = movement.NotificationId,
                ActualDate = movement.ActualShipmentDate,
                Number = movement.Number,
                PreNotificationDate = movement.PrenotificationDate,
                IsCancelled = movement.IsCancelled
            };
        }
    }
}
