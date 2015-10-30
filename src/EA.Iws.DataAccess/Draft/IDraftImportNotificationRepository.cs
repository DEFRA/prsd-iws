namespace EA.Iws.DataAccess.Draft
{
    using System;
    using System.Threading.Tasks;

    public interface IDraftImportNotificationRepository
    {
        Task SetDraftData<TData>(Guid importNotificationId, TData data);

        Task<TData> GetDraftData<TData>(Guid importNotificationId);
    }
}