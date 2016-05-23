namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Shared;

    public interface IImportNotificationRepository
    {
        Task<bool> NotificationNumberExists(string number);

        Task<ImportNotification> Get(Guid id);

        Task Add(ImportNotification notification);

        Task<NotificationType> GetTypeById(Guid id);
    }
}
