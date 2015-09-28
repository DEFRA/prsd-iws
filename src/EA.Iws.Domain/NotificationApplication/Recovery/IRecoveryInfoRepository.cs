namespace EA.Iws.Domain.NotificationApplication.Recovery
{
    using System;
    using System.Threading.Tasks;

    public interface IRecoveryInfoRepository
    {
        Task<RecoveryInfo> GetByNotificationId(Guid notificationId);
    }
}
