namespace EA.Iws.Domain.NotificationApplication.WasteRecovery
{
    using System;
    using System.Threading.Tasks;

    public interface IWasteRecoveryRepository
    {
        Task<WasteRecovery> GetByNotificationId(Guid notificationId);

        void Delete(WasteRecovery wasteRecovery);
    }
}
