namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface IProducerRepository
    {
        Task<Producer> GetByNotificationId(Guid notificationId);

        void Add(Producer producer);
    }
}