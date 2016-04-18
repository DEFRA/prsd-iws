namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IWasteOperationRepository
    {
        Task<WasteOperation> GetByNotificationId(Guid notificationId);

        void Add(WasteOperation wasteOperation);
    }
}