namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Decision;

    internal class ImportObjectionRepository : IImportObjectionRepository
    {
        private readonly ImportNotificationContext context;

        public ImportObjectionRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportObjection> GetByNotificationId(Guid notificationId)
        {
            return await context.ImportObjections.SingleAsync(o => o.NotificationId == notificationId);
        }

        public async Task<ImportObjection> GetByNotificationIdOrDefault(Guid notificationId)
        {
            return await context.ImportObjections.SingleAsync(o => o.NotificationId == notificationId);
        }

        public void Add(ImportObjection importObjection)
        {
            context.ImportObjections.Add(importObjection);
        }
    }
}
