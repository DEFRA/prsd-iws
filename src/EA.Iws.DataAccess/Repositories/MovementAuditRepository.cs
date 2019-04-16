namespace EA.Iws.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;
    using Prsd.Core.Domain;

    internal class MovementAuditRepository : IMovementAuditRepository
    {
        private readonly IwsContext context;
        private readonly INotificationApplicationAuthorization notificationApplicationAuthorization;
        private readonly IUserContext userContext;

        public MovementAuditRepository(IwsContext context,
            INotificationApplicationAuthorization notificationApplicationAuthorization,
            IUserContext userContext)
        {
            this.context = context;
            this.notificationApplicationAuthorization = notificationApplicationAuthorization;
            this.userContext = userContext;
        }

        public async Task Add(MovementAudit audit)
        {
            await notificationApplicationAuthorization.EnsureAccessAsync(audit.NotificationId);

            context.MovementAudits.Add(audit);
        }
    }
}
