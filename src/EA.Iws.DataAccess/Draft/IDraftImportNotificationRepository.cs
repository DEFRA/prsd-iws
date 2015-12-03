namespace EA.Iws.DataAccess.Draft
{
    using System;
    using System.Threading.Tasks;
    using Core.ImportNotification.Draft;

    public interface IDraftImportNotificationRepository
    {
        Task SetDraftData<TData>(Guid importNotificationId, TData data);

        Task<TData> GetDraftData<TData>(Guid importNotificationId);

        Task<ImportNotification> Get(Guid importNotificationId); 
    }
}