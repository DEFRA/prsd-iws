namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IImportNotificationRepository
    {
        Task<bool> NotificationNumberExists(string number);

        Task<ImportNotification> GetByImportNotificationId(Guid id);

        Task Add(ImportNotification notification);
    }
}
