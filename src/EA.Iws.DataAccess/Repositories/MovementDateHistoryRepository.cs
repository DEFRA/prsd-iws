namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;
    using Domain.Security;

    internal class MovementDateHistoryRepository : IMovementDateHistoryRepository
    {
        private readonly IMovementAuthorization authorization;
        private readonly IwsContext context;

        public MovementDateHistoryRepository(IwsContext context,
            IMovementAuthorization authorization)
        {
            this.context = context;
            this.authorization = authorization;
        }

        public async Task<IEnumerable<MovementDateHistory>> GetByMovementId(Guid movementId)
        {
            await authorization.EnsureAccessAsync(movementId);

            return await context.MovementDateHistories
                .Where(mdh => mdh.MovementId == movementId)
                .ToArrayAsync();
        }
    }
}