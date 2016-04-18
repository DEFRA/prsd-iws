namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementDetailsRepository : IMovementDetailsRepository
    {
        private readonly IMovementAuthorization authorization;
        private readonly IwsContext context;

        public MovementDetailsRepository(IwsContext context,
            IMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<MovementDetails> GetByMovementId(Guid movementId)
        {
            await authorization.EnsureAccessAsync(movementId);

            return await context.MovementDetails
                .SingleOrDefaultAsync(md => md.MovementId == movementId);
        }
    }
}