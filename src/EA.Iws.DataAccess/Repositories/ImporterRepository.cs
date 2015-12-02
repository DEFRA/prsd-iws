namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Importer;
    using Domain.Security;

    internal class ImporterRepository : IImporterRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public ImporterRepository(IwsContext context, INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task<Importer> GetByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Importers.SingleAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Importer> GetImporterOrDefaultByNotificationId(Guid notificationId)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Importers.SingleOrDefaultAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Importer> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            await notificationApplicationAuthorization.EnsureAccessAsync(notificationId);
            return await context.Importers.SingleAsync(e => e.NotificationId == notificationId);
        }

        public void Add(Importer importer)
        {
            context.Importers.Add(importer);
        }
    }
}