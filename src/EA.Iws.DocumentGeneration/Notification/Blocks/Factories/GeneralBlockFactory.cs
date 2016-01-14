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
        private readonly IFacilityRepository facilityRepository;

        public GeneralBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            IShipmentInfoRepository shipmentInfoRepository, IFacilityRepository facilityRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.shipmentInfoRepository = shipmentInfoRepository;
            this.facilityRepository = facilityRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            var shipment = await shipmentInfoRepository.GetByNotificationId(notificationId);
            var facilityCollection = await facilityRepository.GetByNotificationId(notificationId);
            return new GeneralBlock(mergeFields, notification, shipment, facilityCollection);
        }
    }
}