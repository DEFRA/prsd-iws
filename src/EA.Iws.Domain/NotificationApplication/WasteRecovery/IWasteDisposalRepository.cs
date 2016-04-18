namespace EA.Iws.Domain.NotificationApplication.WasteRecovery
{
    using System;
    using System.Threading.Tasks;

    public interface IWasteDisposalRepository
    {
        Task<WasteDisposal> GetByNotificationId(Guid notificationId);

        void Delete(WasteDisposal wasteDisposal);
    }
}
