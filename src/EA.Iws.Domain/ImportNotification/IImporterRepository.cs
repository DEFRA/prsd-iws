namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    internal interface IImporterRepository
    {
        Task<Importer> GetByNotificationId(Guid notificationId);

        void Add(Importer importer);
    }
}