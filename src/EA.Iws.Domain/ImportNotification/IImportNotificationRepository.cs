namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;
    using Core.Shared;

    public interface IImportNotificationRepository
    {
        Task<bool> NotificationNumberExists(string number);

        Task<ImportNotification> Get(Guid id);

        Task Add(ImportNotification notification);

        Task<NotificationType> GetTypeById(Guid id);

        Task<Guid?> GetIdOrDefault(string number);

        Task Delete(Guid notificationId);
    }
}
