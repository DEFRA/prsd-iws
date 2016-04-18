namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IImportNotificationOverviewRepository
    {
        Task<ImportNotificationOverview> GetFromDraft(Guid notificationId);

        Task<ImportNotificationOverview> Get(Guid notificationId);
    }
}