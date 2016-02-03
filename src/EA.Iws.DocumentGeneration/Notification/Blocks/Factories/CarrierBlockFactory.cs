namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class CarrierBlockFactory : INotificationBlockFactory
    {
        private readonly IMeansOfTransportRepository meansOfTransportRepository;
        private readonly ICarrierRepository carrierRepository;

        public CarrierBlockFactory(IMeansOfTransportRepository meansOfTransportRepository,
            ICarrierRepository carrierRepository)
        {
            this.carrierRepository = carrierRepository;
            this.meansOfTransportRepository = meansOfTransportRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var meansOfTransport = await meansOfTransportRepository.GetByNotificationId(notificationId);
            var carrierCollection = await carrierRepository.GetByNotificationId(notificationId);
            return new CarrierBlock(mergeFields, meansOfTransport, carrierCollection);
        }
    }
}