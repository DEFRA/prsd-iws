namespace EA.Iws.DocumentGeneration.Notification.Blocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface INotificationBlockFactory
    {
        Task<IDocumentBlock> Create(Guid notificationId, IList<MergeField> mergeFields);
    }
}