namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class ImporterBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public ImporterBlockFactory(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            return new ImporterBlock(mergeFields, notification);
        }
    }
}