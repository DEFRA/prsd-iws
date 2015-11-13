namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementDetailsRepository : IMovementDetailsRepository
    {
        private readonly INotificationApplicationAuthorization authorization;
        private readonly IwsContext context;

        public MovementDetailsRepository(IwsContext context,
            INotificationApplicationAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<MovementDetails> GetByMovementId(Guid movementId)
        {
            var result = await context.MovementDetails
                .Join(
                    context.Movements,
                    md => md.MovementId,
                    m => m.Id,
                    (md, m) => new { Movement = m, MovementDetails = md })
                .SingleOrDefaultAsync(j => j.Movement.Id == movementId);

            await authorization.EnsureAccessAsync(result.Movement.NotificationId);

            return result.MovementDetails;
        }
    }
}