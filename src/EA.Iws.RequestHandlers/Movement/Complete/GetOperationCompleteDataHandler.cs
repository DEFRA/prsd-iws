namespace EA.Iws.RequestHandlers.Movement.Complete
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Prsd.Core.Mediator;
    using Requests.Movement.Complete;

    internal class GetOperationCompleteDataHandler : IRequestHandler<GetOperationCompleteData, OperationCompleteData>
    {
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public GetOperationCompleteDataHandler(IMovementRepository movementRepository,
            INotificationApplicationRepository notificationApplicationRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<OperationCompleteData> HandleAsync(GetOperationCompleteData message)
        {
            var notificationType = (await notificationApplicationRepository.GetByMovementId(message.MovementId)).NotificationType;
            var movement = await movementRepository.GetById(message.MovementId);

            var data = new OperationCompleteData
            {
                NotificationType = notificationType,
                ReceiptDate = movement.Receipt.Date
            };

            return data;
        }
    }
}
