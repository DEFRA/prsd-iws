namespace EA.Iws.DocumentGeneration.Movement.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;

    internal class MovementBlockFactory : IMovementBlockFactory
    {
        private readonly IMovementDetailsRepository movementDetailsRepository;
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public MovementBlockFactory(IMovementRepository movementRepository,
            IMovementDetailsRepository movementDetailsRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.movementDetailsRepository = movementDetailsRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var movement = await movementRepository.GetById(movementId);
            var movementDetails = await movementDetailsRepository.GetByMovementId(movementId);
            var notification = await notificationApplicationRepository.GetById(movement.NotificationId);
            var shipment = await shipmentInfoRepository.GetByNotificationId(movement.NotificationId);
            return new MovementBlock(mergeFields, movement, movementDetails, notification, shipment);
        }
    }
}