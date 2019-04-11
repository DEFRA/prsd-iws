namespace EA.Iws.DataAccess.Repositories.Imports
{
    using System.Threading.Tasks;
    using Domain.ImportMovement;
    using Domain.Security;

    internal class ImportMovementAuditRepository : IImportMovementAuditRepository
    {
        private readonly ImportNotificationContext context;
        private readonly IImportNotificationApplicationAuthorization authorization;

        public ImportMovementAuditRepository(ImportNotificationContext context,
            IImportNotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task Add(ImportMovementAudit audit)
        {
            await authorization.EnsureAccessAsync(audit.NotificationId);

            context.ImportMovementAudits.Add(audit);

            await context.SaveChangesAsync();
        }
    }
}
