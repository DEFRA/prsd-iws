namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Exporter;
    using Domain.Security;

    internal class ExporterRepository : IExporterRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public ExporterRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<Exporter> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Exporters.SingleAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Exporter> GetExporterOrDefaultByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Exporters.SingleOrDefaultAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Exporter> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Exporters.SingleAsync(e => e.NotificationId == notificationId);
        }

        public void Add(Exporter exporter)
        {
            context.Exporters.Add(exporter);
        }
    }
}
