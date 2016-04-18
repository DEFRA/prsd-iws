namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotification;

    internal class ImporterRepository : IImporterRepository
    {
        private readonly ImportNotificationContext context;

        public ImporterRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<Importer> GetByNotificationId(Guid notificationId)
        {
            return await context.Importers.SingleAsync(i => i.ImportNotificationId == notificationId);
        }

        public void Add(Importer importer)
        {
            context.Importers.Add(importer);
        }
    }
}