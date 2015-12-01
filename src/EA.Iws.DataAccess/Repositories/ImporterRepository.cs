namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.NotificationApplication.Importer;

    internal class ImporterRepository : IImporterRepository
    {
        private readonly IwsContext context;

        public ImporterRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<Importer> GetByNotificationId(Guid notificationId)
        {
            return await context.Importers.SingleAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Importer> GetImporterOrDefaultByNotificationId(Guid notificationId)
        {
            return await context.Importers.SingleOrDefaultAsync(e => e.NotificationId == notificationId);
        }

        public async Task<Importer> GetByMovementId(Guid movementId)
        {
            var notificationId =
                await context.Movements.Where(m => m.Id == movementId)
                .Select(m => m.NotificationId)
                .SingleAsync();

            return await context.Importers.SingleAsync(e => e.NotificationId == notificationId);
        }

        public void Add(Importer importer)
        {
            context.Importers.Add(importer);
        }
    }
}