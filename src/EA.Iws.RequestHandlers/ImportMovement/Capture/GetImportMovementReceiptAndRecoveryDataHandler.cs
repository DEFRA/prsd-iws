namespace EA.Iws.RequestHandlers.ImportMovement.Capture
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.ImportMovement;
    using Core.Shared;
    using Domain.ImportMovement;
    using Domain.ImportNotification;
    using Prsd.Core.Mapper;
    using Prsd.Core.Mediator;
    using Requests.ImportMovement.Capture;
    using ImportMovement = Core.ImportMovement.ImportMovement;

    internal class GetImportMovementReceiptAndRecoveryDataHandler : IRequestHandler<GetImportMovementReceiptAndRecoveryData, ImportMovement>
    {
        private readonly IImportMovementRepository movementRepository;
        private readonly IImportNotificationRepository importNotificationRepository;
        private readonly IMapper mapper;

        public GetImportMovementReceiptAndRecoveryDataHandler(IImportMovementRepository movementRepository, 
            IImportNotificationRepository importNotificationRepository,
            IMapper mapper)
        {
            this.movementRepository = movementRepository;
            this.importNotificationRepository = importNotificationRepository;
            this.mapper = mapper;
        }

        public async Task<ImportMovement> HandleAsync(GetImportMovementReceiptAndRecoveryData message)
        {
            var movement = await movementRepository.Get(message.ImportMovementId);
            var notification = await importNotificationRepository.GetByImportNotificationId(movement.NotificationId);

            var movementData = mapper.Map<ImportMovementData>(movement);
            movementData.NotificationType = notification.NotificationType;

            return new ImportMovement
            {
                Data = movementData,
                Receipt = new ImportMovementReceipt
                {
                    PossibleUnits = new List<ShipmentQuantityUnits>
                    {
                        ShipmentQuantityUnits.CubicMetres,
                        ShipmentQuantityUnits.Kilograms
                    }
                },
                Recovery = new ImportMovementRecovery()
            };
        }
    }
}
