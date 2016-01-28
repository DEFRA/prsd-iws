namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class ProducerBlockFactory : INotificationBlockFactory
    {
        private readonly IProducerRepository producerRepository;
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public ProducerBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            IProducerRepository producerRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.producerRepository = producerRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            var producer = await producerRepository.GetByNotificationId(notificationId);

            return new ProducerBlock(mergeFields, notification, producer);
        }
    }
}