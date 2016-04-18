namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IImporterRepository
    {
        Task<Importer> GetByNotificationId(Guid notificationId);

        void Add(Importer importer);
    }
}