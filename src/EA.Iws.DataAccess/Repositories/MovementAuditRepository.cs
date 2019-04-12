namespace EA.Iws.DataAccess.Repositories
{
    using System.Threading.Tasks;
    using Core.Movement;
    using Domain.Movement;
    using Domain.Security;
    using Prsd.Core;
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

            await context.SaveChangesAsync();
        }

        public async Task Add(Movement movement, MovementAuditType type)
        {
            var movementAudit = new MovementAudit(movement.NotificationId, movement.Number,
                userContext.UserId.ToString(), (int)type, SystemTime.UtcNow);

            await Add(movementAudit);
        }
    }
}
