namespace EA.Iws.RequestHandlers.ImportMovement
{
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement;

    internal class IsAddedCancellableImportMovementValidHandler :
        IRequestHandler<IsAddedCancellableImportMovementValid, AddedCancellableImportMovementValidation>
    {
        private readonly IImportMovementRepository repository;
        private readonly IImportNotificationRepository notificationRepository;
        public IsAddedCancellableImportMovementValidHandler(IImportMovementRepository repository, IImportNotificationRepository notificationRepository)
        {
            this.repository = repository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<AddedCancellableImportMovementValidation> HandleAsync(IsAddedCancellableImportMovementValid message)
        {
            var movement = await repository.GetPrenotifiedForNotificationByNumber(message.ImportNotificationId, message.ShipmentNumber);

            var result = await repository.IsShipmentExistingInNonCancellableStatus(message.ImportNotificationId, message.ShipmentNumber);

            if (result != null && result.Status != Core.Movement.MovementStatus.Submitted)
            {
                result.NotificationType = await notificationRepository.GetTypeById(message.ImportNotificationId);
                if (movement != null)
                {
                    result.IsCancellableExistingShipment = true;
                }
            }
            else
            {
                result = new AddedCancellableImportMovementValidation();
                result.IsNonCancellableExistingShipment = false;
                result.IsCancellableExistingShipment = movement != null ? true : false;
            }
            return result;
        }
    }
}
