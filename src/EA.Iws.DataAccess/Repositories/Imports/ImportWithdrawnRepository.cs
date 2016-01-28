namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Decision;

    internal class ImportWithdrawnRepository : IImportWithdrawnRepository
    {
        private readonly ImportNotificationContext context;

        public ImportWithdrawnRepository(ImportNotificationContext context)
        {
            this.context = context;
        }

        public async Task<ImportWithdrawn> GetByNotificationIdOrDefault(Guid notificationId)
        {
            return await context.ImportWithdrawals.SingleOrDefaultAsync(w => w.NotificationId == notificationId);
        }

        public async Task<ImportWithdrawn> GetByNotificationId(Guid notificationId)
        {
            return await context.ImportWithdrawals.SingleAsync(w => w.NotificationId == notificationId);
        }

        public void Add(ImportWithdrawn importWithdrawn)
        {
            context.ImportWithdrawals.Add(importWithdrawn);
        }
    }
}
