namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class CarrierBlockFactory : INotificationBlockFactory
    {
        private readonly ICarrierRepository carrierRepository;
        private readonly INotificationApplicationRepository notificationRepository;

        public CarrierBlockFactory(INotificationApplicationRepository notificationRepository,
            ICarrierRepository carrierRepository)
        {
            this.notificationRepository = notificationRepository;
            this.carrierRepository = carrierRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationRepository.GetById(notificationId);
            var carrierCollection = await carrierRepository.GetByNotificationId(notificationId);
            return new CarrierBlock(mergeFields, notification, carrierCollection);
        }
    }
}