namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class FacilityBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;

        public FacilityBlockFactory(INotificationApplicationRepository notificationApplicationRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            return new FacilityBlock(mergeFields, notification);
        }
    }
}