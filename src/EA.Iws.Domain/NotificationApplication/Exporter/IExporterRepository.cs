namespace EA.Iws.Domain.NotificationApplication.Exporter
{
    using System;
    using System.Threading.Tasks;

    public interface IExporterRepository
    {
        Task<Exporter> GetByNotificationId(Guid notificationId);

        Task<Exporter> GetExporterOrDefaultByNotificationId(Guid notificationId);

        Task<Exporter> GetByMovementId(Guid movementId);

        void Add(Exporter exporter);
    }
}
