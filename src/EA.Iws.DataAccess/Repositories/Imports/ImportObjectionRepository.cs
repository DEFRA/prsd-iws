namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.ImportNotificationAssessment.Decision;
    using Domain.Security;

    internal class ImportObjectionRepository : IImportObjectionRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportObjectionRepository(ImportNotificationContext context, IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<ImportObjection> GetByNotificationId(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportObjections.SingleAsync(o => o.NotificationId == notificationId);
        }

        public async Task<ImportObjection> GetByNotificationIdOrDefault(Guid notificationId)
        {
            await authorization.EnsureAccessAsync(notificationId);
            return await context.ImportObjections.SingleAsync(o => o.NotificationId == notificationId);
        }

        public void Add(ImportObjection importObjection)
        {
            context.ImportObjections.Add(importObjection);
        }
    }
}
