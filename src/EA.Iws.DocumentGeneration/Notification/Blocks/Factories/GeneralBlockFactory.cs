namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.NotificationApplication.Shipment;

    internal class GeneralBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly IShipmentInfoRepository shipmentInfoRepository;

        public GeneralBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            IShipmentInfoRepository shipmentInfoRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            var shipment = await shipmentInfoRepository.GetByNotificationId(notificationId);
            return new GeneralBlock(mergeFields, notification, shipment);
        }
    }
}