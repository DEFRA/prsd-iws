namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IExporterRepository
    {
        Task<Exporter> GetByNotificationId(Guid notificationId);

        void Add(Exporter exporter);
    }
}