namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IInterimStatusRepository
    {
        Task<InterimStatus> GetByNotificationId(Guid notificationId);

        Task<InterimStatus> GetByNotificationIdOrDefault(Guid notificationId);
        
        void Add(InterimStatus interimStatus);
    }
}
