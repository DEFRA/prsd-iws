namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Decision;
    using Domain.Security;

    internal class ImportWithdrawnRepository : IImportWithdrawnRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportWithdrawnRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportWithdrawn> GetByNotificationIdOrDefault(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportWithdrawals.SingleOrDefaultAsync(w => w.NotificationId == notificationId);
        }

        public async Task<ImportWithdrawn> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportWithdrawals.SingleAsync(w => w.NotificationId == notificationId);
        }

        public void Add(ImportWithdrawn importWithdrawn)
        {
            context.ImportWithdrawals.Add(importWithdrawn);
        }
    }
}
