namespace EA.Iws.Domain.NotificationApplication.Importer
{
    using System;
    using System.Threading.Tasks;

    public interface IImporterRepository
    {
        Task<Importer> GetByNotificationId(Guid notificationId);

        Task<Importer> GetImporterOrDefaultByNotificationId(Guid notificationId);

        Task<Importer> GetByMovementId(Guid movementId);

        void Add(Importer importer);
    }
}