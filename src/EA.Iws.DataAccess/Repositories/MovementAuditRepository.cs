namespace EA.Iws.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementAuditRepository : IMovementAuditRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;

        public MovementAuditRepository(IwsContext context,
            INotificationApplicationAuthorization notificationApplicationAuthorization)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
        }

        public async Task Add(MovementAudit audit)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(audit.NotificationId);

            context.MovementAudits.Add(audit);

            await context.SaveChangesAsync();
        }
    }
}
