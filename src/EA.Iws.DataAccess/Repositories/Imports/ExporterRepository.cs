namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;
    using Domain.Security;

    internal class ExporterRepository : IExporterRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ExporterRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<Exporter> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.Exporters.SingleAsync(e => e.ImportNotificationId == notificationId);
        }

        public void Add(Exporter exporter)
        {
            context.Exporters.Add(exporter);
        }
    }
}