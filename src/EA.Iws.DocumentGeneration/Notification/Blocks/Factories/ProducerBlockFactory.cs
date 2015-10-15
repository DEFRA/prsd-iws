namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class ProducerBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public ProducerBlockFactory(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            return new ProducerBlock(mergeFields, notification);
        }
    }
}