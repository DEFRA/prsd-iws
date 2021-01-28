namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Security;
    using Domain.TransportRoute;
    using EA.Iws.Core.Notification;

    internal class TransportRouteRepository : ITransportRouteRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public TransportRouteRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<TransportRoute> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.TransportRoutes.SingleOrDefaultAsync(p => p.NotificationId == notificationId);
        }        
    }
}