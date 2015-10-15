namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;

    internal class CarrierBlockFactory : INotificationBlockFactory
    {
        private readonly INotificationApplicationRepository repository;

        public CarrierBlockFactory(INotificationApplicationRepository repository)
        {
            this.repository = repository;
        }

        public async Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields)
        {
            var notification = await repository.GetById(notificationId);
            return new CarrierBlock(mergeFields, notification);
        }
    }
}