namespace EA.Iws.Domain.NotificationApplication.Annexes
{
    using System;
    using System.Threading.Tasks;

    public interface IAnnexCollectionRepository
    {
        Task<AnnexCollection> GetByNotificationId(Guid notificationId);

        void Add(AnnexCollection annexCollection);
    }
}
