namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Exporter;

    internal class ExporterRepository : IExporterRepository
    {
        private readonly IwsContext context;

        public ExporterRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Exporter> GetByNotificationId(Guid notificationId)
        {
            return await context.Exporters.SingleAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Exporter> GetExporterOrDefaultByNotificationId(Guid notificationId)
        {
            return await context.Exporters.SingleOrDefaultAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Exporter> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await context.Exporters.SingleAsync(e => e.NotificationId == notificationId);
        }

        public void Add(Exporter exporter)
        {
            context.Exporters.Add(exporter);
        }
    }
}
