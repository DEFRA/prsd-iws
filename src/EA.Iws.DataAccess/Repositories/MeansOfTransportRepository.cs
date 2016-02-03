namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication;
    using Domain.Security;

    internal class MeansOfTransportRepository : IMeansOfTransportRepository
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IwsContext context;

        public MeansOfTransportRepository(IwsContext context,
            INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public void Add(MeansOfTransport meansOfTransport)
        {
            context.MeansOfTransports.Add(meansOfTransport);
        }

        public async Task<MeansOfTransport> GetByMovementId(Guid movementId)
        {
            var notificationId = await context
                .Movements
                .Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await GetByNotificationId(notificationId);
        }

        public async Task<MeansOfTransport> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);

            return await context
                .MeansOfTransports
                .SingleOrDefaultAsync(x =>
                    x.NotificationId == notificationId);
        }
    }
}