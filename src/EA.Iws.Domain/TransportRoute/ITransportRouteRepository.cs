namespace EA.Iws.Domain.TransportRoute
{
    using System;
    using System.Threading.Tasks;

    public interface ITransportRouteRepository
    {
        Task<TransportRoute> GetByNotificationId(Guid notificationId);
    }
}