namespace EA.Iws.RequestHandlers.Movement
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Movement;

    internal class IsAddedCancellableMovementValidHandler :
        IRequestHandler<IsAddedCancellableMovementValid, AddedCancellableMovementValidation>
    {
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationRepository;

        public IsAddedCancellableMovementValidHandler(IMovementRepository movementRepository,
            INotificationApplicationRepository notificationRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<AddedCancellableMovementValidation> HandleAsync(IsAddedCancellableMovementValid message)
        {
            var movement = await movementRepository.GetByNumberOrDefault(message.ShipmentNumber, message.NotificationId);
            var notificationType = await notificationRepository.GetNotificationType(message.NotificationId);

            var result = new AddedCancellableMovementValidation()
            {
                NotificationType = notificationType
            };
            
            if (movement != null)
            {
                result.IsCancellableExistingShipment = movement.IsInternallyCancellable;

                result.IsNonCancellableExistingShipment = !movement.IsInternallyCancellable;

                result.Status = movement.Status;
            }

            return result;
        }
    }
}
