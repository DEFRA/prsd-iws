namespace EA.Iws.Domain.NotificationApplication
{
    using System;
    using System.Threading.Tasks;

    public interface IMeansOfTransportRepository
    {
        Task<MeansOfTransport> GetByNotificationId(Guid notificationId);

        Task<MeansOfTransport> GetByMovementId(Guid movementId);

        void Add(MeansOfTransport meansOfTransport);
    }
}