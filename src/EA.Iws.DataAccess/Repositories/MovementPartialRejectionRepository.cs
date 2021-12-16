namespace EA.Iws.DataAccess.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using Domain.Movement;

    internal class MovementPartialRejectionRepository : IMovementPartialRejectionRepository
    {
        private readonly IwsContext context;

        public MovementPartialRejectionRepository(IwsContext context)
        {
            this.context = context;
        }

        public async Task<MovementPartialRejection> GetByMovementId(Guid movementId)
        {
            return await context.MovementPartialRejections.SingleAsync(r => r.MovementId == movementId);
        }

        public async Task<MovementPartialRejection> Get(Guid id)
        {
            return await context.MovementPartialRejections.SingleAsync(r => r.Id == id);
        }

        public async Task<MovementPartialRejection> GetByMovementIdOrDefault(Guid movementId)
        {
            return await context.MovementPartialRejections.SingleOrDefaultAsync(m => m.MovementId == movementId);
        }

        public void Add(MovementPartialRejection movementPartialRejection)
        {
            context.MovementPartialRejections.Add(movementPartialRejection);
        }

        public async Task<IEnumerable<MovementPartialRejection>> GetMovementPartialRejectionsByMovementIds(IEnumerable<Guid> movementIds)
        {
            var movementsPartialRejection = await context.MovementPartialRejections
                .Where(m => movementIds.Contains(m.MovementId))
                .ToArrayAsync();

            return movementsPartialRejection;
        }
    }
}
