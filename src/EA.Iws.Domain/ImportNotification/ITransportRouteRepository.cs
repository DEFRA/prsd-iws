namespace EA.Iws.Domain.ImportNotification
{
    using System;
    using System.Threading.Tasks;

    public interface ITransportRouteRepository
    {
        Task<TransportRoute> GetByNotificationId(Guid notificationId);

        void Add(TransportRoute transportRoute);
    }
}