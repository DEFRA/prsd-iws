namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class TransportRouteRepository : ITransportRouteRepository
    {
        private readonly ImportNotificationContext context;

        public TransportRouteRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<TransportRoute> GetByNotificationId(Guid notificationId)
        {
            return await context.TransportRoutes.SingleAsync(t => t.ImportNotificationId == notificationId);
        }

        public void Add(TransportRoute transportRoute)
        {
            context.TransportRoutes.Add(transportRoute);
        }
    }
}