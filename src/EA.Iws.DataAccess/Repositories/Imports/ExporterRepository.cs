namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class ExporterRepository : IExporterRepository
    {
        private readonly ImportNotificationContext context;

        public ExporterRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<Exporter> GetByNotificationId(Guid notificationId)
        {
            return await context.Exporters.SingleAsync(e => e.ImportNotificationId == notificationId);
        }

        public void Add(Exporter exporter)
        {
            context.Exporters.Add(exporter);
        }
    }
}