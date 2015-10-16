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
        private readonly IMovementRepository movementRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public MovementBlockFactory(IMovementRepository movementRepository,
            INotificationApplicationRepository notificationApplicationRepository,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.movementRepository = movementRepository;
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<IDocumentBlock> Create(Guid movementId, IList<MergeField> mergeFields)
        {
            var movement = await movementRepository.GetById(movementId);
            var notification = await notificationApplicationRepository.GetById(movement.NotificationId);
            var shipment = await shipmentInfoRepository.GetByNotificationId(movement.NotificationId);
            return new MovementBlock(mergeFields, movement, notification, shipment);
        }
    }
}