namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface IProducerRepository
    {
        Task<ProducerCollection> GetByNotificationId(Guid notificationId);

        Task<ProducerCollection> GetByMovementId(Guid movementId);

        void Add(ProducerCollection producerCollection);
    }
}