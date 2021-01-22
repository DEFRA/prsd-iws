namespace EA.Iws.Domain.TransportRoute
{
    using EA.Iws.Core.Notification;
    using System;
    using System.Threading.Tasks;

    public interface ITransportRouteRepository
    {
        Task<TransportRoute> GetByNotificationId(Guid notificationId);

        Task<UKCompetentAuthority> GetNotificationCompetentAuthorityById(Guid notificationId);
    }
}