namespace EA.Iws.RequestHandlers.ImportNotificationMovements
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Core.ImportNotificationMovements;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Prsd.Core.Mediator;
    using Requests.ImportNotificationMovements;
    using ImportMovement = Core.ImportMovement.ImportMovement;

    internal class GetImportMovementsSummaryHandler : IRequestHandler<GetImportMovementsSummary, Summary>
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportNotificationRepository notificationRepository;

        public GetImportMovementsSummaryHandler(IImportMovementRepository movementRepository, 
            IImportNotificationRepository notificationRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationRepository = notificationRepository;
        }

        public async Task<Summary> HandleAsync(GetImportMovementsSummary message)
        {
            var notification = await notificationRepository.GetByImportNotificationId(message.ImportNotificationId);

            var movements = await movementRepository.GetForNotification(message.ImportNotificationId);

            return new Summary
            {
                Id = message.ImportNotificationId,
                NotificationType = notification.NotificationType,
                NotificationNumber = notification.NotificationNumber,
                Movements = new List<ImportMovement>(movements.Select(m => new ImportMovement
                {
                    Data = new ImportMovementData
                    {
                        Number = m.Number,
                        ActualDate = m.ActualShipmentDate,
                        PreNotificationDate = m.PrenotificationDate
                    }
                }))
            };
        }
    }
}
