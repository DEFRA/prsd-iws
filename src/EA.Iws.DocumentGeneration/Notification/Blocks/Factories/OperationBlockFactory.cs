namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class OperationBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository notificationApplicationRepository;
        private readonly ITechnologyEmployedRepository technologyEmployedRepository;

        public OperationBlockFactory(INotificationApplicationRepository notificationApplicationRepository,
            ITechnologyEmployedRepository technologyEmployedRepository)
        {
            this.notificationApplicationRepository = notificationApplicationRepository;
            this.technologyEmployedRepository = technologyEmployedRepository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await notificationApplicationRepository.GetById(notificationId);
            var technologyEmployed = await technologyEmployedRepository.GetByNotificaitonId(notificationId);
            return new OperationBlock(mergeFields, notification, technologyEmployed);
        }
    }
}